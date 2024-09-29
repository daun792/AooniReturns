using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionManager : GameManager
{
    protected override void Awake()
    {
        base.Awake();

        MaxRoundCount = 8;
        GameTime = 120;
    }

    protected override void SetRandomOni()
    {
        base.SetRandomOni();

        App.Manager.UI.GetPanel<NoticePanel>().NoticeBecomeOni();
    }

    protected override bool CheckVictoryCondition()
    {
        if (App.Manager.Player.OniPlayers.Count == App.Manager.Player.AllPlayers.Count)
        {
            return true;
        }

        if (CheckOniAllDead())
        {
            return true;
        }

        if (App.Manager.UI.GetPanel<TimePanel>().Remaining <= 0f)
        {
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
}
