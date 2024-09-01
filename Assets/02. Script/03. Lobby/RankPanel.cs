using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : UIBase
{
    public override UIState GetUIState() => UIState.Rank;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {
        return;
    }
}
