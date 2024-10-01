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

        _charCtrl.MoveToPosition(manager.Respawn);
    }

    [Rpc]
    protected override void RPC_Attacked(float _damage, RpcInfo _info = default)
    {
        base.RPC_Attacked(_damage, _info);

 
    }

    protected override void Dead(CharacterCtrl _charCtrl)
    {
        base.Dead(_charCtrl);

        Respawn();
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);

        Setup();

        var manager = App.Manager.Game as DualManager;

        ownerCtrl.MoveToPosition(manager.Respawn);
        ownerCtrl.SetCharacterDead(false);
    }
}
