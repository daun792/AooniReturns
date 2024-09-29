using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanel : UIBase
{
    private ResultPlayerInfoBack[] playerInfos;

    public override void Init()
    {
        playerInfos = GetComponentsInChildren<ResultPlayerInfoBack>(true);
    }

    public override void OpenPanel()
    {
        base.OpenPanel();

        var resultRankList = App.Manager.Player.AllPlayers.OrderBy(x => x.OniKill + x.HumanKill + x.Survive).ToList();

        int i = 0;

        for (; i < resultRankList.Count; i++)
        {
            playerInfos[i].Setup(resultRankList[i]);
        }


        for (; i < playerInfos.Length; i++)
        {
            playerInfos[i].SetNone();
        }
    }
}
