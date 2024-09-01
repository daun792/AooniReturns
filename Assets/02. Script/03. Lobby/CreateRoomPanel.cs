using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : UIBase
{
    public override UIState GetUIState() => UIState.CreateRoom;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {
        return;
    }
}