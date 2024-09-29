using UnityEngine;
using Fusion;

public class PrisonCtrl : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<CharacterCtrl>(out var charCtrl))
            {
                charCtrl.SetCharacterBusted(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<CharacterCtrl>(out var charCtrl))
            {
                charCtrl.SetCharacterBusted(false);
            }
        }
    }
}
