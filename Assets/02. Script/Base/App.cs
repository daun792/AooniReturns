using UnityEngine;
using DG.Tweening;

public enum SceneName
{
    Developer,
    Title,
    Lobby,
    Game
}

public class App : Singleton<App>
{
    private readonly SoundManager sound;
    private readonly UIManager ui;
    private readonly LobbyUIManager lobby;

    private readonly SettingData setting;

    #region Getter Setter
    public partial class Manager
    {
        public static SoundManager Sound => instance.sound;
        public static UIManager UI => instance.ui;
        public static LobbyUIManager Lobby => instance.lobby;
    }

    public partial class Data
    {
        public static SettingData Setting => instance.setting;
    }
    #endregion

    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 120;

        DOTween.safeModeLogBehaviour = DG.Tweening.Core.Enums.SafeModeLogBehaviour.Error;

        DontDestroyOnLoad(gameObject);
    }

    public static void LoadScene(SceneName sceneName)
    {
        DOTween.KillAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)sceneName);
    }
}
