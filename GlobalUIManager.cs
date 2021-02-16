using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalUIManager : MonoBehaviour
{
    public static GlobalUIManager Instance { get; private set; }
    
    [SerializeField]
    private ModalWindowManager exitModalWindow = null;
    [SerializeField]
    private ModalWindowManager menuModalWindow = null;

    public bool IsMenuOpen { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name.Equals("Lobby"))
            {
                exitModalWindow.OpenWindow();
            }
            else
            {
                if(GameManager.Instance.State == GameManager.GameState.Play)
                {
                    menuModalWindow.OpenWindow();
                    IsMenuOpen = true;
                }
            }
        }
#endif
    }
    
    private void OnDestroy()
    {
        if (Instance != this)
        {
            return;
        }

        Instance = null;
    }

    public void CloseMenuWindow()
    {
        menuModalWindow.CloseWindow();
        IsMenuOpen = false;
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }

    public void OnClickLoadLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
