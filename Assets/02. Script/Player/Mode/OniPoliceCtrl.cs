using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class OniPoliceCtrl : OniCtrl
{
    protected override void InteractHuman(CharacterCtrl _charCtrl)
    {
        RPC_ArrestHuman(_charCtrl);
    }

    [Rpc]
    private void RPC_ArrestHuman(CharacterCtrl _charCtrl)
    {
        var manager = App.Manager.Game as PoliceManager;

        _charCtrl.MoveToPosition(manager.Prision);
    }
}
