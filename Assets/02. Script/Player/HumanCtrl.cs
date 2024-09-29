using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class HumanCtrl : NetworkBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject canvas;

    public Animator Anim { get; private set; }

    protected CharacterCtrl ownerCtrl;

    private Color hideColor = new(1, 1, 1, 0);

    private bool canHide = false;
    private bool isHide = false;

    protected virtual void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    public override void Spawned()
    {
        ownerCtrl = transform.parent.GetComponent<CharacterCtrl>();
    }

    public void Setup()
    {
        gameObject.SetActive(true);

        if (isHide)
        {
            RPC_Hide();
        }
    }

    public void UnSetup()
    {
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        if (!App.Manager.Game.IsGamePlay)
        {
            return;
        }

        if (!canHide)
        {
            return;
        }

        RPC_Hide();
    }

    [Rpc]
    private void RPC_Hide()
    {
        isHide = !isHide;

        ownerCtrl.SetAbleToMove(!isHide);
        sprite.color = isHide ? hideColor : Color.white;
        canvas.SetActive(!isHide);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrel"))
        {
            canHide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrel"))
        {
            canHide = false;
        }
    }
}
