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
    [Networked] public string NickName { get; private set; }
    [Networked] public int Level { get; private set; }
    [Networked] public bool IsHost { get; private set; }

    [Networked, OnChangedRender(nameof(OnChangeState))] public CharacterType CurrState { get; private set; }
    [Networked, OnChangedRender(nameof(OnChangeDir))] public Vector2 CurrDir { get; private set; }
    [Networked, OnChangedRender(nameof(OnChangeWalk))] public bool IsWalk { get; private set; }
    [Networked, OnChangedRender(nameof(OnChangeDead))] public bool IsDead { get; set; }
    [Networked] public bool IsBusted { get; private set; } = false;

    [Networked] public int OniKill { get; private set; } = 0;
    [Networked] public int HumanKill { get; private set; } = 0;
    [Networked] public int Survive { get; private set; } = 0;

    public OniCtrl Oni { get; private set; }
    public HumanCtrl Human { get; private set; }
    public ArrowCtrl Arrow { get; private set; }

    private Rigidbody2D rigid;
    private CircleCollider2D charCollider;
    private JoystickPanel joystick;
    private Animator currAnimator;

    private bool canMove = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        charCollider = GetComponent<CircleCollider2D>();

        Oni = GetComponentInChildren<OniCtrl>(true);
        Human = GetComponentInChildren<HumanCtrl>(true);
        Arrow = GetComponentInChildren<ArrowCtrl>(true);

        currAnimator = Human.GetComponent<Animator>();
    }

    public override void Spawned()
    {
        App.Manager.Player.SubmitPlayer(this);

        if (!HasStateAuthority)
        {
            return;
        }

        Object.RequestStateAuthority();

        NickName = App.Data.Player.NickName;
        Level = CalculateLevel(App.Data.Player.ExperiencePoints);
        IsHost = Runner.IsSceneAuthority;

        CurrState = CharacterType.Human;
        CurrDir = new Vector2(0, -1);
        IsWalk = false;
        IsDead = false;
        IsBusted = false;

        OniKill = 0;
        HumanKill = 0;
        Survive = 0;

        joystick = App.Manager.UI.GetPanel<JoystickPanel>();
    }

    private int CalculateLevel(int _totalExp)
    {
        int currLevel = 1;
        int requiredExp = 50;

        while (_totalExp >= requiredExp)
        {
            _totalExp -= requiredExp;
            currLevel++;
            requiredExp += 100;
        }

        return currLevel;
    }

    #region Move
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        if (!canMove)
        {
            return;
        }

        CalculatePosition();
    }

    private void CalculatePosition()
    {
        var xDir = joystick.Horizontal;
        var yDir = joystick.Vertical;

        var dir = new Vector2(xDir, yDir);
        IsWalk = dir != Vector2.zero;

        if (dir == Vector2.zero)
        {
            rigid.velocity = Vector2.zero;
            return;
        }

        CurrDir = dir.normalized;

        rigid.velocity = 4.5f * CurrDir;
    }
    #endregion

    #region Networked Variables
    public void SetCharacterState(int _index)
    {
        if (!HasStateAuthority)
        {
            return;
        }

        SetCharacterDead(false);
        CurrState = (CharacterType)_index;
    }

    private void OnChangeState()
    {
        switch (CurrState)
        {
            case CharacterType.Human:
                Human.Setup();

                currAnimator = Human.Anim;
                SetupAnimator();

                Oni.UnSetup();
                break;

            case CharacterType.Oni:
                Oni.Setup();

                currAnimator = Oni.Anim;
                SetupAnimator();

                Human.UnSetup();
                break;
        }
    }

    private void SetupAnimator()
    {
        currAnimator.SetFloat("MoveX", CurrDir.x);
        currAnimator.SetFloat("MoveY", CurrDir.y);
        currAnimator.SetBool("isWalk", IsWalk);
    }

    public void SetCharacterDead(bool _isDead)
    {
        if (!HasStateAuthority)
        {
            return;
        }

        IsDead = _isDead;
    }

    private void OnChangeDead()
    {
        charCollider.enabled = !IsDead;

        if (IsDead)
        {
            Oni.gameObject.SetActive(false);
            Human.gameObject.SetActive(false);
        }
    }

    public void SetCharacterBusted(bool _isBusted)
    {
        if (!HasStateAuthority)
        {
            return;
        }

        IsBusted = _isBusted;
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
    #endregion

    #region Score
    public void AddOniKillScore()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        OniKill++;
        App.Manager.UI.GetPanel<ScorePanel>().UpdateScore();
    }

    public void AddHumanKillScore()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        HumanKill++;
        App.Manager.UI.GetPanel<ScorePanel>().UpdateScore();
    }

    public void AddSurviveScore()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        Survive++;
        App.Manager.UI.GetPanel<ScorePanel>().UpdateScore();
    }
    #endregion

    public void MoveToPosition(Vector2 _randomPosition)
    {
        if (!HasStateAuthority)
        {
            return;
        }

        transform.position = _randomPosition;
    }

    public void SetAbleToMove(bool _isAble)
    {
        if (!HasStateAuthority)
        {
            return;
        }

        canMove = _isAble;
    }
}