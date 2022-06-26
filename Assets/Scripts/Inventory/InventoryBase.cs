using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    [SerializeField] private InventoryBase _interactingInv;
    public InventoryBase interactingInv => _interactingInv;

    [SerializeField] private Transform slotParent;

    public bool IsSlotType<T>()
    {
        return slotParent.GetChild(0).TryGetComponent(out T _);
    }

    public T GetSlot<T>(int index)
    {
        return slotParent.GetChild(index).GetComponent<T>();
    }

    public T[] GetAllSlots<T>()
    {
        T[] result = new T[slotParent.childCount];

        for(int i = 0; i < slotParent.childCount; i++)
        {
            result[i] = slotParent.GetChild(i).GetComponent<T>();
        }

        return result;
    }
}
