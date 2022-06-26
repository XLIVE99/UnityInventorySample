using UnityEngine.EventSystems;

public class SlotPlayer : SlotBase
{
    public enum SlotType { LeftHand, RightHand} //Can be replaced with slot id, but enum has more readability
    public SlotType slotType;

    public override void OnPointerClick(PointerEventData eventData)
    {
        ItemInteractionPanel.instance.ClosePanel();

        RemoveItem();
    }
}
