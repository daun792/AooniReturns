using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ProfilesModels;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClanMemberBack : MonoBehaviour
{
    [SerializeField] Image medalImg;

    [SerializeField] TextMeshProUGUI rankTMP;
    [SerializeField] TextMeshProUGUI levelTMP;
    [SerializeField] TextMeshProUGUI nickTMP;

    [SerializeField] Button outBtn;

    private string entityID = string.Empty;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Init(string _ID)
    {
        bool isSuccess = true;

        if (entityID == _ID) return;

        GetPlayFabIdFromEntityId(_ID,
        (playerID) =>
        {
            GetMemberProfile(playerID,
            (result) =>
            {
                nickTMP.text = result;
            },
            (error) =>
            {
                isSuccess = false;
            });

            GetMemberUserData(playerID,
            (result) =>
            {
                if (result.ContainsKey("ExperiencePoints"))
                {
                    int.TryParse(result["ExperiencePoints"].Value, out var exp);
                    SetLevelTMP(exp);
                }
            },
            (error) =>
            {
                isSuccess = false;
            });

            GetMemberRank(playerID,
            (result) =>
            {
                if (result == 0)
                {
                    rankTMP.text = "순위권 외";
                }
                else
                {
                    rankTMP.text = string.Format("{0}위", result);
                }
            },
            (error) =>
            {
                isSuccess = false;
            });
        },
        (error) =>
        {
            isSuccess = false;
        });

        entityID = _ID;
        gameObject.SetActive(isSuccess);
    }

    private void GetPlayFabIdFromEntityId(string entityId, Action<string> onSuccess, Action<EPlayerDataError> onError)
    {
        var request = new GetEntityProfileRequest
        {
            Entity = new PlayFab.ProfilesModels.EntityKey
            {
                Id = entityId,
                Type = "title_player_account"
            }
        };

        PlayFabProfilesAPI.GetProfile(request, result =>
        {
            var playerId = result.Profile.Lineage.MasterPlayerAccountId;
            if (playerId != null)
            {
                onSuccess?.Invoke(playerId);
            }
            else
            {
                Debug.LogError("PlayerId not found for this entity.");
            }
        }, error =>
        {
            Debug.LogError("Error getting entity profile: " + error.ErrorMessage);
        });
    }

    private void GetMemberProfile(string playFabId, Action<string> onSuccess, Action<EPlayerDataError> onError)
    {
        var request = new GetPlayerProfileRequest
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true 
            }
        };

        PlayFabClientAPI.GetPlayerProfile(request,
        result =>
        {
        if (result.PlayerProfile != null && !string.IsNullOrEmpty(result.PlayerProfile.DisplayName))
            {
                onSuccess?.Invoke(result.PlayerProfile.DisplayName);
            }
            else
            {
                Debug.LogError($"Player profile found but no display name for PlayFabId: {playFabId}");
            }
        },
        error =>
        {
            Debug.LogError($"Failed to get player profile for PlayFabId: {playFabId}. Error: {error.GenerateErrorReport()}");
        });
    }

    private void GetMemberUserData(string playFabId, Action<Dictionary<string, UserDataRecord>> onSuccess, Action<EPlayerDataError> onError)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = playFabId
        };

        PlayFabClientAPI.GetUserData(request,
        result =>
        {
            onSuccess?.Invoke(result.Data);
        },
        error =>
        {
            Debug.LogError($"Failed to get user data for PlayFabId: {playFabId}. Error: {error.GenerateErrorReport()}");
            onError?.Invoke(EPlayerDataError.LoadUserDataFailed);
        });
    }

    private void SetLevelTMP(int _exp)
    {
        var result = CalculateLevel(_exp);

        levelTMP.text = string.Format("Lv.{0}", result.ToString());
    }

    private int CalculateLevel(int _totalExp)
    {
        int currLevel = 1;
        int requiredExp = 50;

        while (_totalExp >= requiredExp)
        {
            _totalExp -= requiredExp;
            currLevel++;
            requiredExp += 100;
        }

        return currLevel;
    }

    private void GetMemberRank(string playFabId, Action<int> onSuccess, Action<PlayFabError> onError)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "ExperiencePoints",
            PlayFabId = playFabId,    
            MaxResultsCount = 1      
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request,
        result =>
        {
            if (result.Leaderboard != null && result.Leaderboard.Count > 0)
            {
                var entry = result.Leaderboard[0];
                int rank = entry.Position; 
                onSuccess?.Invoke(rank);
            }
            else
            {
                Debug.LogError("Leaderboard entry not found.");
                onError?.Invoke(new PlayFabError { ErrorMessage = "Leaderboard entry not found." });
            }
        },
        error =>
        {
            Debug.LogError($"Failed to get leaderboard around player. Error: {error.GenerateErrorReport()}");
            onError?.Invoke(error);
        });
    }
}
