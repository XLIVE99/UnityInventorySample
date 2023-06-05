using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Base", menuName = "Scriptables/Item Base")]
public class ItemBaseScriptable : ScriptableObject
{
    public enum EItemType { Basic, Equippable }
    [System.Flags]
    public enum EItemGroup { None, Melee, Shield, Magic = 4}

    [SerializeField] protected string itemName;
    public string ItemName => itemName;

    [SerializeField] protected EItemGroup itemGroup;
    public EItemGroup ItemGroup => itemGroup;

    [SerializeField] protected EItemGroup allowedGroup;
    public EItemGroup AllowedGroup => allowedGroup;

    [SerializeField] protected GameObject itemPrefab;
    public GameObject ItemPrefab => itemPrefab;
}
