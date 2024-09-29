using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualManager : GameManager
{
    public Vector3 Respawn => respawnPos.position;

    protected override void Awake()
    {
        base.Awake();

        MaxRoundCount = 4;
        GameTime = 60;
    }

    protected override bool CheckVictoryCondition()
    {
        return false;
    }
}

