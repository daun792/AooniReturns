using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TitleData : Data
{
    //public Dictionary<string, CharData> charDatas = new();

    public bool isTitleDataLoaded { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadTitleData(Action _loadDataCallback, Action<PlayFabError> _errorHandler)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
                if (isTitleDataLoaded)
                {
                    return;
                }

                //charDatas.Clear();

                //var gameData = result.Data["GameData"];
                //var deserializedGameData = JsonUtilityHelper.FromJson<GameData>(gameData);
                //foreach (var data in deserializedGameData)
                //{
                //    if (data.version != Application.version)
                //    {
                //        _errorHandler(new PlayFabError
                //        {
                //            // steal one of the rarest code in playfab
                //            ErrorMessage = "InvalidVersion",
                //            Error = PlayFabErrorCode.VersionNotFound
                //        });
                //        return;
                //    }
                //}

                //var charData = result.Data["CharData"];
                //var charDataList = JsonUtilityHelper.FromJson<CharData>(charData);
                //foreach (var data in charDataList)
                //{
                //    charDatas.Add(data.code, data);
                //}

                isTitleDataLoaded = true;
                _loadDataCallback();
            },
            _errorHandler);
    }
}

public class JsonUtilityHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "";
        if (json[0] == '{')
        {
            newJson = json;
        }
        else
        {
            newJson = "{ \"array\": " + json + "}";
        }
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.array = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}