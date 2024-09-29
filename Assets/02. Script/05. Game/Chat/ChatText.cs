using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class ChatText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI content;

    private const string humanString = "[인간]";
    private const string oniString = "[오니]";

    private const string chatString = "{0}{1}: {2}";

    public void Setup(string _content, PlayerRef _sender)
    {
        if (_sender == PlayerRef.None)
        {
            content.text = _content;
        }
        else
        {
            var playerObj = App.Manager.Network.Runner.GetPlayerObject(_sender);
            var charCtrl = playerObj.GetComponent<CharacterCtrl>();

            var charState = charCtrl.CurrState == CharacterType.Human ? humanString : oniString;
            content.text = string.Format(chatString, charState, charCtrl.NickName, _content);
        }
    }
}