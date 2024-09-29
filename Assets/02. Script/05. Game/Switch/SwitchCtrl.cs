using UnityEngine;
using Fusion;

public class SwitchCtrl : NetworkBehaviour
{
    [SerializeField] SwitchUICtrl uiCtrl;

    public float CurrHP { get; private set; } = 50f;
    public bool IsDestroyed { get; private set; } = false;

    public void Setup()
    {
        IsDestroyed = false;
        gameObject.SetActive(true);

        CurrHP = 50f;
        uiCtrl.SetHP(CurrHP);
    }

    public void Attacked(float _damage)
    {
        if (IsDestroyed)
        {
            return;
        }

        RPC_Attacked(_damage);
    }

    [Rpc]
    private void RPC_Attacked(float _damage)
    {
        CurrHP -= _damage;
        uiCtrl.SetHP(CurrHP);

        if (CurrHP <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        IsDestroyed = true;
        gameObject.SetActive(false);
    }
}
