using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Photon.Realtime;

public enum EScene : byte
{
    Title = 1,
    Lobby = 3,
    Ready = 4,
    Game = 5,
    Result = 6,
}

[RequireComponent(typeof(INetworkSceneManager), typeof(INetworkObjectProvider))]
public class NetworkManager : Manager
{
    [SerializeField] NetworkRunner netRunner;

    private INetworkSceneManager netSceneManager;
    private INetworkObjectProvider netObjectProvider;

    // NetworkRunnmer.CloudServices
    // CloudServices.CloudCommunicator
    // CloudCommnuicator.FusionRelayClient -> Realtime.LoadBalancingClient
    public NetworkRunner Runner => netRunner;
    public SessionInfo Session => netRunner.SessionInfo;

    protected override void Awake()
    {
        base.Awake();

        netSceneManager = GetComponent<INetworkSceneManager>();
        netObjectProvider = GetComponent<INetworkObjectProvider>();
    }

    public void CreateMatch(Action _onComplete = null)
    {
        StartCoroutine(CreateMatchInternal(_onComplete));
    }

    public void FindMatch(Action _onComplete = null)
    {
        StartCoroutine(FindMatchInternal(_onComplete));
    }

    public void LeaveMatch(Action _onComplete = null)
    {
        netRunner.Shutdown();
        SceneManager.LoadScene((int)EScene.Lobby);

        try { _onComplete?.Invoke(); }
        catch (Exception error)
        {
            Debug.LogError("Exception was thrown while invoking OnComplete of LeaveMatch. " +
                $"{error.Message}\n{error.StackTrace}");
        }
    }

    public void StartGame(Action _onComplete = null)
    {
        StartCoroutine(StartGameInternal(_onComplete));
    }

    public void ShowResult(Action _onComplete = null)
    {
        //var elapsed = App.Manager.UI.GetPanel<Panel_Time>().Elapsed;
        //App.PlayerInfo.ElapsedMin = elapsed.Item1;
        //App.PlayerInfo.ElapsedSec = elapsed.Item2;

        //StartCoroutine(ShowResultInternal(_onComplete));
    }

    public void ReturnToLobby(Action _onComplete = null)
    {
        StartCoroutine(ReturnToLobbyInternal(_onComplete));
    }

    private IEnumerator CreateMatchInternal(Action _onComplete)
    {
        var joinTask = netRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = $"ROOM_{UnityEngine.Random.Range(0, 10000)}",
            IsOpen = false,
            IsVisible = false,
            UseCachedRegions = true,
            SceneManager = netSceneManager,
            ObjectProvider = netObjectProvider,
        });

        yield return new WaitUntil(() => joinTask.IsCompleted);

        var joinTaskResult = joinTask.Result;
        if (!joinTaskResult.Ok)
        {
            // TODO: handle error case
            Debug.LogError("Failed to join game. Exiting...");
            LeaveMatch();
            yield break;
        }

        SceneManager.LoadScene((int)EScene.Ready);

        try { _onComplete?.Invoke(); }
        catch (Exception error)
        {
            Debug.LogError("Exception was thrown while invoking OnComplete of FindMatch. " +
                $"{error.Message}\n{error.StackTrace}");
        }
    }

    private IEnumerator FindMatchInternal(Action _onComplete)
    {
        var joinTask = netRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            MatchmakingMode = MatchmakingMode.FillRoom,
            IsOpen = true,
            IsVisible = true,
            UseCachedRegions = true,
            SceneManager = netSceneManager,
            ObjectProvider = netObjectProvider,
        });

        yield return new WaitUntil(() => joinTask.IsCompleted);

        var joinTaskResult = joinTask.Result;
        if (!joinTaskResult.Ok)
        {
            // TODO: handle error case
            Debug.LogError("Failed to join game. Exiting...");
            LeaveMatch();
            yield break;
        }

        SceneManager.LoadScene((int)EScene.Ready);

        try { _onComplete?.Invoke(); }
        catch (Exception error)
        {
            Debug.LogError("Exception was thrown while invoking OnComplete of FindMatch. " +
                $"{error.Message}\n{error.StackTrace}");
        }
    }

    private IEnumerator StartGameInternal(Action _onComplete)
    {
        netRunner.SessionInfo.IsOpen = false;
        var loadTask = netRunner.LoadScene(SceneRef.FromIndex((int)EScene.Game));

        yield return new WaitUntil(() => loadTask.IsDone);

        try { _onComplete?.Invoke(); }
        catch (Exception error)
        {
            Debug.LogError("Exception was thrown while invoking OnComplete of StartGame. " +
                $"{error.Message}\n{error.StackTrace}");
        }
    }

    private IEnumerator ShowResultInternal(Action _onComplete)
    {
        var loadTask = netRunner.LoadScene(SceneRef.FromIndex((int)EScene.Result));

        yield return new WaitUntil(() => loadTask.IsDone);

        try { _onComplete?.Invoke(); }
        catch (Exception error)
        {
            Debug.LogError("Exception was thrown while invoking OnComplete of ShowResult. " +
                $"{error.Message}\n{error.StackTrace}");
        }
    }

    private IEnumerator ReturnToLobbyInternal(Action _onComplete)
    {
        var loadTask = netRunner.LoadScene(SceneRef.FromIndex((int)EScene.Lobby));

        yield return new WaitUntil(() => loadTask.IsDone);

        try { _onComplete?.Invoke(); }
        catch (Exception error)
        {
            Debug.LogError("Exception was thrown while invoking OnComplete of ReturnToLobby. " +
                $"{error.Message}\n{error.StackTrace}");
        }
    }
}
