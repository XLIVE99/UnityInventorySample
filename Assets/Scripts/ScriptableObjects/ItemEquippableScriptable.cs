using UnityEngine;

[CreateAssetMenu(fileName = "Item Equippable", menuName = "Scriptables/Item Equippable")]
public class ItemEquippableScriptable : ItemBaseScriptable
{
    [SerializeField] private EquipRules[] equipRules;
    public EquipRules[] EquipRules => equipRules;

    [SerializeField, TextArea] private string inspectText;
    public string InspectText => inspectText;

    [SerializeField, TextArea] private string additionals; //Additionals such as console commands
    public string Additionals => additionals;

}
