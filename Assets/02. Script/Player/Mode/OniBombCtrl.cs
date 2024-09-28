using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class OniBombCtrl : OniCtrl
{
    protected override void InteractHuman(CharacterCtrl _charCtrl)
    {
        ownerCtrl.SetCharacterState(0);

        RPC_SetHumanToOni(_charCtrl);
    }

    [Rpc]
    private void RPC_SetHumanToOni(CharacterCtrl _charCtrl)
    {
        _charCtrl.SetCharacterState(1);
    }
}
