using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class InfectionManager : GameManager
{
    protected override void Awake()
    {
        base.Awake();

        MaxRoundCount = 2;
        GameTime = 120;
    }

    protected override void SetRandomOni()
    {
        base.SetRandomOni();

        //App.Manager.UI.GetPanel<NoticePanel>().NoticeBecomeOni();
    }

    protected override bool CheckVictoryCondition()
    {
        if (App.Manager.Player.OniPlayers.Count == App.Manager.Player.AllPlayers.Count)
        {
            App.Manager.UI.Chat.SendNotice($"<color=#00FF00>아오오니의 승리!</color>", true);
            return true;
        }

        if (CheckOniAllDead())
        {
            App.Manager.UI.Chat.SendNotice($"<color=#00FF00>인간의 승리!</color>", true);
            return true;
        }

        if (App.Manager.UI.GetPanel<TimePanel>().Remaining <= 0f)
        {
            App.Manager.UI.Chat.SendNotice($"<color=#00FF00>인간의 승리!</color>", true);
            RPC_AddHumanSurviveScore();
            return true;
        }

        return false;
    }

    private bool CheckOniAllDead()
    {
        if (App.Manager.Player.OniPlayers.Count == 0)
        {
            return false;
        }

        foreach (var charCtrl in App.Manager.Player.OniPlayers)
        {
            if (charCtrl.IsDead == true)
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
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
