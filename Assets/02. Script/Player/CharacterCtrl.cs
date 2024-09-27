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

[Serializable]
public struct CharacterState
{
    public CharacterType type;
    public GameObject obj;
    public Animator animator;
}

public class CharacterCtrl : NetworkBehaviour
{
    [Header("Player Settings")]
    [SerializeField] float DefaultSpeed = 4.5f;

    [Header("Joystick Settings")]
    public JoystickPanel joystick;
    public float joystickSensitivity = 1f;

    [Header("CharacterState")]
    [SerializeField] CharacterState[] characterStates;

    // networked values
    [Networked] float speed { get; set; }
    [Networked] bool IsCaught { get; set; }
    [Networked] public bool Targetable { get; private set; } = true;

    [Networked, OnChangedRender(nameof(SaveCurrentInfo))]
    public bool IsBusted { get; private set; } = false;

    [Networked, OnChangedRender(nameof(OnEscape))]
    public bool Escaped { get; set; } = false;

    [Networked, OnChangedRender(nameof(SaveCurrentInfo))]
    public bool Dead { get; set; } = false;

    private Rigidbody2D rb2d;
    public CharacterType CurrState { get; private set; }
    private Animator currAnimator;

    private TweenerCore<float, float, FloatOptions> speedTween;
    private float speedTarget;
    public ArrowCtrl arrow;

    public Vector2 currDir;

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

        App.Manager.Player.SubmitPlayer(this);
        joystick = App.Manager.UI.GetPanel<JoystickPanel>();

        SetCharacterState(0);
    }

    public void SetCharacterState(int _index)
    {
        for (int i = 0; i < characterStates.Length; i++)
        {
            int index = i;

            if (index == _index)
            {
                currAnimator = characterStates[index].animator;
                CurrState = characterStates[index].type;
                characterStates[index].obj.SetActive(true);
            }
            else
            {
                characterStates[index].obj.SetActive(false);
            }
        }
    }
    
    public void SetCharacterDead()
    {
        Dead = true;
    }

    #region Calculate Position
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority || IsCaught || Escaped) return;

        CalculatePosition();
    }

    private void CalculatePosition()
    {
        var xDir = joystick.Horizontal * joystickSensitivity;
        var yDir = joystick.Vertical * joystickSensitivity;

        var dir = new Vector2(xDir, yDir);

        if (dir == Vector2.zero)
        {
            currAnimator.SetBool("isWalk", false);

            Speed = 0f;
            rb2d.velocity = Vector2.zero;
            return;
        }

        currAnimator.SetBool("isWalk", true);
        currAnimator.SetFloat("MoveX", xDir);
        currAnimator.SetFloat("MoveY", yDir);

        Speed = DefaultSpeed;
        currDir = dir.normalized;

        rb2d.velocity = Speed * currDir;
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
}