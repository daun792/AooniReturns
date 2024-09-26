using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ModeType
{
    Infection,
    Bomb,
    Police,
    Dual
}

public class CreateRoomPanel : UIBase
{
    [SerializeField] Button createRoomBtn;
    [SerializeField] Button backBtn;

    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] Button[] modeBtns;

    private ModeType selectedMode;

    public override UIState GetUIState() => UIState.CreateRoom;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {
        createRoomBtn.onClick.AddListener(OnClickCreateRoom);
        backBtn.onClick.AddListener(ClosePanel);

        for (int i = 0; i < modeBtns.Length; i++)
        {
            int index = i;

            modeBtns[index].onClick.AddListener(() => OnClickMode(index));
        }
    }

    public override void OpenPanel()
    {
        base.OpenPanel();

        OnClickMode(0);
    }

    public override void ClosePanel()
    {
        base.ClosePanel();

        App.UI.Lobby.GetPanel<JoinRoomPanel>().OpenPanel();
    }

    private void OnClickMode(int _index)
    {
        selectedMode = (ModeType)_index;

        for (int i = 0; i < modeBtns.Length; i++)
        {
            if (_index == i)
            {
                modeBtns[i].image.color = Color.red;
            }
            else
            {
                modeBtns[i].image.color = Color.white;
            }
        }
    }

    private void OnClickCreateRoom()
    {
        App.Manager.Network.CreateMatch(roomNameInput.text, passwordInput.text, selectedMode);
    }
}