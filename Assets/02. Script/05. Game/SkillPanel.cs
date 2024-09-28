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
        StartCoroutine(WaitForCoolTime());
        App.Manager.Player.MyCtrl.arrow.FireArrow();
    }

    private void OnClickBarrel()
    {

    }

    private IEnumerator WaitForCoolTime()
    {
        arrowBtn.enabled = false;
        arrowBtn.image.color = new Color(1, 1, 1, 0.5f);

        float time = 0;

        while (time <= 1)
        {
            time += Time.deltaTime;

            arrowBtn.image.fillAmount = time / 1f;

            yield return null;
        }

        arrowBtn.enabled = true;
        arrowBtn.image.color = Color.white;
    }
}
