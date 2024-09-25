using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowCtrl : MonoBehaviour
{
    [SerializeField] Animator animCtrl;

    private Vector3 startPosition = new(0, 0.7f, 0);

    public void FireArrow()
    {
        gameObject.SetActive(true);

        Vector2 normalizedDir = NormalizeDirection(App.Manager.Player.MyCtrl.currDir);

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
        if (collision.gameObject.TryGetComponent<CharacterCtrl>(out var characterCtrl))
        {
            transform.DOKill();

            gameObject.SetActive(false);
            transform.localPosition = startPosition;
        }
    }
}
