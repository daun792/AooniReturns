using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System.Collections.Generic;

public class ReadyManager : SimManager, IPlayerLeft
{
    [Header("Network Objects")]
    [SerializeField] NetworkObject playerPrefab;

    [Header("Panel Buttons")]
    [SerializeField] Button startBtn;
    [SerializeField] Button exitBtn;
    [SerializeField] GameObject loadingPanel;

    private Dictionary<PlayerRef, NetworkObject> playerList;

    private void Start()
    {
        playerList = new(8);
        Runner.SpawnAsync(playerPrefab);

        startBtn.onClick.AddListener(OnClickStart);
        exitBtn.onClick.AddListener(OnClickExit);
    }

    private void OnClickStart()
    {
        App.Manager.Network.StartGame();
        loadingPanel.SetActive(true);
    }

    private void OnClickExit()
    {
        App.Manager.Network.LeaveMatch();
    }

    void IPlayerLeft.PlayerLeft(PlayerRef _player)
    {
        for (int i = 0; i < playerList.Count; ++i)
        {
            playerList.Remove(_player);
        }

        //Runner.SessionInfo.SetSessionProperty("CurrentPlayers", currentPlayers - 1);

        //RefreshPlayerPreview();
    }
}
