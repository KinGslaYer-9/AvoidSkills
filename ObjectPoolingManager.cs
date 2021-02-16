using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{   
    public static ObjectPoolingManager Instance { get; private set; }
    
    public List<PooledObject> pooledObjects = new List<PooledObject>();
    
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
        
        for (var i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].Initialize(transform);
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

    public bool PushToPool(string itemName, GameObject item, Transform parent = null)
    {
        var pool = GetPoolItem(itemName);

        if (pool == null)
        {
            return false;
        }

        pool.PushToPool(item, parent == null ? transform : parent);
        return true;
    }

    public GameObject PopFromPool(string itemName, Transform parent = null)
    {
        var pool = GetPoolItem(itemName);

        if (pool == null)
        {
            return null;
        }

        return pool.PopFromPool(parent);
    }
    
    private PooledObject GetPoolItem(string itemName)
    {
        for (var i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].poolItemName.Equals(itemName))
            {
                return pooledObjects[i];
            }
        }

        Debug.LogWarning("There's no matched pool list.");
        return null;
    }
}
