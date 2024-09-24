using UnityEngine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : NetManager
{
    [Header("Network")]
    [SerializeField] NetworkObject netPlayerObject;
    [SerializeField] Transform respawnPos;

    // networked properties
    [Networked] public bool GameOver { get; private set; } = false;

    public Vector3 RespawnPosition => respawnPos.position;
    public Quaternion RespawnRotation => respawnPos.rotation;

    private NetworkObject myPlayerObject;
    public int[] RandomNum { get; private set; }

    private List<Transform> spawnPositions;

    protected override void Awake()
    {
        base.Awake();

        InitializeSpawnPositions();
    }

    private void GenerateRandomNumber()
    {
        int randomNumber = Random.Range(10000, 100000); // Generates a number between 10000 and 99999
        string randomNumberStr = randomNumber.ToString();

        Debug.LogError(randomNumber);

        RandomNum = new int[randomNumberStr.Length];

        for (int i = 0; i < randomNumberStr.Length; i++)
        {
            RandomNum[i] = int.Parse(randomNumberStr[i].ToString());
        }

        RPC_GenerateRandomNumber(RandomNum);
    }

    [Rpc]
    private void RPC_GenerateRandomNumber(int[] _rands)
    {
        RandomNum = _rands;

        Debug.LogError(string.Join(' ', _rands));
    }

    private void InitializeSpawnPositions()
    {
        //spawnPositions = FindObjectsOfType<SpawnPoint>().Select(x => x.transform).ToList();
    }

    public override void Spawned()
    {
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

        App.Manager.Sound.PlayBGM("Ambience");

        if (HasStateAuthority)
        {
            // 뱌로 보내면 안감. 실패 이유 구글링해도 안나옴.
            // ObjectNotConfirmed라는데 서버에서 해주는건 있지도 않으면서 확인은 왜한다는건지 모르겠음
            // 도대체 얘내가 뭘 추구하고 뭘 위해서 이렇게 하는지도 모르겠음
            // TargetObjectVerificationResult.ObjectNotConfirmed
            yield return new WaitForSeconds(1f);
            GenerateRandomNumber();
        }
    }

    public override void Render()
    {
        if (!Runner.IsSceneAuthority || GameOver)
        {
            return;
        }

        var bustedCount = 0;
        var escapedCount = 0;
        var players = App.Manager.Player.AllPlayers;

        if (players.Count == 0)
        {
            return;
        }

        for (int i = 0; i < players.Count; ++i)
        {
            var player = players[i];
            if (player.IsBusted)
            {
                ++bustedCount;
            }
            else if (player.Escaped)
            {
                ++escapedCount;
            }
        }

        if (bustedCount + escapedCount >= players.Count)
        {
            App.Manager.Network.ShowResult();
            GameOver = true;
        }

        //if (App.Manager.UI.GetPanel<TimePanel>().Remaining <= 0f)
        //{
        //    App.Manager.Network.ShowResult();
        //    GameOver = true;
        //}
    }
}

