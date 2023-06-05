using UnityEngine;

[RequireComponent(typeof(InventoryBase))]
public class InventoryInitializer : MonoBehaviour
{
    public ItemBaseScriptable[] items;

    private void Start()
    {
        SlotBase[] slots = GetComponent<InventoryBase>().GetAllSlots<SlotBase>();
        for(int i = 0; i < items.Length && i < slots.Length; i++)
        {
            slots[i].AddItem(ItemDatabase.instance.CreateItem(items[i]));
        }
    }
}
