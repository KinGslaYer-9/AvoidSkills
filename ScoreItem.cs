using UnityEngine;

public class ScoreItem : MonoBehaviour, IItem
{
    [SerializeField]
    private string poolItemName = "ScoreItem";

    public void Use(GameObject target, out IItem.ItemType type)
    {
        type = IItem.ItemType.Score;
        
        GameManager.Instance.AddScore(Random.Range(10, 50));
        ObjectPoolingManager.Instance.PushToPool(poolItemName, gameObject);
    }
}
