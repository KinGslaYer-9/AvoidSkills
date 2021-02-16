using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PooledObject
{
    public string poolItemName = string.Empty;
    public GameObject prefab = null;
    public int poolCount = 0;
    [SerializeField]
    private List<GameObject> poolList = new List<GameObject>();

    public void Initialize(Transform parent = null)
    {
        for (var i = 0; i < poolCount; i++)
        {
            poolList.Add(CreateItem(parent));
        }
    }

    public void PushToPool(GameObject item, Transform parent = null)
    {
        item.transform.SetParent(parent);
        item.SetActive(false);
        poolList.Add(item);
    }

    public GameObject PopFromPool(Transform parent = null)
    {
        if (poolList.Count == 0)
        {
            poolList.Add(CreateItem(parent));
        }

        var item = poolList[0];
        poolList.RemoveAt(0);
        return item;
    }
    
    private GameObject CreateItem(Transform parent = null)
    {
        var item = Object.Instantiate(prefab, parent);
        item.name = poolItemName;
        item.transform.SetParent(parent);
        item.SetActive(false);
        return item;
    }
}
