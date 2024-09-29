using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class SwitchUICtrl : NetworkBehaviour
{
    [SerializeField] Image hpFillImg;

    public void SetHP(float _value)
    {
        hpFillImg.fillAmount = _value / 50f;
    }
}