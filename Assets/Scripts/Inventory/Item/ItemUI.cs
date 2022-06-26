using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Text itemName;

    public void Initialize()
    {
        if(TryGetComponent(out ItemBase item))
        {
            itemName.text = item.itemName;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError("There is no item on this (Click to go)", transform);
#endif
        }
    }
}
