using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundPanel : UIBase
{
    [SerializeField] TextMeshProUGUI roundTMP;

    private const string roundTextFormat = "ROUND {0} / 4";

    public override void Init()
    {
        roundTMP.text = string.Format(roundTextFormat, 1);
    }

    public void OpenPanel(int _roundCount)
    {
        roundTMP.text = string.Format(roundTextFormat, _roundCount);

        base.OpenPanel();
    }
}
