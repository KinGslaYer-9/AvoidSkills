using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    #region Singleton Property

    public static LoadingSceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if (obj != null)
                {
                    _instance = obj;
                }
                else
                {
                    _instance = Create();
                }
            }

            return _instance;
        }
    }
    
    private static LoadingSceneController _instance;

    #endregion

    private static LoadingSceneController Create()
    {
        return Instantiate(Resources.Load<LoadingSceneController>("LoadingUI"));
    }
    
    private void Awake()
    {
        if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    
    private void OnDestroy()
    {
        if (_instance != this)
        {
            return;
        }

        _instance = null;
        Resources.UnloadUnusedAssets();
    }

    [SerializeField]
    private CanvasGroup canvasGroup = null;

    private string loadSceneName;
    private float progressValue;

    public void LoadScene(string sceneName)
    {
        AudioManager.Instance.Stop("Sea");
        
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        progressValue = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        var timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressValue = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressValue = Mathf.Lerp(0.9f, 1f, timer);
                if (progressValue >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        var timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
