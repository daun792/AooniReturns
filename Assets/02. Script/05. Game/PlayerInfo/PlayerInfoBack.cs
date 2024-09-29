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
        //nickTMP.text = 
    }

    public void SetNone()
    {
        targetCtrl = null;

        gameObject.SetActive(false);
    }
}
