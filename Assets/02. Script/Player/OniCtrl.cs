using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class OniCtrl : NetworkBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] CharacterUICtrl uiCtrl;

    public float CurrHP { get; private set; } = 100f;

    [Networked]
    public bool IsInvincible { get; private set; } = false;

    public void Setup()
    {
        CurrHP = 100f;
        IsInvincible = false;
    }

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

    public void Attacked(float _damage)
    {
        RPC_Attacked(_damage);
    }

    [Rpc]
    private void RPC_Attacked(float _damage)
    {
        CurrHP -= _damage;
        uiCtrl.SetHP(CurrHP);
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
