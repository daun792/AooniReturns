using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoBack : MonoBehaviour
{
    [SerializeField] Image medalImg;
    [SerializeField] TextMeshProUGUI nickTMP;

    private CharacterCtrl targetCtrl;
    private CharacterType currState = CharacterType.Human;

    private const string nickNameString = "Lv.{0} {1} {2}";
    private const string masterClient = "(πÊ¿Â)";

    private void Update()
    {
        if (targetCtrl == null) 
        {
            return;
        }

        if (currState != targetCtrl.CurrState)
        {
            currState = targetCtrl.CurrState;

            nickTMP.color = currState == CharacterType.Human ? Color.white : Color.red;
        }
    }

    public void SetPlayer(CharacterCtrl _charCtrl)
    {
        targetCtrl = _charCtrl;

        gameObject.SetActive(true);

        //medalImg.sprite = 
        var clientRole = _charCtrl.Object.StateAuthority.IsMasterClient ? masterClient : string.Empty;
        nickTMP.text = string.Format(nickNameString, _charCtrl.Level, _charCtrl.NickName, clientRole);
    }

    public void SetNone()
    {
        targetCtrl = null;

        gameObject.SetActive(false);
    }
}
