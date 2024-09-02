using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanPanel : UIBase
{
    public override UIState GetUIState() => UIState.Clan;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {

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
