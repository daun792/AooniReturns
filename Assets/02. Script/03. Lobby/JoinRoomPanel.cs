using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomPanel : UIBase
{
    public override UIState GetUIState() => UIState.JoinRoom;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {
        return;
    }
}
