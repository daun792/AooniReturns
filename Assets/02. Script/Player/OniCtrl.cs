using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class OniCtrl : NetworkBehaviour
{
    [SerializeField] SpriteRenderer sprite;

    [Networked]
    public bool IsInvincible { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Human"))
        {
            if (collision.transform.parent.TryGetComponent<CharacterCtrl>(out var charCtrl))
            {
                RPC_SetHumanToOni(charCtrl);
            }
        }
    }

    [Rpc]
    private void RPC_SetHumanToOni(CharacterCtrl _charCtrl)
    {
        _charCtrl.SetCharacterState(1);
    }

    public void Attacked()
    {
        RPC_Attacked();
    }

    [Rpc]
    private void RPC_Attacked()
    {
        StartCoroutine(AttackedAnimation());
    }

    private IEnumerator AttackedAnimation()
    {
        IsInvincible = true;
        sprite.color = Color.red;

        yield return new WaitForSeconds(1);

        IsInvincible = false;
        sprite.color = Color.white;
    }
}
