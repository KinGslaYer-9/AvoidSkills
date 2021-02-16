using UnityEngine;

public class CharacterSelectController : MonoBehaviour
{
    public static int characterIndex;
    
    [SerializeField]
    private GameObject[] characters = null;

    private void Start()
    {
        characters[0].gameObject.SetActive(true);
        
        for (var i = 1; i < characters.Length; i++)
        {
            characters[i].gameObject.SetActive(false);
        }
    }

    public void ChangePreviousCharacter()
    {
        characters[characterIndex].gameObject.SetActive(false);
        
        characterIndex--;
        if (characterIndex < 0)
        {
            characterIndex = characters.Length - 1;
        }
        
        characters[characterIndex].gameObject.SetActive(true);
    }

    public void ChangeNextCharacter()
    {
        characters[characterIndex].gameObject.SetActive(false);
        
        characterIndex++;
        if (characterIndex >= characters.Length)
        {
            characterIndex = 0;
        }
        
        characters[characterIndex].gameObject.SetActive(true);
    }
}
