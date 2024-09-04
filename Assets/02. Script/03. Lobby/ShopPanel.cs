using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPanel : UIBase
{
    [SerializeField] Button oniBtn;
    [SerializeField] Button humanBtn;
    [SerializeField] Button backBtn;

    [SerializeField] GameObject oniScrollBack;
    [SerializeField] GameObject humanScrollBack;

    [SerializeField] TextMeshProUGUI itemNameTMP;

    private ShopItemBtn[] oniItemBtns;
    private ShopItemBtn[] humanItemBtns;

    public override UIState GetUIState() => UIState.Shop;

    public override bool IsAddUIStack() => true;

    public override void Init()
    {
        oniBtn.onClick.AddListener(OnClickOni);
        humanBtn.onClick.AddListener(OnClickHuman);
        backBtn.onClick.AddListener(ClosePanel);

        oniItemBtns = oniScrollBack.GetComponentsInChildren<ShopItemBtn>();
        humanItemBtns = humanScrollBack.GetComponentsInChildren<ShopItemBtn>();
    }

    public override void OpenPanel()
    {
        if (App.UI.Lobby.CurrState == UIState.CreateRoom)
        {
            App.UI.Lobby.GetPanel<CreateRoomPanel>().ClosePanel();
        }

        base.OpenPanel();

        OnClickOni();
    }

    private void OnClickOni()
    {
        oniScrollBack.SetActive(true);
        humanScrollBack.SetActive(false);

        ResetItemBtns();

        itemNameTMP.text = string.Empty;
    }

    private void OnClickHuman()
    {
        oniScrollBack.SetActive(false);
        humanScrollBack.SetActive(true);

        ResetItemBtns();

        itemNameTMP.text = string.Empty;
    }

    public void OnClickItem(ShopItemBtn _targetItem)
    {
        ResetItemBtns();

        _targetItem.SetSelected();
        //itemNameTMP.text = _targetItem.Data.Name;
    }

    public void ResetItemBtns()
    {
        if (oniScrollBack.activeSelf)
        {
            foreach (var btn in oniItemBtns)
            {
                btn.ResetSelected();
            }
        }
        else
        {
            foreach (var btn in humanItemBtns)
            {
                btn.ResetSelected();
            }
        }
    }
}
