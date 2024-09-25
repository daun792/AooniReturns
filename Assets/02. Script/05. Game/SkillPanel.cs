using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : UIBase
{
    [SerializeField] Button arrowBtn;
    [SerializeField] Button barrelBtn;

    public override void Init()
    {
        arrowBtn.onClick.AddListener(OnClickArrow);
        barrelBtn.onClick.AddListener(OnClickBarrel);
    }

    private void OnClickArrow()
    {
        App.Manager.Player.MyCtrl.arrow.FireArrow();
    }

    private void OnClickBarrel()
    {

    }
}
