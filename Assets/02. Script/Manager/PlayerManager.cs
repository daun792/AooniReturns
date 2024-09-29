using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerManager : NetManager, IPlayerJoined, IPlayerLeft
{
    public List<NetworkObject> networkObjList = new(8);
    public List<CharacterCtrl> characterList = new(8);

    public CharacterCtrl MyCtrl { get; private set; }

    public IReadOnlyList<CharacterCtrl> AllPlayers => characterList;

    public IReadOnlyList<CharacterCtrl> HumanPlayers => characterList.Where(x => x.CurrState == CharacterType.Human).ToList();
    public IReadOnlyList<CharacterCtrl> OniPlayers => characterList.Where(x => x.CurrState == CharacterType.Oni).ToList();

    public void SubmitPlayer(CharacterCtrl _char)
    {
        if (characterList.Contains(_char))
        {
            return;
        }

        characterList.Add(_char);
        networkObjList.Add(_char.Object);

        if (_char.Object.StateAuthority.PlayerId == App.Manager.Network.Runner.LocalPlayer.PlayerId)
        {
            MyCtrl = _char;
        }
    }

    public void SetRandomOni(int _num)
    {
        var availablePlayers = networkObjList.ToList();

        List<NetworkId> oniIds = new();

        for (int i = 0; i < _num; i++)
        {
            if (availablePlayers.Count == 0)
            {
                break;
            }

            int randomIndex = Random.Range(0, availablePlayers.Count);
            var playerIndex = availablePlayers[randomIndex];

            availablePlayers.RemoveAt(randomIndex);

            oniIds.Add(playerIndex.Id);
        }

        RPC_SetOni(oniIds.ToArray());
    }

    [Rpc]
    private void RPC_SetOni(NetworkId[] characterNetworkIds)
    {
        foreach (var characterNetworkId in characterNetworkIds)
        {
            for (int i = 0; i < networkObjList.Count; i++)
            {
                if (networkObjList[i].Id == characterNetworkId)
                {
                    characterList[i].SetCharacterState(1);
                    break; 
                }
            }
        }
    }

    public void SetAllHuman()
    {
        RPC_SetHuman();
    }

    [Rpc]
    private void RPC_SetHuman()
    {
        foreach (var character in characterList)
        {
            character.SetCharacterState(0);
        }
    }

    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        StartCoroutine(WaitForCharCtrl(player));
    }

    private IEnumerator WaitForCharCtrl(PlayerRef player)
    {
        yield return new WaitUntil(() => Runner.GetPlayerObject(player) != null);

        var playerObj = Runner.GetPlayerObject(player);

        yield return new WaitUntil(() => playerObj.GetComponent<CharacterCtrl>() != null);

        App.Manager.UI.GetPanel<PlayerInfoPanel>().Setup();
    }

    void IPlayerLeft.PlayerLeft(PlayerRef player)
    {
        var playerObj = Runner.GetPlayerObject(player);

        for (int i = 0; i < networkObjList.Count; i++)
        {
            int index = i;

            if (networkObjList[index].Id == playerObj.Id)
            {
                networkObjList.RemoveAt(index);
                characterList.RemoveAt(index);
            }
        }

        App.Manager.UI.GetPanel<PlayerInfoPanel>().Setup();
    }
}