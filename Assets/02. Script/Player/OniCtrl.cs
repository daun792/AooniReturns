using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniCtrl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Human"))
        {
            if (collision.transform.parent.TryGetComponent<CharacterCtrl>(out var charCtrl))
            {
                charCtrl.SetCharacterState(1);
            }
        }
    }
}
