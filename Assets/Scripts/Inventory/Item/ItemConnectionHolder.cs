using UnityEngine;

public class ItemConnectionHolder : MonoBehaviour
{
    public ItemConnection[] connections;

    public void ClearConnectedItems()
    {
        foreach(ItemConnection connection in connections)
        {
            connection.RemoveFromSlot();
        }
    }
}
