using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.U2D.Animation;

public abstract class OniCtrl : NetworkBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] CharacterUICtrl uiCtrl;

    [SerializeField] SpriteLibrary spriteLibrary;
    [SerializeField] SpriteLibraryAsset[] spriteAssets;

    public Animator Anim { get; private set; }
    public float CurrHP { get; private set; } = 100f;

    [Networked] public bool IsInvincible { get; private set; } = false;

    protected CharacterCtrl ownerCtrl;

    protected virtual void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    public override void Spawned()
    {
        ownerCtrl = transform.parent.GetComponent<CharacterCtrl>();

        spriteLibrary.spriteLibraryAsset = spriteAssets[ownerCtrl.OniSkinIndex];
    }

    public void Setup()
    {
        gameObject.SetActive(true);

        CurrHP = 100f;
        uiCtrl.SetHP(CurrHP);

        IsInvincible = false;
        sprite.color = Color.white;
    }

    public void UnSetup()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!App.Manager.Game.IsGamePlay)
        {
            return;
        } 

        if (collision.CompareTag("Human"))
        {
            if (!ownerCtrl.HasStateAuthority)
            {
                return;
            }

            if (collision.transform.parent.TryGetComponent<CharacterCtrl>(out var charCtrl))
            {
                ownerCtrl.AddHumanKillScore();
                ownerCtrl.AddScore(25);

                InteractHuman(charCtrl);
            }
        }
    }

    protected abstract void InteractHuman(CharacterCtrl _charCtrl);

    public void Attacked(float _damage)
    {
        RPC_Attacked(_damage);
    }

    [Rpc]
    protected virtual void RPC_Attacked(float _damage, RpcInfo _info = default)
    {
        CurrHP -= _damage;
        uiCtrl.SetHP(CurrHP);

        var playerObj = App.Manager.Network.Runner.GetPlayerObject(_info.Source);
        var charCtrl = playerObj.GetComponent<CharacterCtrl>();
        charCtrl.AddScore(5);

        if (CurrHP <= 0)
        {
            Dead(charCtrl);
        }
        else
        {
            StartCoroutine(AttackedAnimation());
        }
    }

    protected virtual void Dead(CharacterCtrl _charCtrl)
    {
        ownerCtrl.SetCharacterDead(true);

        _charCtrl.AddOniKillScore();
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
