using UnityEngine;

public class ItemInspectable : ItemBase, IClickableItem
{
    public void OnClicked(InventoryBase currentInv)
    {
        ItemInspectableScriptable i = itemInfo as ItemInspectableScriptable;
        if(i != null)
        {
            MainCanvas.instance.interactionPanel.AddButton("Inspect", OnInspect);

            MainCanvas.instance.interactionPanel.FocusPanel(this, currentInv);
        }
    }

    #region BUTTON_METHODS
    private void OnInspect(InventoryBase _)
    {
        ItemInspectableScriptable i = itemInfo as ItemInspectableScriptable;
        MainCanvas.instance.inspectionPanel.Inspect(i.InspectText);
    }
    #endregion
}
