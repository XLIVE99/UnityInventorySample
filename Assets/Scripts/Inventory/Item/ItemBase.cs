using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public string itemName { get; protected set; }
    public int itemID { get; protected set; }
    public int itemGroupID { get; protected set; }

    public virtual void Initialize(ItemDatabase.ItemInfo info)
    {
        itemID = info.itemID;
        itemGroupID = info.itemGroupID;

        itemName = info.itemName;
    }
}
