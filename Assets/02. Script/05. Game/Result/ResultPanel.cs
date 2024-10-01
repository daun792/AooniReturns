using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : UIBase
{
    [SerializeField] Button continueBtn;

    private ResultPlayerInfoBack[] playerInfos;

    public override void Init()
    {
        playerInfos = GetComponentsInChildren<ResultPlayerInfoBack>(true);

        continueBtn.onClick.AddListener(() => App.Manager.Network.JoinLobby());
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
