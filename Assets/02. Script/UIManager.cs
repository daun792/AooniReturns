using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum UIState
{
    Normal,
    Play,
    Start,
    Inventory,
    Option,
    End
}

public class UIManager : Manager
{
    [SerializeField] Image blackBlur;

    [HideInInspector]
    public UIState CurrState
        => UIStack.Count == 0 ? UIState.Normal : UIStack.Peek();

    private Dictionary<Type, UIBase> UIDic;
    private Stack<UIState> UIStack;

    protected override void Awake()
    {
        base.Awake();

        var UIs = GetComponentsInChildren<UIBase>();

        UIDic = new(UIs.Length);
        UIStack = new();

        foreach (var UI in UIs)
        {
            UIDic.Add(UI.GetPanelType(), UI);
        }
    }

    private void Start()
    {
        InitUIs();
    }

    private void InitUIs()
    {
        foreach (var UI in UIDic.Values)
        {
            if (!UI.gameObject.activeSelf) //wake up panels
            {
                UI.gameObject.SetActive(true);
                UI.gameObject.SetActive(false);
            }

            try { UI.Init(); }
            catch (Exception error)
            { Debug.LogError($"ERROR: {error.Message}\n{error.StackTrace}"); }
        }
    }

    #region Get Panel
    public T GetPanel<T>() where T : UIBase => (T)UIDic[typeof(T)];

    public bool TryGetPanel<T>(out T _panel) where T : UIBase
    {
        if (UIDic.TryGetValue(typeof(T), out var panel))
        {
            _panel = (T)panel;
            return true;
        }

        _panel = default;
        return false;
    }
    #endregion

    #region UI Stack Managing
    public void AddUIStack(UIState _state)
    {
        UIStack.Push(_state);
    }

    public void PopUIStack(UIState _state = 0)
    {
        if (CurrState != _state) return;

        UIStack.Pop();
    }
    #endregion

    #region Fade In / Out
    public void FadeIn(Action _endEvent = null)
    {
        App.Manager.Sound.StopBGM();

        if (blackBlur.color.a == 1f)
        {
            _endEvent?.Invoke();
            return;
        }

        blackBlur.gameObject.SetActive(true);

        blackBlur.DOKill();
        blackBlur.DOFade(1f, 0.5f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _endEvent?.Invoke();
            });
    }

    public void FadeOut(Action _endEvent = null)
    {
        if (blackBlur.color.a == 0f)
        {
            _endEvent?.Invoke();
            return;
        }

        blackBlur.DOFade(0f, 1f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _endEvent?.Invoke();
                blackBlur.gameObject.SetActive(false);
            });
    }

    public void FadeInOut(Action _midEvent = null)
    {
        App.Manager.Sound.StopBGM();

        if (blackBlur.color.a == 1f)
        {
            _midEvent?.Invoke();
            FadeOut();
            return;
        }

        blackBlur.gameObject.SetActive(true);

        blackBlur.DOKill();
        blackBlur.DOFade(1f, 0.5f).SetEase(Ease.Linear)
           .OnComplete(() =>
           {
               _midEvent?.Invoke();
               FadeOut();
           });
    }
    #endregion
}
