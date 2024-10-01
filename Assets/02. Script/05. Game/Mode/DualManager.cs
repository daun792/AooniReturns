using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class DualManager : GameManager
{
    [Header("Switch")]
    [SerializeField] SwitchCtrl switchItem;

    public Vector3 Respawn => respawnPos.position;

    protected override void Awake()
    {
        base.Awake();

        MaxRoundCount = 4;
        GameTime = 60;
    }

    protected override bool CheckVictoryCondition()
    {
        if (App.Manager.UI.GetPanel<TimePanel>().Remaining <= 0)
        {
            CheckSwitchDestroyed();
            SetWinner();
            return true;
        }

        return false;
    }

    private void CheckSwitchDestroyed()
    {
        if (switchItem.IsDestroyed)
        {
            RPC_AddSwitchScore(App.Manager.Player.HumanPlayers[0].Object.StateAuthority);
        }
        else
        {
            RPC_AddSwitchScore(App.Manager.Player.OniPlayers[0].Object.StateAuthority);
        }
    }

    [Rpc]
    private void RPC_AddSwitchScore(PlayerRef _player)
    {
        var playerObj = App.Manager.Network.Runner.GetPlayerObject(_player);
        var charCtrl = playerObj.GetComponent<CharacterCtrl>();
        charCtrl.AddScore(100);
    }

    private void SetWinner()
    {
        if (App.Manager.Player.OniPlayers[0].Score >= App.Manager.Player.HumanPlayers[0].Score)
        {
            App.Manager.UI.Chat.SendNotice($"<color=#00FF00>아오오니의 승리!</color>", true);
        }
        else
        {
            App.Manager.UI.Chat.SendNotice($"<color=#00FF00>인간의 승리!</color>", true);
        }
    }
}

