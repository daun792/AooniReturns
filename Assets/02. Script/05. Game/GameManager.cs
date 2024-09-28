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

    private bool isGamePlay = false;
    public int RoundCount { get; private set; } = 0;

    private GameState currState = GameState.None;

    private NetworkObject myPlayerObject;

    public List<Transform> spawnPositions;

    private float internalTime = 1;

    protected override void Awake()
    {
        base.Awake();

        InitializeSpawnPositions();
    }

    public override void Spawned()
    {
        App.Manager.Sound.PlayBGM("BGM_Game");

        StartCoroutine(Initialize());
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

        if (Runner.IsSceneAuthority)
        {
            // 뱌로 보내면 안감. 실패 이유 구글링해도 안나옴.
            // ObjectNotConfirmed라는데 서버에서 해주는건 있지도 않으면서 확인은 왜한다는건지 모르겠음
            // 도대체 얘내가 뭘 추구하고 뭘 위해서 이렇게 하는지도 모르겠음
            // TargetObjectVerificationResult.ObjectNotConfirmed
            yield return new WaitForSeconds(1f);
            StartCoroutine(InternalGameLoop());
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

            var nextState = GetNextState(currState);

            RPC_ChangeState(nextState);

            internalTime = GetRequiredTime(nextState);

            currState = nextState;

            if ((int)internalTime < 0)
            {
                yield break;
            }
        }
        while (true);
    }

    private GameState GetNextState(GameState _prevState)
    {
        var nextState = GameState.None;

        switch (_prevState)
        {
            case GameState.None:
                nextState = GameState.Begin;
                break;

            case GameState.Begin:
                nextState = GameState.CountDown;
                break;

            case GameState.CountDown:
                nextState = GameState.Play;
                break;

            case GameState.Play:
                if (RoundCount >= 8)
                {
                    nextState = GameState.Over;
                }
                else
                {
                    nextState = GameState.Begin;
                }
                break;
        }

        return nextState;
    }

    private float GetRequiredTime(GameState _time) => _time switch
    {
        GameState.Begin => 3,
        GameState.CountDown => 10,
        GameState.Play => 120,
        _ => -1,
    };

    [Rpc]
    private void RPC_ChangeState(GameState _state, RpcInfo _info = default)
    {
        currState = _state;

        switch (_state)
        {
            case GameState.None:
                isGamePlay = false;
                Debug.LogError($"Impossible route detected. {_state}");
                break;

            case GameState.Begin:
                isGamePlay = false;
                RoundCount++;

                if (RoundCount > 1)
                {
                    App.Manager.Player.SetAllHuman();
                }

                App.Manager.UI.GetPanel<RoundPanel>().OpenPanel();
                App.Manager.UI.GetPanel<TimePanel>().ClosePanel();
                App.Manager.UI.GetPanel<NoticePanel>().NoticeBeforeGameStart();
                break;

            case GameState.CountDown:
                isGamePlay = false;
                App.Manager.UI.GetPanel<NoticePanel>().NoticeCountDown();
                break;

            case GameState.Play:
                isGamePlay = true;

                App.Manager.UI.GetPanel<RoundPanel>().ClosePanel();
                App.Manager.UI.GetPanel<TimePanel>().OpenPanel();
                SetRandomOni();
                break;

            case GameState.Over:
                isGamePlay = false;
                break;
        }
    }

    private void SetRandomOni()
    {
        if (!Runner.IsSceneAuthority)
        {
            return;
        }

        App.Manager.Player.SetRandomOni();
    }

    private void InitializeSpawnPositions()
    {
        //spawnPositions = FindObjectsOfType<SpawnPoint>().Select(x => x.transform).ToList();
    }

    public override void Render()
    {
        if (!Runner.IsSceneAuthority || !isGamePlay)
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
            isGamePlay = false;
            internalTime = 0;
            return;
        }

        if (GetOniAllDead())
        {
            isGamePlay = false;
            internalTime = 0;
            return;
        }

        if (App.Manager.UI.GetPanel<TimePanel>().Remaining <= 0f)
        {
            isGamePlay = false;
            internalTime = 0;
            return;
        }
    }

    public bool GetOniAllDead()
    {
        if (App.Manager.Player.OniPlayers.Count == 0) 
        {
            return false;
        }

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

