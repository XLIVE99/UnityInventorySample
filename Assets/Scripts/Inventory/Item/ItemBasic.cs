public class ItemBasic : ItemBase, IClickableItem
{
    public void OnClicked(InventoryBase currentInv)
    {
        ItemInteractionPanel.instance.FocusPanel(this, currentInv);
    }
}
