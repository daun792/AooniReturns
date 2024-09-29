using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class OniDualCtrl : OniCtrl
{
    protected override void InteractHuman(CharacterCtrl _charCtrl)
    {
        RPC_TeleportHuman(_charCtrl);
    }

    [Rpc]
    private void RPC_TeleportHuman(CharacterCtrl _charCtrl)
    {
        var manager = App.Manager.Game as DualManager;

        _charCtrl.MoveToPosition(manager.Respawn.position);
    }

    protected override void Dead()
    {
        base.Dead();

        Respawn();
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);

        Setup();

        var manager = App.Manager.Game as DualManager;

        ownerCtrl.MoveToPosition(manager.Respawn.position);
        ownerCtrl.SetCharacterDead(false);
    }
}
