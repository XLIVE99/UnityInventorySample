public interface IEquippable
{
    bool CheckEquip(InventoryBase checkInv, out EquipRules selectedRule);
    void Equip(InventoryBase interactInv, EquipRules selectedSlot);
}
