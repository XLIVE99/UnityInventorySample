using UnityEngine;

public class ItemConnection : ItemBase
{
    [HideInInspector] public SlotBase slot;

    public void RemoveFromSlot()
    {
        slot.RemoveItem(true);
    }
}
