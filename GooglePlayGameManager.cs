using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;

public class GooglePlayGameManager : MonoBehaviour
{
    #region Singleton

    private static GooglePlayGameManager _instance;
    public static GooglePlayGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<GooglePlayGameManager>();
                if (obj != null)
                {
                    _instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<GooglePlayGameManager>();
                    _instance = newObj;
                }
            }
            return _instance;
        }
    }

    #endregion

    [SerializeField]
    private Button logInButton = null;
    [SerializeField]
    private Button logOutButton = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        LogIn();
    }

    private void OnDestroy()
    {
        if (_instance != this)
        {
            return;
        }

        _instance = null;
    }

    public void LogIn()
    {
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                logInButton.gameObject.SetActive(false);
                logOutButton.gameObject.SetActive(true);
            });
        }
    }

    public void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        logOutButton.gameObject.SetActive(false);
        logInButton.gameObject.SetActive(true);
    }
}
