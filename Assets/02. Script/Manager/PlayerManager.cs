using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class PlayerManager : NetManager
{
    private readonly List<CharacterCtrl> characterList = new(8);

    public CharacterCtrl MyCtrl { get; private set; }

    public IReadOnlyList<CharacterCtrl> AllPlayers => characterList;

    public IReadOnlyList<CharacterCtrl> HumanPlayers => characterList.Where(x => x.CurrState == CharacterType.Human).ToList();
    public IReadOnlyList<CharacterCtrl> OniPlayers => characterList.Where(x => x.CurrState == CharacterType.Oni).ToList();

    public void SubmitPlayer(CharacterCtrl _char)
    {
        characterList.Add(_char);

        if (_char.Object.StateAuthority.PlayerId == App.Manager.Network.Runner.LocalPlayer.PlayerId)
        {
            MyCtrl = _char;
        }
    }

    public void SetRandomOni()
    {
        var randomIndex = Random.Range(0, AllPlayers.Count);

        RPC_SetOni(AllPlayers[randomIndex]);
    }

    [Rpc]
    private void RPC_SetOni(CharacterCtrl _charCtrl)
    {
        _charCtrl.SetCharacterState(1);

        App.Manager.UI.GetPanel<NoticePanel>().NoticeBecomeOni();
    }

    public void SetAllHuman()
    {
        foreach (var charCtrl in AllPlayers)
        {
            charCtrl.SetCharacterState(0);
        }
    }
}