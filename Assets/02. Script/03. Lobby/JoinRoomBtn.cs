using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

public class JoinRoomBtn : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roomNameTMP;
    [SerializeField] TextMeshProUGUI modeTMP;
    [SerializeField] TextMeshProUGUI playerCountTMP;
    
    public void SetInfo(SessionInfo _info)
    {
        gameObject.SetActive(true);

        roomNameTMP.text = _info.Name;
        modeTMP.text = GetModeName(_info.Properties["GameMode"]);
        playerCountTMP.text = string.Format("{0}/{1}", _info.PlayerCount, _info.MaxPlayers);
    }

    private string GetModeName(int _modeIndex) => (ModeType)_modeIndex switch
    {
        ModeType.Infection => "°¨¿° ¸ðµå",
        ModeType.Bomb => "ÆøÅº ¸ðµå",
        ModeType.Police => "µµµÏ°ú °æÂû ¸ðµå",
        ModeType.Dual => "µà¾ó ¸ðµå",
        _ => "°¨¿° ¸ðµå"
    };

    public void SetNone()
    {
        gameObject.SetActive(false);
    }
}
