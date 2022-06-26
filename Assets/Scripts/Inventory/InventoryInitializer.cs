using UnityEngine;

[RequireComponent(typeof(InventoryBase))]
public class InventoryInitializer : MonoBehaviour
{
    public int[] addItemsID;

    private void Start()
    {
        SlotBase[] slots = GetComponent<InventoryBase>().GetAllSlots<SlotBase>();
        for(int i = 0; i < addItemsID.Length && i < slots.Length; i++)
        {
            slots[i].AddItem(ItemDatabase.instance.GetItem(addItemsID[i]));
        }
    }
}
