using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class ReadyManager : SimManager, IPlayerJoined, IPlayerLeft
{
    [Header("Network Objects")]
    [SerializeField] NetworkObject playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> playerList;

    private void Start()
    {
        playerList = new(8);
        Runner.SpawnAsync(playerPrefab);
    }

    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        App.UI.Ready.SetPlayerCount();
    }

    void IPlayerLeft.PlayerLeft(PlayerRef _player)
    {
        for (int i = 0; i < playerList.Count; ++i)
        {
            playerList.Remove(_player);
        }

        App.UI.Ready.SetPlayerCount();
    }
}
