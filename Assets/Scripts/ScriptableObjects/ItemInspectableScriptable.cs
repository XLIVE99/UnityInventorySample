using UnityEngine;

[CreateAssetMenu(fileName = "Item Inspectable", menuName = "Scriptables/Item Inspectable")]
public class ItemInspectableScriptable : ItemBaseScriptable
{
    [SerializeField, TextArea] protected string inspectText;
    public string InspectText => inspectText;
}
