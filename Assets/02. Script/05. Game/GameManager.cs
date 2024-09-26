using UnityEngine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum GameState
{
    None,
    Begin,
    CountDown,
    Play,
    Over,
}

public class GameManager : NetManager
{
    [Header("Network")]
    [SerializeField] NetworkObject netPlayerObject;
    [SerializeField] Transform respawnPos;

    // networked properties
    [Networked] public bool GamePlay { get; private set; } = false;
    [Networked] public int RoundCount { get; private set; } = 1;

    private GameState currState = GameState.None;
    private bool isGamePlaying => currState != GameState.Over;
    
    public Vector3 RespawnPosition => respawnPos.position;
    public Quaternion RespawnRotation => respawnPos.rotation;

    private NetworkObject myPlayerObject;
    public int[] RandomNum { get; private set; }

    public List<Transform> spawnPositions;

    private float internalTime = 0;

    protected override void Awake()
    {
        base.Awake();

        InitializeSpawnPositions();
    }

    public override void Spawned()
    {
        StartCoroutine(Initialize());
        StartCoroutine(InternalGameLoop());
    }

    private IEnumerator Initialize()
    {
        //var spawnLoc = new Vector3(5f, 0f, 0f);

        spawnPositions.ElementAt(Random.Range(0, spawnPositions.Count))
            .GetPositionAndRotation(out var spawnLoc, out var spawnRot);

        var spawnTask = Runner.SpawnAsync(netPlayerObject, spawnLoc, spawnRot,
            Runner.LocalPlayer, null, NetworkSpawnFlags.SharedModeStateAuthLocalPlayer);

        yield return new WaitUntil(() => spawnTask.GetAwaiter().IsCompleted);

        if (spawnTask.IsFailed)
        {
            Debug.LogError("Unable to spawn player object. Quiting...");
            App.Manager.Network.LeaveMatch();
            yield break;
        }

        myPlayerObject = spawnTask.GetAwaiter().GetResult();

        App.Manager.Sound.PlayBGM("BGM_Game");

        if (HasStateAuthority)
        {
            // 뱌로 보내면 안감. 실패 이유 구글링해도 안나옴.
            // ObjectNotConfirmed라는데 서버에서 해주는건 있지도 않으면서 확인은 왜한다는건지 모르겠음
            // 도대체 얘내가 뭘 추구하고 뭘 위해서 이렇게 하는지도 모르겠음
            // TargetObjectVerificationResult.ObjectNotConfirmed
            yield return new WaitForSeconds(1f);
            //GenerateRandomNumber();
        }
    }

    private IEnumerator InternalGameLoop()
    {
        do
        {
            internalTime -= Time.deltaTime;
            if (internalTime > 0f)
            {
                yield return null;
                continue;
            }

            var prevState = currState;
            var nextState = GetNextState(currState);
            currState = nextState;

            if (nextState == GameState.None)
            {
                Debug.LogError($"Impossible route detected. {prevState} > {nextState}");
                yield break;
            }

            internalTime = GetRequiredTime(nextState); // get required time for next state

            if (nextState == GameState.Begin)
            {
                RoundCount++;
            }

            if ((int)internalTime < 0)
            {
                yield break;
            }
        }
        while (isGamePlaying);
    }

    private GameState GetNextState(GameState _prevState)
    {
        var nextState = GameState.None;

        switch (_prevState)
        {
            case GameState.None:
                nextState = GameState.Begin;
                App.Manager.UI.GetPanel<RoundPanel>().OpenPanel();
                App.Manager.UI.GetPanel<TimePanel>().ClosePanel();
                App.Manager.UI.GetPanel<NoticePanel>().NoticeBeforeGameStart();
                break;

            case GameState.Begin:
                nextState = GameState.CountDown;
                App.Manager.UI.GetPanel<NoticePanel>().NoticeCountDown();
                break;

            case GameState.CountDown:
                nextState = GameState.Play;
                GamePlay = true;
                App.Manager.UI.GetPanel<RoundPanel>().ClosePanel();
                App.Manager.UI.GetPanel<TimePanel>().OpenPanel();
                break;

            case GameState.Play:
                GamePlay = false;

                if (RoundCount >= 8)
                {
                    nextState = GameState.Over;
                }
                else
                {
                    nextState = GameState.Begin;
                    App.Manager.UI.GetPanel<RoundPanel>().OpenPanel();
                    App.Manager.UI.GetPanel<TimePanel>().ClosePanel();
                    App.Manager.UI.GetPanel<NoticePanel>().NoticeBeforeGameStart();
                }
                break;
        }

        Debug.Log(nextState);

        return nextState;
    }

    private float GetRequiredTime(GameState _time) => _time switch
    {
        GameState.Begin => 5,
        GameState.CountDown => 10,
        GameState.Play => 120,
        _ => 0,
    };

    //private void GenerateRandomNumber()
    //{
    //    int randomNumber = Random.Range(10000, 100000); // Generates a number between 10000 and 99999
    //    string randomNumberStr = randomNumber.ToString();

    //    Debug.LogError(randomNumber);

    //    RandomNum = new int[randomNumberStr.Length];

    //    for (int i = 0; i < randomNumberStr.Length; i++)
    //    {
    //        RandomNum[i] = int.Parse(randomNumberStr[i].ToString());
    //    }

    //    RPC_GenerateRandomNumber(RandomNum);
    //}

    //[Rpc]
    //private void RPC_GenerateRandomNumber(int[] _rands)
    //{
    //    RandomNum = _rands;

    //    Debug.LogError(string.Join(' ', _rands));
    //}

    private void InitializeSpawnPositions()
    {
        //spawnPositions = FindObjectsOfType<SpawnPoint>().Select(x => x.transform).ToList();
    }

   

    public override void Render()
    {
        if (!Runner.IsSceneAuthority || GamePlay)
        {
            return;
        }

        var players = App.Manager.Player.AllPlayers;

        if (players.Count == 0)
        {
            return;
        }

        if (App.Manager.Player.OniPlayers.Count == players.Count)
        {
            internalTime = 0;
            return;
        }

        if (GetOniAllDead())
        {
            internalTime = 0;
            return;
        }

        if (App.Manager.UI.GetPanel<TimePanel>().Remaining <= 0f)
        {
            internalTime = 0;
            return;
        }
    }

    public bool GetOniAllDead()
    {
        foreach (var charCtrl in App.Manager.Player.OniPlayers)
        {
            if (charCtrl.Dead == true)
            {
                continue;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}

