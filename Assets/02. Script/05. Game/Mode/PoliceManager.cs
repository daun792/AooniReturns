using UnityEngine;
using Fusion;

public class PoliceManager : GameManager
{
    [SerializeField] Transform prisonPosition;

    [Header("Start Position")]
    [SerializeField] Transform humanStartPosition;
    [SerializeField] Transform oniStartPosition;

    [Header("Switch")]
    [SerializeField] SwitchCtrl[] switchs;

    public Vector3 Prision => prisonPosition.position;
    public int RemainSwitchCount => GetRemainSwitchCount();

    protected override void Awake()
    {
        base.Awake();

        MaxRoundCount = 6;
        GameTime = 180;
    }

    private int GetRemainSwitchCount()
    {
        int count = 0;

        foreach (var item in switchs)
        {
            if (!item.IsDestroyed)
            {
                count++;
            }
        }

        return count;
    }

    protected override void SetRandomOni()
    {
        ResetSwitchs();

        if (!Runner.IsSceneAuthority)
        {
            return;
        }

        var num = App.Manager.Player.AllPlayers.Count / 2;
        App.Manager.Player.SetRandomOni(num);

        RPC_TeleportPlayers();
    }

    private void ResetSwitchs()
    {
        foreach (var item in switchs) 
        {
            item.Setup();
        }
    }

    [Rpc]
    private void RPC_TeleportPlayers()
    {
        foreach (var player in App.Manager.Player.AllPlayers)
        {
            if (player.CurrState == CharacterType.Human)
            {
                player.MoveToPosition(humanStartPosition.position);
            }
            else
            {
                player.MoveToPosition(oniStartPosition.position);
            }
        }
    }

    protected override bool CheckVictoryCondition()
    {
        if (CheckOniAllDead())
        {
            App.Manager.UI.Chat.SendNotice($"<color=#00FF00>인간의 승리!</color>", true);
            return true;
        }

        if (CheckSwitchAllDestroyed())
        {
            App.Manager.UI.Chat.SendNotice($"<color=#00FF00>인간의 승리!</color>", true);
            return true;
        }

        if (CheckHumanAllBusted())
        {
            App.Manager.UI.Chat.SendNotice($"<color=#00FF00>아오오니의 승리!</color>", true);
            return true;
        }

        if (App.Manager.UI.GetPanel<TimePanel>().Remaining <= 0f)
        {
            App.Manager.UI.Chat.SendNotice($"<color=#00FF00>아오오니의 승리!</color>", true);
            return true;
        }

        return false;
    }

    private bool CheckOniAllDead()
    {
        if (App.Manager.Player.OniPlayers.Count == 0)
        {
            return false;
        }

        foreach (var charCtrl in App.Manager.Player.OniPlayers)
        {
            if (charCtrl.IsDead == true)
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckSwitchAllDestroyed()
    {
        foreach (var item in switchs)
        {
            if (item.IsDestroyed == true)
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckHumanAllBusted()
    {
        if (App.Manager.Player.HumanPlayers.Count == 0)
        {
            return false;
        }

        foreach (var charCtrl in App.Manager.Player.HumanPlayers)
        {
            if (charCtrl.IsBusted == true)
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}
