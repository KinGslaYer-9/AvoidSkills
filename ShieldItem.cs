using UnityEngine;

public class ShieldItem : MonoBehaviour, IItem
{
    [SerializeField]
    private string poolItemName = "ShieldItem";
    
    public void Use(GameObject target, out IItem.ItemType type)
    {
        type = IItem.ItemType.Shield;
        
        var playerHealth = target.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            ObjectPoolingManager.Instance.PushToPool(poolItemName, gameObject);
        }
    }
}
