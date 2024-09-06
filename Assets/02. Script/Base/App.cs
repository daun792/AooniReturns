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

    private SoundManager sound;

    private TitleData title;
    private PlayerData player;
    private SettingData setting;
    private ClanData clan;

    #region Getter Setter
    public partial class Manager
    {
        public static UIManager UI => instance.ui;
        public static ViewManager View => instance.view;

        public static SoundManager Sound => instance.sound;
    }

    public partial class Data
    {
        public static SettingData Setting => instance.setting;
        public static TitleData Title => instance.title;

        public static PlayerData Player => instance.player;
        public static ClanData Clan => instance.clan;
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

    public partial class View
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

    public class UI
    {
        public static TitleUIManager Title { get => GetTitleUIManager(); }
        public static LobbyUIManager Lobby { get => GetLobbyUIManager(); }
    }
    #endregion
}
