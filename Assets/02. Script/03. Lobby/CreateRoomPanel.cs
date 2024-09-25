using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : UIBase
{
    [SerializeField] Button createRoomBtn;

    public override UIState GetUIState() => UIState.CreateRoom;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {
        createRoomBtn.onClick.AddListener(OnCreateRoom);
    }

    public override void ClosePanel()
    {
        base.ClosePanel();

        App.UI.Lobby.GetPanel<JoinRoomPanel>().OpenPanel();
    }


    private void OnCreateRoom()
    {
        App.Manager.Network.CreateMatch();
    }
}