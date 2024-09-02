using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionPanel : UIBase
{
    [SerializeField] Button bgmBtn;
    [SerializeField] Button sfxBtn;
    [SerializeField] Button controlPositionBtn;
    [SerializeField] Button crossKeysBtn;

    [SerializeField] Button continueBtn;

    [SerializeField] TextMeshProUGUI bgmTMP;
    [SerializeField] TextMeshProUGUI sfxTMP;
    [SerializeField] TextMeshProUGUI controlPositionTMP;
    [SerializeField] TextMeshProUGUI crossKeysTMP;

    private bool isMuteBGM;
    private bool isMuteSFX;

    public override UIState GetUIState() => UIState.Option;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {
        isMuteBGM = App.Manager.Sound.Volume.BGM < 0.001f;
        isMuteSFX = App.Manager.Sound.Volume.SFX < 0.001f;

        SetSoundText(bgmTMP, isMuteBGM);
        SetSoundText(sfxTMP, isMuteSFX);

        bgmBtn.onClick.AddListener(OnClickBGM);
        sfxBtn.onClick.AddListener(OnClickSFX);

        continueBtn.onClick.AddListener(ClosePanel);
    }

    public override void OpenPanel()
    {
        if (App.UI.Lobby.CurrState == UIState.CreateRoom)
        {
            App.UI.Lobby.GetPanel<CreateRoomPanel>().ClosePanel();
        }

        base.OpenPanel();
    }

    private void OnClickBGM()
    {
        isMuteBGM = !isMuteBGM;
        App.Manager.Sound.MuteVolume(AudioType.BGM, isMuteBGM);
        SetSoundText(bgmTMP, isMuteBGM);
    }

    private void OnClickSFX()
    {
        isMuteSFX = !isMuteSFX;
        App.Manager.Sound.MuteVolume(AudioType.SFX, isMuteSFX);
        SetSoundText(sfxTMP, isMuteSFX);
    }

    private void SetSoundText(TextMeshProUGUI _tmp, bool _isMute)
    {
        _tmp.text = _isMute ? "OFF" : "ON";
    }
}