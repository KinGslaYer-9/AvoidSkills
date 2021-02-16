using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton Property
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    var container = new GameObject("GameManager");

                    _instance = container.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }

    #endregion
    
    public enum GameState
    {
        Ready,
        Play,
        End
    }

    public GameState State;
    
    private float score;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InGameUIManager.Instance.SetActiveReadyUI();
        
        AudioManager.Instance.Play("Sea");
        
        score = 0;
        State = GameState.Ready;
    }

    private void Update()
    {
        if(State == GameState.Play)
        {
            score += Time.deltaTime;
            InGameUIManager.Instance.UpdateSurviveScoreText(score);
        }
    }
    
    private void OnDestroy()
    {
        if (_instance != this)
        {
            return;
        }

        _instance = null;
    }

    public void AddScore(float newScore)
    {
        if(State == GameState.Play)
        {
            score += newScore;
            InGameUIManager.Instance.UpdateSurviveScoreText(score);
        }
    }

    public void EndGame()
    {
        if (GlobalUIManager.Instance.IsMenuOpen)
        {
            GlobalUIManager.Instance.CloseMenuWindow();
        }
        
        State = GameState.End;
        InGameUIManager.Instance.SetActiveGameoverUI(true);

        var bestScore = PlayerPrefs.GetFloat("BestScore");

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetFloat("BestScore", bestScore);
        }

        InGameUIManager.Instance.UpdateBestScore((int) bestScore);
        InGameUIManager.Instance.UpdateCurrentScore((int) score);
    }
}
