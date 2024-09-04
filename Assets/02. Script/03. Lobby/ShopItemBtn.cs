using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemBtn : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] TextMeshProUGUI priceTMP;
    [SerializeField] GameObject usingTMP;
    [SerializeField] GameObject selectedImg;
    [SerializeField] Button selectedBtn;

    public bool IsSelected { get; private set; }

    private void Awake()
    {
        selectedBtn.onClick.AddListener(OnClickSelected);
    }

    private void OnClickSelected()
    {
        App.UI.Lobby.GetPanel<ShopPanel>().OnClickItem(this);
    }

    public void SetSelected()
    {
        selectedImg.SetActive(true);
        IsSelected = true;
    }

    public void ResetSelected()
    {
        selectedImg.SetActive(false);
        IsSelected = false;
    }

    public void SetUsing()
    {

    }

    public void ResetUsing()
    {

    }
}
