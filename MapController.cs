using System.Collections;
using UnityEngine;

public class MapController : MonoBehaviour
{
    // 변수 이름 고민해야 함
    public float nextPhaseTime;
    public float timeBetChangeCube;
    
    [SerializeField]
    private GameObject[] disadvantageCubePrefabs = null;
    [SerializeField]
    private GameObject[] originalMap = null;
    private GameObject[] lavaMap = null;

    private string poolItemName = "DisadvantageCube";

    private int maxCount = 128;
    private int currentMaxCount = 32;
    private int previousCount;

    private bool isChange = true;

    private void Awake()
    {
        // Setting DisadvantageCube
        foreach (var t in disadvantageCubePrefabs)
        {
            if (t.gameObject.GetComponent<BoxCollider>() == null)
            {
                t.gameObject.AddComponent<BoxCollider>();
            }

            if (t.gameObject.tag.Equals("Damage Zone") == false)
            {
                t.gameObject.tag = "Damage Zone";
            }
        }
        
        lavaMap = new GameObject[originalMap.Length];
    }

    private void Start()
    {
        StartCoroutine(MapChange());
    }

    private IEnumerator MapChange()
    {
        while(GameManager.Instance.State != GameManager.GameState.Play)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(nextPhaseTime);

        int selectedRandomIndex;

        if (isChange)
        {
            // Debug.Log("<b><color=Blue>Map Change to Lava Map</color></b>");

            for (var i = previousCount; i < currentMaxCount; i++)
            {
                do
                {
                    selectedRandomIndex = Random.Range(0, originalMap.Length);
                } while (originalMap[selectedRandomIndex].gameObject.activeSelf == false);

                var disadvantageCube =
                    ObjectPoolingManager.Instance.PopFromPool(poolItemName + Random.Range(1, 5));

                if (disadvantageCube != null)
                {
                    disadvantageCube.transform.position = originalMap[selectedRandomIndex].transform.position;
                    disadvantageCube.transform.rotation = Quaternion.identity;
                    disadvantageCube.transform.localScale = new Vector3(2f, 2f, 2f);
                    lavaMap[selectedRandomIndex] = disadvantageCube;
                    disadvantageCube.SetActive(true);

                    originalMap[selectedRandomIndex].gameObject.SetActive(false);
                }

                yield return new WaitForSecondsRealtime(timeBetChangeCube);
            }

            previousCount = currentMaxCount;
            currentMaxCount *= 2;

            if (currentMaxCount > maxCount)
            {
                isChange = false;
            }
        }
        else
        {
            // Debug.Log("<b><color=Red>Map Change to Original Map</color></b>");

            currentMaxCount /= 2;
            previousCount = currentMaxCount / 2;

            for (var i = currentMaxCount - 1; i >= previousCount; i--)
            {
                do
                {
                    selectedRandomIndex = Random.Range(0, originalMap.Length);
                } while (originalMap[selectedRandomIndex].gameObject.activeSelf == true);

                ObjectPoolingManager.Instance.PushToPool(lavaMap[selectedRandomIndex].gameObject.name,
                    lavaMap[selectedRandomIndex].gameObject);

                originalMap[selectedRandomIndex].gameObject.SetActive(true);

                yield return new WaitForSeconds(timeBetChangeCube);
            }

            if (currentMaxCount <= 1)
            {
                isChange = true;
                currentMaxCount = 32;
                previousCount = 0;
            }
        }
    }
}
