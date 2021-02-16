using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject muteButton = null;
    [SerializeField]
    private GameObject unMuteButton = null;

    private CanvasGroup canvasGroup;
    
    [SerializeField]
    private GameObject tutorialPanel = null;
    [SerializeField]
    private GameObject background = null;

    [SerializeField]
    private CharacterSelectController characterSelectController = null;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnClickSetMute()
    {
        AudioManager.Instance.Play("ButtonClickOpen");
        AudioManager.Instance.SetMute();

        muteButton.SetActive(false);
        unMuteButton.SetActive(true);
    }

    public void OnClickSetUnMute()
    {
        AudioManager.Instance.SetUnMute();
        
        muteButton.SetActive(true);
        unMuteButton.SetActive(false);
    }
    
    public void OnClickGameStart()
    {
        LoadingSceneController.Instance.LoadScene("Main");
    }

    public void OnClickOpenTutorial()
    {
        AudioManager.Instance.Play("ButtonClickOpen");

        canvasGroup.interactable = false;
        
        background.SetActive(true);
        tutorialPanel.SetActive(true);
    }
    
    public void OnClickCloseTutorial()
    {
        AudioManager.Instance.Play("ButtonClickClose");

        canvasGroup.interactable = true;
        
        tutorialPanel.SetActive(false);
        background.SetActive(false);
    }

    public void OnClickPreviousCharacterButton()
    {
        AudioManager.Instance.Play("ButtonClickOpen");
        characterSelectController.ChangePreviousCharacter();
    }

    public void OnClickNextCharacterButton()
    {
        AudioManager.Instance.Play("ButtonClickOpen");
        characterSelectController.ChangeNextCharacter();
    }
}
