using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : UIManager
{
    [SerializeField] Button shopBtn;
    [SerializeField] Button createRoomBtn;
    [SerializeField] Button cafeBtn;
    [SerializeField] Button rankBtn;
    [SerializeField] Button clanBtn;

    protected override void Start()
    {
        base.Start();

        shopBtn.onClick.AddListener(OnClickShop);
        createRoomBtn.onClick.AddListener(OnClickCreateRoom);
        cafeBtn.onClick.AddListener(OnClickCafe);
        rankBtn.onClick.AddListener(OnClickRank);
        clanBtn.onClick.AddListener(OnClickClan);
    }

    private void OnClickShop()
    {
        GetPanel<ShopPanel>().OpenPanel();
    }

    public void OnClickCreateRoom()
    {
        GetPanel<CreateRoomPanel>().OpenPanel();
    }

    private void OnClickCafe()
    {
       
    }

    private void OnClickRank()
    {
        GetPanel<RankPanel>().OpenPanel();
    }

    private void OnClickClan()
    {
        GetPanel<ClanPanel>().OpenPanel();
    }

}
