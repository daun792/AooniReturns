using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPanel : UIBase
{
    [SerializeField] TextMeshProUGUI nickTMP;

    [SerializeField] TextMeshProUGUI recordTMP;
    [SerializeField] TextMeshProUGUI clanTMP;
    [SerializeField] TextMeshProUGUI rankTMP;
    [SerializeField] TextMeshProUGUI goldTMP;

    [SerializeField] TextMeshProUGUI levelTMP;
    [SerializeField] TextMeshProUGUI expTMP;
    [SerializeField] Image expImg;

    public override void Init()
    {
        nickTMP.text = App.Data.Player.NickName;
        recordTMP.text = string.Format("{0}오니 / {1}히로시 / {2}생존", App.Data.Player.OniKills, App.Data.Player.HiroshiKills, App.Data.Player.SurvivalCount);
        clanTMP.text = string.Format("클랜 <color=#00FF00>{0}</color>", App.Data.Player.Clan); 
        rankTMP.text = string.Format("전체 랭킹 <color=#00FF00>{0}</color>", 1);
        goldTMP.text = App.Data.Player.Currency.ToString();

        CalculateLevel();
    }

    private void CalculateLevel()
    {
        levelTMP.text = ((App.Data.Player.ExperiencePoints / 450f) + 1).ToString();

        var remainEXP = App.Data.Player.ExperiencePoints % 450f;
        var percentage = remainEXP / 450f * 100;
        expTMP.text = string.Format("{0}/450({1}%)", remainEXP, Mathf.Round(percentage * 100f) / 100f);

        expImg.fillAmount = remainEXP / 450f;
    }
}
