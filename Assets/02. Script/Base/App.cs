using UnityEngine;
using DG.Tweening;

public enum SceneName
{
    Developer,
    Title,
    Notice,
    Lobby,
    Game
}

public class App : Singleton<App>
{
    private ViewManager view;
    private UIManager ui;

    private ReadyManager ready;

    private SoundManager sound;
    private NetworkManager network;
    private PlayerManager player;
    private GameManager game;

    private TitleData titleData;
    private PlayerData playerData;
    private SettingData settingData;
    private ClanData clanData;

    #region Getter Setter
    public partial class Manager
    {
        public static UIManager UI => instance.ui;
        public static ViewManager View => instance.view;

        public static ReadyManager Ready => instance.ready;

        public static SoundManager Sound => instance.sound;
        public static NetworkManager Network => instance.network;
        public static PlayerManager Player => instance.player;
        public static GameManager Game => instance.game;
    }

    public partial class Data
    {
        public static SettingData Setting => instance.settingData;
        public static TitleData Title => instance.titleData;

        public static PlayerData Player => instance.playerData;
        public static ClanData Clan => instance.clanData;
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

    #region Get View As T
    public static T ViewManagerAs<T>(object manager) where T : MonoBehaviour
    {
        if (manager == null || manager is not T)
        {
            return null;
        }

        return manager as T;
    }

    public static T GetViewAs<T>() where T : MonoBehaviour
    {
        return ViewManagerAs<T>(Manager.View);
    }

    public static T GetUIAs<T>() where T : MonoBehaviour
    {
        return ViewManagerAs<T>(Manager.UI);
    }
    #endregion

    #region  Get ViewManager
    public static DeveloperManager GetDeveloperManager()
    {
        return GetViewAs<DeveloperManager>();
    }

    public static TitleManager GetTitleManager()
    {
        return GetViewAs<TitleManager>();
    }

    public static LobbyManager GetLobbyManager()
    {
        return GetViewAs<LobbyManager>();
    }

    public partial class Manager
    {
        public static DeveloperManager Developer { get => GetDeveloperManager(); }
        public static TitleManager Title { get => GetTitleManager(); }
        public static LobbyManager Lobby { get => GetLobbyManager(); }
    }
    #endregion

    #region  Get UIManager
    public static TitleUIManager GetTitleUIManager()
    {
        return GetUIAs<TitleUIManager>();
    }

    public static LobbyUIManager GetLobbyUIManager()
    {
        return GetUIAs<LobbyUIManager>();
    }

    public static GameUIManager GetGameUIManager()
    {
        return GetUIAs<GameUIManager>();
    }

    public class UI
    {
        public static TitleUIManager Title { get => GetTitleUIManager(); }
        public static LobbyUIManager Lobby { get => GetLobbyUIManager(); }
        public static GameUIManager Game { get => GetGameUIManager(); }
    }
    #endregion
}
