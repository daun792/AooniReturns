using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoPanel : UIBase
{
    private PlayerInfoBack[] playerInfos;

    public override void Init()
    {
        playerInfos = GetComponentsInChildren<PlayerInfoBack>(true);
    }

    public void Setup()
    {
        var players = App.Manager.Player.AllPlayers;

        int i = 0;

        for (; i < players.Count; i++)
        {
            playerInfos[i].SetPlayer(players[i]);
        }

        for (; i < playerInfos.Length; i++)
        {
            playerInfos[i].SetNone();
        }
    }
}
