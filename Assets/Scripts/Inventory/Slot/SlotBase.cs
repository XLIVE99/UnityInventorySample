using UnityEngine;
using UnityEngine.EventSystems;

public class SlotBase : MonoBehaviour, IPointerClickHandler
{
    public ItemBase item;

    public bool isAvailable => item == null;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (item == null)
            return;

        if(item.TryGetComponent(out IClickableItem clickable))
        {
            InventoryBase currentInv = GetComponentInParent<InventoryBase>();

            clickable.OnClicked(currentInv);
        }
    }

    public void AddItem(ItemBase i)
    {
        if(item == null && i != null)
        {
            item = i;
            i.transform.SetParent(transform);
            i.transform.localPosition = Vector3.zero;
        }
    }

    public void RemoveItem(bool force = false)
    {
        if(item != null)
        {
            if (item.itemID < 0 && !force)
                return;

            if (item.TryGetComponent(out ItemConnectionHolder holder))
                holder.ClearConnectedItems();
            Destroy(item.gameObject);
            item = null;
        }
    }
}
