using UnityEngine;

public class ItemConnection : ItemBase
{
    [HideInInspector] public SlotBase slot;

    public override void Initialize(ItemDatabase.ItemInfo info)
    {
        itemName = "Blocked";
        itemID = -1;
    }

    public void RemoveFromSlot()
    {
        slot.RemoveItem(true);
    }
}
