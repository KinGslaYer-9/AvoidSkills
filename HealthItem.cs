using UnityEngine;

public class HealthItem : MonoBehaviour, IItem
{
    [SerializeField]
    private string poolItemName = "HealthItem";

    public void Use(GameObject target, out IItem.ItemType type)
    {
        type = IItem.ItemType.Health;
        
        var playerHealth = target.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.RestoreHealth(Random.Range(10, 50));
            ObjectPoolingManager.Instance.PushToPool(poolItemName, gameObject);
        }
    }
}
