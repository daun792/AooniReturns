using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanel : UIBase
{
    public override UIState GetUIState() => UIState.Option;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {
        return;
    }
}