using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPlayerInfoBack : MonoBehaviour
{
    [SerializeField] Image medalImg;
    [SerializeField] TextMeshProUGUI nickTMP;
    [SerializeField] TextMeshProUGUI scoreTMP;
    [SerializeField] TextMeshProUGUI expTMP;
    [SerializeField] TextMeshProUGUI goldTMP;

    private const string nickNameString = "Lv.{0} {1}";
    private const string scoreString = "{0}/{1}/{2}";
    private const string addString = "+{0}";

    public void Setup(CharacterCtrl _charCtrl)
    {
        gameObject.SetActive(true);

        Sprite[] sprites = Resources.LoadAll<Sprite>("Medal");
        medalImg.sprite = sprites[_charCtrl.Level];

        nickTMP.text = string.Format(nickNameString, _charCtrl.Level, _charCtrl.NickName);
        scoreTMP.text = string.Format(scoreString, _charCtrl.OniKill, _charCtrl.HumanKill, _charCtrl.Survive);

        var total = _charCtrl.OniKill + _charCtrl.HumanKill + _charCtrl.Survive;
        expTMP.text = string.Format(addString, total);
        goldTMP.text = string.Format(addString, total);
    }

    public void SetNone()
    {
        gameObject.SetActive(false);
    }
}
