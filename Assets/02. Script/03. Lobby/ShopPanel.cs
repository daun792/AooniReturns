using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : UIBase
{
    public override UIState GetUIState() => UIState.Shop;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {
        return;
    }

    public override void OpenPanel()
    {
        if (App.UI.Lobby.CurrState == UIState.CreateRoom)
        {
            App.UI.Lobby.GetPanel<CreateRoomPanel>().ClosePanel();
        }

        base.OpenPanel();
    }
}
