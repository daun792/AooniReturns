using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : GameManager
{
    protected override void Awake()
    {
        base.Awake();

        MaxRoundCount = 3;
        GameTime = 60;
    }

    protected override bool CheckVictoryCondition()
    {
        return false;
    }
}
