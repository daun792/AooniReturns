using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public enum EPlayerDataError
{
    LoadPlayerDataFailed,

    LoadProfileFailed,
    LoadUserDataFailed,
    UpdatePlayerStatisticsFailed,
    UpdateUserDataFailed,

    InvalidNickname,
    InvalidNicknameLength,
}

public class PlayerData : Data
{
    private string DefaultNick => $"아오오니{new Random().Next(1000, 10000)}";

    private Dictionary<string, UserDataRecord> roData;
    private Dictionary<string, UserDataRecord> userData;
    public IReadOnlyDictionary<string, UserDataRecord> UserData => userData;

    private string nickName;
    private string clan;
    private int currency;
    private int kills;

    public string NickName => nickName;
    public string Clan => clan;
    public int Currency => currency;
    public int Kills => kills;

    private enum ENickValidateResult : byte
    {
        Invalid,
        Valid,
        ShouldUpdate
    }

    public void GetPlayerData(Action _getPlayerDataCallback, Action<EPlayerDataError> _errorHandler)
    {
        PlayFabClientAPI.GetPlayerCombinedInfo(new GetPlayerCombinedInfoRequest()
        {
            InfoRequestParameters = new()
            {
                GetPlayerProfile = true,
                GetPlayerStatistics = true,
                GetUserReadOnlyData = true,
                GetUserData = true,
            }
        },
        (result) =>
        {
            var payload = result.InfoResultPayload;

            UpdatePlayerProfileInternal(payload.PlayerProfile, null, payload.UserReadOnlyData);
            UpdateUserDataInternal(payload.UserData);
            UpdatePlayerStatisticsInternal(payload.PlayerStatistics);

            _getPlayerDataCallback?.Invoke();
        },
        (error) =>
        {
            _errorHandler?.Invoke(EPlayerDataError.LoadPlayerDataFailed);
        });
    }

    #region Profile

    public void GetPlayerProfile(Action _getProfileCallback, Action<EPlayerDataError> _errorHandler)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result =>
        {
            UpdatePlayerProfileInternal(result.PlayerProfile, _errorHandler);
            _getProfileCallback?.Invoke();
        },
        error =>
        {
            _errorHandler?.Invoke(EPlayerDataError.LoadProfileFailed);
        });
    }

    private void UpdatePlayerProfileInternal(PlayerProfileModel _profileData, Action<EPlayerDataError> _errorHandler, Dictionary<string, UserDataRecord> _roData = null)
    {
        if (_roData != null)
        {
            roData = _roData;
        }

        UpdatePlayerNickInternal(_profileData.DisplayName, _errorHandler);
    }

    #endregion

    #region Nickname

    public void SetPlayerNickName(string _nickName, Action<string> _setPlayerNickNameCallback, Action<EPlayerDataError> _errorHandler)
    {
        switch (ValidateNickInput(ref _nickName, _errorHandler))
        {
            case ENickValidateResult.Invalid:
                return;

            case ENickValidateResult.ShouldUpdate:
            case ENickValidateResult.Valid:
                break;
        }

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = _nickName,
        },
        result =>
        {
            UpdatePlayerNickInternal(result.DisplayName, _errorHandler);
            _setPlayerNickNameCallback?.Invoke(nickName);
        },
        error =>
        {
            _errorHandler?.Invoke(EPlayerDataError.InvalidNickname);
        });
    }

    private void UpdatePlayerNickInternal(string _nickname, Action<EPlayerDataError> _errorHandler)
    {
        nickName = _nickname;

        switch (ValidateNickData(ref nickName))
        {
            case ENickValidateResult.Invalid:
                nickName = DefaultNick;
                goto case ENickValidateResult.ShouldUpdate;

            case ENickValidateResult.ShouldUpdate:
                SetPlayerNickName(nickName, null, _errorHandler);
                break;

            case ENickValidateResult.Valid:
            default: break;
        }
    }

    private ENickValidateResult ValidateNickInput(ref string _nickName, Action<EPlayerDataError> _errorHandler)
    {
        if (_nickName == null)
        {
            _errorHandler?.Invoke(EPlayerDataError.InvalidNicknameLength);
            return ENickValidateResult.Invalid;
        }

        if (_nickName.Length < 1 || _nickName.Length > 8)
        {
            _errorHandler?.Invoke(EPlayerDataError.InvalidNicknameLength);
            return ENickValidateResult.Invalid;
        }

        return ENickValidateResult.Valid;
    }

    private ENickValidateResult ValidateNickData(ref string _nickName)
    {
        if (_nickName == null || _nickName.Length < 1 || _nickName.Length > 10)
        {
            return ENickValidateResult.Invalid;
        }

        return ENickValidateResult.Valid;
    }

    #endregion

    #region Player Statistics (Currency, Kills)

    private void UpdatePlayerStatisticsInternal(List<StatisticValue> statistics)
    {
        foreach (var stat in statistics)
        {
            switch (stat.StatisticName)
            {
                case "Currency":
                    currency = stat.Value;
                    break;
                case "Kills":
                    kills = stat.Value;
                    break;
            }
        }
    }

    public void SetPlayerStatistics(int _currency, int _kills, Action onSuccess, Action<EPlayerDataError> onError)
    {
        var requestStats = new List<StatisticUpdate>
        {
            new StatisticUpdate { StatisticName = "Currency", Value = _currency },
            new StatisticUpdate { StatisticName = "Kills", Value = _kills }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest()
        {
            Statistics = requestStats
        },
        result =>
        {
            currency = _currency;
            kills = _kills;
            onSuccess?.Invoke();
        },
        error =>
        {
            onError?.Invoke(EPlayerDataError.UpdatePlayerStatisticsFailed);
        });
    }

    #endregion

    #region Title data (Clan)

    private void UpdateUserDataInternal(Dictionary<string, UserDataRecord> _userData)
    {
        userData = _userData;

        if (userData.ContainsKey("Clan"))
        {
            clan = userData["Clan"].Value;
        }
    }

    public void SetPlayerClan(string _clan, Action onSuccess, Action<EPlayerDataError> onError)
    {
        var requestData = new Dictionary<string, string>
        {
            { "Clan", _clan }
        };

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = requestData
        },
        result =>
        {
            clan = _clan;
            onSuccess?.Invoke();
        },
        error =>
        {
            onError?.Invoke(EPlayerDataError.UpdateUserDataFailed);
        });
    }

    public void GetPlayerTitleData(Action _getTitleDataCallback, Action<EPlayerDataError> _errorHandler)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
        result =>
        {
            UpdateUserDataInternal(result.Data);
            _getTitleDataCallback?.Invoke();
        },
        error =>
        {
            _errorHandler?.Invoke(EPlayerDataError.LoadUserDataFailed);
        });
    }

    #endregion
}
