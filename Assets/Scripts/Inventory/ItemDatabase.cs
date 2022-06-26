using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public enum ItemType { Basic, Equippable}

    [System.Serializable]
    public struct ItemInfo
    {
        public string itemName;
        public ItemType itemType;
        public int itemID;
        public int itemGroupID;
        public string itemAdds; //Additionals
    }

    public struct ItemWrapper
    {
        public List<ItemInfo> items;
    }

    public List<ItemInfo> items = new List<ItemInfo>();

    public static ItemDatabase instance;

    private GameObject itemPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        itemPrefab = Resources.Load<GameObject>("Items/UI/Item");

        string itemJson = Resources.Load<TextAsset>("Items/Items").text;
        itemJson = Regex.Replace(itemJson, @"\s(?=(?:""[^ ""]*"" |[^ ""])*$)|\r?\n\s+", "");

        //Debug.Log("json: " + itemJson);

        ItemWrapper wrapper = JsonUtility.FromJson<ItemWrapper>(itemJson);
        items.AddRange(wrapper.items);
    }

    public GameObject GetEmptyItem()
    {
        GameObject createdItem = Instantiate(itemPrefab);
        return createdItem;
    }

    public ItemBase GetItem(int itemID)
    {
        foreach(ItemInfo item in items)
        {
            if(item.itemID == itemID)
            {
                GameObject createdItem = Instantiate(itemPrefab);
                AddItemToGameObject(createdItem, item);
                createdItem.GetComponent<ItemUI>().Initialize();
                return createdItem.GetComponent<ItemBase>();
            }
        }

        return null;
    }

    private void AddItemToGameObject(GameObject g, ItemInfo item)
    {
        switch (item.itemType)
        {
            case ItemType.Basic:
                g.AddComponent<ItemBasic>().Initialize(item);
                break;
            case ItemType.Equippable:
                g.AddComponent<ItemEquippable>().Initialize(item);
                break;
            default:
                break;
        }
    }
}
