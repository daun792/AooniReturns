using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class ReadyManager : SimManager, IPlayerJoined, IPlayerLeft
{
    [Header("Network Objects")]
    [SerializeField] NetworkObject playerPrefab;

    private void Start()
    {
        Runner.SpawnAsync(playerPrefab, Vector3.zero, Quaternion.identity,
            Runner.LocalPlayer, null, NetworkSpawnFlags.SharedModeStateAuthLocalPlayer);
    }

    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        App.UI.Ready.SetPlayerCount();
    }

    void IPlayerLeft.PlayerLeft(PlayerRef _player)
    {
        App.UI.Ready.SetPlayerCount();
    }
}
