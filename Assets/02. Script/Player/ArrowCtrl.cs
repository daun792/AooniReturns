using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using DG.Tweening;

public class ArrowCtrl : NetworkBehaviour
{
    [SerializeField] Animator animCtrl;

    private Vector3 startPosition = new(0, 0.7f, 0);

    private CharacterCtrl myCharCtrl;

    private void Start()
    {
        gameObject.SetActive(false);
        myCharCtrl = GetComponentInParent<CharacterCtrl>();
    }

    public void FireArrow()
    {
        RPC_FireArrow(myCharCtrl.CurrDir);
    }

    [Rpc]
    private void RPC_FireArrow(Vector2 _dir)
    {
        gameObject.SetActive(true);

        Vector2 normalizedDir = NormalizeDirection(_dir);

        Vector3 targetPosition = transform.position + new Vector3(normalizedDir.x, normalizedDir.y, 0) * 2;

        animCtrl.SetFloat("MoveX", normalizedDir.x);
        animCtrl.SetFloat("MoveY", normalizedDir.y);

        transform.DOMove(targetPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameObject.SetActive(false);
            transform.localPosition = startPosition;
        });
    }

    private Vector2 NormalizeDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            return new Vector2(Mathf.Sign(dir.x), 0);
        }
        else
        {
            return new Vector2(0, Mathf.Sign(dir.y));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!App.Manager.Game.IsGamePlay)
        {
            return;
        }

        if (collision.CompareTag("Oni"))
        {
            if (collision.TryGetComponent<OniCtrl>(out var oniCtrl))
            {
                if (!oniCtrl.IsInvincible)
                {
                    transform.DOKill();

                    gameObject.SetActive(false);
                    transform.localPosition = startPosition;

                    oniCtrl.Attacked(10);
                }
            }
        }
    }
}
