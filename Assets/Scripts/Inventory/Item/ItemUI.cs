using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Text itemName;

    public void Initialize(string itemN)
    {
        itemName.text = itemN;
    }
}
