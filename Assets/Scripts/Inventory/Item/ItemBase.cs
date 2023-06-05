using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [HideInInspector] public ItemBaseScriptable itemInfo;

    public virtual void Initialize(ItemBaseScriptable info)
    {
        itemInfo = info;
    }
}
