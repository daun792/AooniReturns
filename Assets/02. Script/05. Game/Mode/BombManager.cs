using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BombManager : GameManager
{
    protected override void Awake()
    {
        base.Awake();

        MaxRoundCount = 3;
        GameTime = 60;
    }

    protected override bool CheckVictoryCondition()
    {
        if (App.Manager.UI.GetPanel<TimePanel>().Remaining <= 0f)
        {
            RPC_AddHumanSurviveScore();
            return true;
        }

        return false;
    }

    [Rpc]
    private void RPC_AddHumanSurviveScore()
    {
        foreach (var player in App.Manager.Player.HumanPlayers)
        {
            player.AddSurviveScore();
        }
    }
}
