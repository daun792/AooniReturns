using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UIBase
{
    [SerializeField] Button loginBtn;
    [SerializeField] Button signUpBtn;

    public override void Init()
    {
        loginBtn.onClick.AddListener(OnClickLogin);
        signUpBtn.onClick.AddListener(OnClickSignUp);
    }

    private void OnClickLogin()
    {
        App.LoadScene(SceneName.Notice);
    }

    private void OnClickSignUp()
    {
        App.UI.Title.GetPanel<SignUpPanel>().OpenPanel();
    }
}
