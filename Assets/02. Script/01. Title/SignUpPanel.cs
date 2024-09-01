using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanel : UIBase
{
    [SerializeField] Button cancelBtn;

    public override UIState GetUIState() => UIState.SignUp;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {
        cancelBtn.onClick.AddListener(OnClickCancel);
    }

    private void OnClickCancel()
    {
        ClosePanel();
    }
}
