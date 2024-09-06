using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RankType
{
    Exp,
    OniKill,
    HiroshiKill,
    SurvivalCount,
}

public class RankDetailBack : MonoBehaviour
{
    [SerializeField] RankType type;

    private RankPlayerBack[] playerBacks;

    private void Awake()
    {
        playerBacks = GetComponentsInChildren<RankPlayerBack>();
    }

    public void Init()
    {

    }

    private string GetStatisticName => type switch
    {
        RankType.Exp => "ExperiencePoints",
        RankType.OniKill => "OniKills",
        RankType.HiroshiKill => "HiroshiKills",
        RankType.SurvivalCount => "SurvivalCount",
        _ => "ExperiencePoints"
    };
}
