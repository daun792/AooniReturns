using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePanel : UIBase
{
    [SerializeField] TextMeshProUGUI scoreTMP;

    private CharacterCtrl myCharCtrl;

    private const string scoreString = "오니 {0} 히로시 {1} 생존 {2}";

    public override void Init()
    {
        StartCoroutine(WaitForMyChar());
    }

    private IEnumerator WaitForMyChar()
    {
        yield return new WaitUntil(() => App.Manager.Player.MyCtrl != null);

        myCharCtrl = App.Manager.Player.MyCtrl;

        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreTMP.text = string.Format(scoreString, myCharCtrl.OniKill, myCharCtrl.HumanKill, myCharCtrl.Survive);
    }
}
