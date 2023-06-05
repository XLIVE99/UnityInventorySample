using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public ItemBaseScriptable[] items;

    [System.Serializable]
    public struct ItemInfo
    {
        public string itemName;
        public ItemBaseScriptable.EItemType itemType;
        public int itemID;
        public int itemGroupID;
        public string itemAdds; //Additionals
    }

    public struct ItemWrapper
    {
        public List<ItemInfo> items;
    }

    //public List<ItemInfo> items = new List<ItemInfo>();

    public static ItemDatabase instance;

    private GameObject itemPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        itemPrefab = Resources.Load<GameObject>("Items/UI/Item");
    }

    public GameObject GetEmptyItem()
    {
        GameObject createdItem = Instantiate(itemPrefab);
        return createdItem;
    }

    public ItemBase CreateItem(ItemBaseScriptable i)
    {
        GameObject createdItem = Instantiate(i.ItemPrefab);
        createdItem.GetComponent<ItemUI>().Initialize(i.ItemName);
        createdItem.GetComponent<ItemBase>().Initialize(i);
        return createdItem.GetComponent<ItemBase>();
    }
}
