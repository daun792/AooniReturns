using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System.Collections.Generic;

public class ReadyManager : SimManager, IPlayerLeft
{
    [Header("Network Objects")]
    [SerializeField] NetworkObject playerPrefab;

    [Header("Panel Buttons")]
    [SerializeField] Button btn_GameStart;
    [SerializeField] Button btn_GameExit;
    [SerializeField] GameObject loadingPanel;

    private Dictionary<PlayerRef, NetworkObject> playerList;

    private void Start()
    {
        playerList = new(4);
        Runner.SpawnAsync(playerPrefab);

        //btn_GameStart.onClick.AddListener(StartGameBtnEvent);
        //btn_GameExit.onClick.AddListener(ExitGameBtnEvent);
    }

    private void StartGameBtnEvent()
    {
        App.Manager.Network.StartGame();
        loadingPanel.SetActive(true);
    }

    private void ExitGameBtnEvent()
    {
        App.Manager.Network.LeaveMatch();
    }

    //public void SubmitPreviewObject(NetworkObject _netObj)
    //{
    //    _netObj.transform.SetParent(playerPreviewTrans[playerList.Count]);
    //    _netObj.transform.localPosition = Vector3.zero;
    //    playerList.Add(_netObj.StateAuthority, _netObj);
    //    RefreshPlayerPreview();
    //}

    //private void RefreshPlayerPreview()
    //{
    //    var idx = 0;
    //    var maxPlayerCount = Runner.SessionInfo.MaxPlayers;

    //    foreach (var netObj in playerList.Values)
    //    {
    //        playerMugshot[idx].SetPlayer(netObj.GetComponent<PreviewCtrl>());
    //        netObj.transform.SetParent(playerPreviewTrans[idx]);
    //        ++idx;
    //    }

    //    for (int i = playerList.Count; i < maxPlayerCount; ++i)
    //    {
    //        playerMugshot[i].SetEmpty();
    //    }

    //    btn_GameStart.gameObject.SetActive(Runner.IsSceneAuthority);
    //    if (Runner.IsSceneAuthority)
    //    {
    //        btn_GameStart.interactable = true;
    //        // btn_GameStart.interactable = currPlayerCount == maxPlayerCount;
    //    }

    //    loadingPanel.SetActive(false);
    //}

    void IPlayerLeft.PlayerLeft(PlayerRef _player)
    {
        for (int i = 0; i < playerList.Count; ++i)
        {
            playerList.Remove(_player);
        }

        //RefreshPlayerPreview();
    }
}
