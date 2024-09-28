using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class OniCtrl : NetworkBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] CharacterUICtrl uiCtrl;

    [Networked, OnChangedRender(nameof(OnChangeCurrHP))]
    public float CurrHP { get; private set; } = 100f;

    [Networked]
    public bool IsInvincible { get; private set; } = false;

    private void OnEnable()
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

    private void OnChangeCurrHP()
    {
        //uiCtrl.SetHP(CurrHP);
    }

    [Rpc]
    private void RPC_SetHumanToOni(CharacterCtrl _charCtrl)
    {
        _charCtrl.SetCharacterState(1);
    }

    public void Attacked(float _damage)
    {
        CurrHP -= _damage;

        RPC_Attacked();
    }

    [Rpc]
    private void RPC_Attacked()
    {
        StartCoroutine(AttackedAnimation());
        uiCtrl.SetHP(CurrHP);
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
