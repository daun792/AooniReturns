using UnityEngine;
using System.Collections;
using Fusion;
using System.Collections.Generic;

public class ReadyManager : SimManager, IPlayerJoined, IPlayerLeft
{
    [Header("Network Objects")]
    [SerializeField] NetworkObject playerPrefab;
    [SerializeField] NetworkObject chatPrefab;

    private void Start()
    {
        var playerObject = Runner.SpawnAsync(playerPrefab);

        var chatObj = App.Manager.Network.Runner.Spawn(chatPrefab);
        chatObj.transform.SetParent(App.Manager.UI.transform);
        chatObj.GetComponent<RectTransform>().anchoredPosition = new(0, 100);
        App.UI.Ready.SetChatPanel(chatObj.GetComponent<ChatPanel>());
    }

    void IPlayerJoined.PlayerJoined(PlayerRef player)
    {
        App.UI.Ready.SetPlayerCount();

        StartCoroutine(WaitForCharCtrl(player));
    }

    private IEnumerator WaitForCharCtrl(PlayerRef player)
    {
        yield return new WaitUntil(() => Runner.GetPlayerObject(player) != null);

        var playerObj = Runner.GetPlayerObject(player);

        yield return new WaitUntil(() => playerObj.GetComponent<CharacterCtrl>() != null);

        var charCtrl = playerObj.GetComponent<CharacterCtrl>();
        App.Manager.UI.Chat.SendNotice($"<color=#00FF00>{charCtrl.NickName}님이 게임에 입장하셨습니다.</color>");
        App.Manager.UI.GetPanel<PlayerInfoPanel>().Setup();
    }

    void IPlayerLeft.PlayerLeft(PlayerRef _player)
    {
        App.UI.Ready.SetPlayerCount();

        App.Manager.Player.OnPlayerLeft(_player);
    }
}
