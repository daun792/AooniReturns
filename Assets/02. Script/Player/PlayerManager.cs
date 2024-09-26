using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class PlayerManager : Manager
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
}