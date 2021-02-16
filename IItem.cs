using UnityEngine;

public interface IItem
{
    public enum ItemType
    {
        Health,
        Score,
        Shield
    }
    
    void Use(GameObject target, out ItemType type);
}
