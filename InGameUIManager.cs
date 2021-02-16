using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance { get; private set; }
    
    [SerializeField]
    private GameObject gameoverUI = null;
    [SerializeField]
    private GameObject readyUI = null;
    [SerializeField]
    private TMP_Text countDownText = null;
    
    [SerializeField]
    private TMP_Text surviveScoreText = null;
    [SerializeField]
    private TMP_Text healthText = null;
    [SerializeField]
    private TMP_Text highestRecordText = null;
    [SerializeField]
    private TMP_Text currentRecordText = null;

    private CanvasGroup canvasGroup;
    
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

        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    private void OnDestroy()
    {
        if (Instance != this)
        {
            return;
        }

        Instance = null;
    }

    public void UpdateSurviveScoreText(float score)
    {
        surviveScoreText.text = "생존 점수 : <b>" + (int) score + "</b>";
    }

    public void UpdateHealthText(float health)
    {
        if (health < 0)
        {
            healthText.text = "현재 체력 : <b>0</b>";
            return;
        }
        
        healthText.text = "현재 체력 : <b>" + (int) health + "</b>";
    }

    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }
    
    public void SetActiveReadyUI()
    {
        canvasGroup.interactable = false;
        readyUI.SetActive(true);

        StartCoroutine(StartCountDown());
    }

    private IEnumerator StartCountDown()
    {
        for (var i = 5; i >= 0; i--)
        {
            if (i > 0)
            {
                countDownText.text = i.ToString();   
            }
            else
            {
                countDownText.fontSize = 50f;
                countDownText.text = "스킬을 피하세요!";
            }
            yield return new WaitForSecondsRealtime(1f);
        }

        canvasGroup.interactable = true;
        readyUI.SetActive(false);
        GameManager.Instance.State = GameManager.GameState.Play;
    }

    public void UpdateCurrentScore(int score)
    {
        currentRecordText.text = "현재 점수 : <b>" + score + "<b>";
    }

    public void UpdateBestScore(int score)
    {
        highestRecordText.text = "최고 점수 : <b>" + score + "</b>";
    }

    public void GameRestart()
    {
#if !UNITY_EDITOR
        AdmobManager.Instance.Show();
#endif
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
