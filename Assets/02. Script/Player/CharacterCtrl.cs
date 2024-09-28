using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Fusion;
using System;
using System.Linq;
using UnityEngine;

public enum CharacterType
{
    Human,
    Oni
}

public class CharacterCtrl : NetworkBehaviour
{
    [Header("Joystick Settings")]
    private JoystickPanel joystick;
    public float joystickSensitivity = 1f;


    CircleCollider2D characterCollider;
    // networked values
    [Networked] float speed { get; set; }

    [Networked, OnChangedRender(nameof(SaveCurrentInfo))]
    public bool IsBusted { get; private set; } = false;

    [Networked, OnChangedRender(nameof(OnEscape))]
    public bool Escaped { get; set; } = false;

    [Networked, OnChangedRender(nameof(SaveCurrentInfo))]
    public bool Dead { get; set; } = false;

    private Rigidbody2D rb2d;
    OniCtrl oniCtrl;
    HumanCtrl humanCtrl;

    public CharacterType CurrState { get; private set; }
    private Animator currAnimator;

    private TweenerCore<float, float, FloatOptions> speedTween;
    private float speedTarget;
    public ArrowCtrl arrow;

    [Networked, OnChangedRender(nameof(OnChangeDir))] 
    public Vector2 CurrDir { get; private set; } = new Vector2(0, -1);

    [Networked, OnChangedRender(nameof(OnChangeWalk))] 
    public bool IsWalk { get; private set; } = false;

    public float Stamina { get; private set; } = 100;

    public float Speed
    {
        get => speed;
        set
        {
            if (value == 0f)
            {
                speedTween?.Kill();
                speed = 0f;
                speedTarget = 0f;
                return;
            }

            if (speedTarget == value)
            {
                return;
            }

            speedTween?.Kill();
            speedTarget = value;
            speedTween = DOTween.To(() => speed, v =>
            {
                speed = v;
            },
            value, 0.5f).SetEase(Ease.OutCubic);
        }
    }

    public override void Spawned()
    {
        if (!HasStateAuthority)
        {
            Object.RequestStateAuthority();
        }

        rb2d = GetComponent<Rigidbody2D>();
        characterCollider = GetComponent<CircleCollider2D>();

        oniCtrl = GetComponentInChildren<OniCtrl>();
        humanCtrl = GetComponentInChildren<HumanCtrl>();

        App.Manager.Player.SubmitPlayer(this);
        joystick = App.Manager.UI.GetPanel<JoystickPanel>();

        SetCharacterState(0);
    }

    public void SetCharacterState(int _index)
    {
        characterCollider.enabled = true;

        switch ((CharacterType)_index)
        {
            case CharacterType.Human:
                humanCtrl.gameObject.SetActive(true);

                currAnimator = humanCtrl.GetComponent<Animator>();
                currAnimator.SetFloat("MoveX", CurrDir.x);
                currAnimator.SetFloat("MoveY", CurrDir.y);
                currAnimator.SetBool("isWalk", IsWalk);

                CurrState = humanCtrl.Type;

                oniCtrl.gameObject.SetActive(false);
                break;

            case CharacterType.Oni:
                oniCtrl.gameObject.SetActive(true);

                oniCtrl.Setup();

                currAnimator = oniCtrl.GetComponent<Animator>();
                currAnimator.SetFloat("MoveX", CurrDir.x);
                currAnimator.SetFloat("MoveY", CurrDir.y);
                currAnimator.SetBool("isWalk", IsWalk);

                CurrState = oniCtrl.Type;

                humanCtrl.gameObject.SetActive(false);
                break;
        }
    }
    
    public void SetCharacterDead()
    {
        Dead = true;

        oniCtrl.gameObject.SetActive(false);
        humanCtrl.gameObject.SetActive(false);

        characterCollider.enabled = false;
    }

    #region Calculate Position
    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            CalculatePosition();
        }
    }

    private void CalculatePosition()
    {
        var xDir = joystick.Horizontal * joystickSensitivity;
        var yDir = joystick.Vertical * joystickSensitivity;

        var dir = new Vector2(xDir, yDir);
        IsWalk = dir != Vector2.zero;

        if (dir == Vector2.zero)
        {
            Speed = 0f;
            rb2d.velocity = Vector2.zero;
            return;
        }

        CurrDir = dir.normalized;

        rb2d.velocity = 6 * CurrDir;
    }
    #endregion

    private void SaveCurrentInfo()
    {
        // 네트워크 동기화 로직
    }

    private void OnEscape()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        SaveCurrentInfo();
    }

    private void OnChangeDir()
    {
        currAnimator.SetFloat("MoveX", CurrDir.x);
        currAnimator.SetFloat("MoveY", CurrDir.y);
    }

    private void OnChangeWalk()
    {
        currAnimator.SetBool("isWalk", IsWalk);
    }
}