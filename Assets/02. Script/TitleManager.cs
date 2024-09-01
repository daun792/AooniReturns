using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Button cafeBtn;
    [SerializeField] Button loginBtn;
    [SerializeField] Button signUpBtn;
    [SerializeField] Button cancelBtn;
    [SerializeField] GameObject signUpBack;

    private void Start()
    {
        App.Manager.Sound.PlayBGM("BGM_Title");

        cafeBtn.onClick.AddListener(OnClickCafe);
        loginBtn.onClick.AddListener(OnClickCafe);
        signUpBtn.onClick.AddListener(OnClickSignUp);
        cancelBtn.onClick.AddListener(OnClickCancel);
    }

    private void OnClickCafe()
    {
        Application.OpenURL("https://m.cafe.naver.com/onireturns");
    }

    private void OnClickLogin()
    {
        App.LoadScene(SceneName.Lobby);
    }

    private void OnClickSignUp()
    {
        signUpBack.SetActive(true);
    }

    private void OnClickCancel()
    {
        signUpBack.SetActive(false);
    }
}
