using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SpawnObject
{
    public string[] poolItemName;

    public float maxDistance;
    public float timeBetSpawnMax;
    public float timeBetSpawnMin;

    public float spawnHeight;
    public Vector3 objectScale;

    public float disappearTime;

    public bool isSkill;

    [HideInInspector]
    public float timeBetSpawn;
    [HideInInspector]
    public float lastSpawnTime;
}

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }
    
    [SerializeField]
    private GameObject[] characters = null;
    [SerializeField]
    private Transform playerSpawnPosition = null;
    
    [SerializeField]
    private SpawnObject[] spawnObjects = null;
    
    private Transform playerTransform = null;

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
        
        foreach (var t in spawnObjects)
        {
            var min = t.timeBetSpawnMin;
            var max = t.timeBetSpawnMax;

            t.timeBetSpawn = Random.Range(min, max);
            t.lastSpawnTime = 0f;
        }
        
        var player = Instantiate(characters[CharacterSelectController.characterIndex], playerSpawnPosition);
        playerTransform = player.transform;
    }

    private void Update()
    {
        if(GameManager.Instance.State != GameManager.GameState.Play)
        {
            return;
        }
        
        for (var i = 0; i < spawnObjects.Length; i++)
        {
            if (Time.time >= spawnObjects[i].lastSpawnTime + spawnObjects[i].timeBetSpawn && playerTransform != null)
            {
                spawnObjects[i].lastSpawnTime = Time.time;
                spawnObjects[i].timeBetSpawn =
                    Random.Range(spawnObjects[i].timeBetSpawnMin, spawnObjects[i].timeBetSpawnMax);
                
                Spawn(spawnObjects[i]);
            }
        }
    }
    
    private void OnDestroy()
    {
        if (Instance != this)
        {
            return;
        }

        Instance = null;
    }

    private void Spawn(SpawnObject spawnObject)
    {
        var spawnPosition = Utility.GetRandomPointOnNavMesh(playerTransform.position, spawnObject.maxDistance, NavMesh.AllAreas);

        spawnPosition += Vector3.up * spawnObject.spawnHeight;

        var selectedObject = spawnObject.poolItemName[Random.Range(0, spawnObject.poolItemName.Length)];
        var createObject = ObjectPoolingManager.Instance.PopFromPool(selectedObject);

        if (createObject != null)
        {
            createObject.transform.position = spawnPosition;
            createObject.transform.rotation = Quaternion.identity;
            createObject.transform.localScale = spawnObject.objectScale;
            createObject.SetActive(true);
        }

        // Setting Skill DisappearTime
        if (spawnObject.isSkill)
        {
            var effectController = createObject.GetComponent<EffectController>();

            if (effectController != null)
            {
                effectController.enabled = true;

                spawnObject.disappearTime = effectController.EffectDisableTime;
            }
        }

        StartCoroutine(PushToPoolSpawnObject(selectedObject, spawnObject.disappearTime, createObject));
    }

    private IEnumerator PushToPoolSpawnObject(string poolItemName, float disappearTime, GameObject createObject)
    {
        yield return new WaitForSeconds(disappearTime);

        ObjectPoolingManager.Instance.PushToPool(poolItemName, createObject);
    }
}
