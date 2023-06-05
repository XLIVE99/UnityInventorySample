public class ItemBasic : ItemBase, IClickableItem
{
    public void OnClicked(InventoryBase currentInv)
    {
        MainCanvas.instance.interactionPanel.FocusPanel(this, currentInv);
    }
}
