using UnityEngine;
using Fusion;

public class GameManager : MonoBehaviour
{
    private NetworkRunner _networkRunner;

    private void Start()
    {
        _networkRunner = gameObject.AddComponent<NetworkRunner>();
        _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
        });
    }
}
