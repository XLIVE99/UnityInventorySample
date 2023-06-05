using UnityEngine;

public class ItemEquippable : ItemBase, IEquippable, IClickableItem
{
    ItemEquippableScriptable EquippableInfo => itemInfo as ItemEquippableScriptable;

    public void OnClicked(InventoryBase currentInv)
    {
        bool canEquip = CheckEquip(currentInv.interactingInv, out EquipRules _);
        MainCanvas.instance.interactionPanel.AddButton("Equip", OnEquipPressed, canEquip);

        if (!string.IsNullOrEmpty(EquippableInfo.InspectText))
            MainCanvas.instance.interactionPanel.AddButton("Inspect", OnInspectPressed);

        MainCanvas.instance.interactionPanel.FocusPanel(this, currentInv);
    }

    public bool CheckEquip(InventoryBase checkInv, out EquipRules selectedRule)
    {
        selectedRule = new EquipRules();
        if(checkInv == null)
        {
#if UNITY_EDITOR
            Debug.LogError("Please make sure to assign interacting inventory!");
#endif
            return false;
        }

        if (!checkInv.IsSlotType<SlotPlayer>())
        {
#if UNITY_EDITOR
            Debug.LogError("Please make sure you are interacting with player inventory!");
#endif
            return false;
        }

        ItemEquippableScriptable EInfo = EquippableInfo; //No need to cast multiple times
        SlotPlayer[] playerSlots = checkInv.GetAllSlots<SlotPlayer>();

        //Sort according to enum, it will be easier to place item
        for(int i = 0; i < playerSlots.Length; i++)
        {
            if(i != (int)playerSlots[i].slotType)
            {
                int changePos = (int)playerSlots[i].slotType;
                SlotPlayer temp = playerSlots[i];
                playerSlots[i] = playerSlots[changePos];
                playerSlots[changePos] = temp;
            }
        }

        int availableBits = 0;
        foreach (SlotPlayer slot in playerSlots)
        {
            if (!slot.hasItem)
                availableBits |= 1 << (int)slot.slotType;
        }

        foreach (EquipRules rule in EInfo.EquipRules)
        {
            int itemMask = 0; //Item coverage slots, e.g. 00011 => The item requires two slots to check before equip

            //Check other slots
            foreach(SlotPlayer.SlotType t in rule.slotChecks)
            {
                itemMask |= 1 << (int)t;
            }

            int lastCheck = availableBits & itemMask;
            lastCheck ^= itemMask;

            if (lastCheck != 0) //There is an item in check slot
            {
                int slotIndex = 0;
                bool conflict = false;
                while(lastCheck != 0) //itemMask changed with lastCheck start
                {
                    if((lastCheck & 1) == 1 && playerSlots[slotIndex].hasItem)
                    {
                        //Check if the item on slot is allowed
                        if (playerSlots[slotIndex].item.itemInfo == null)
                        {
#if UNITY_EDITOR
                            Debug.LogWarning("Item is not allowed");
#endif
                            conflict = true;
                            break;
                        }
                        else if (!playerSlots[slotIndex].item.itemInfo.AllowedGroup.HasFlag(itemInfo.ItemGroup))
                        {
#if UNITY_EDITOR
                            Debug.LogWarning("Item is not allowed");
#endif
                            conflict = true;
                            break;
                        }
                    }
                    lastCheck >>= 1; //itemMask change end
                    slotIndex++;
                }

                if (conflict)
                    continue;
            }

            //Check if slot is empty
            itemMask = 0;
            foreach (SlotPlayer.SlotType t in rule.slotCoverage)
            {
                itemMask |= 1 << (int)t;
            }

            lastCheck = availableBits & itemMask;
            lastCheck ^= itemMask;

            if (lastCheck == 0)
            {
                selectedRule = rule;
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("There is not enough space for " + itemInfo.ItemName);
#endif
            }
        }

        return false;
    }

    public void Equip(InventoryBase interactInv, EquipRules selectedRule)
    {
        SlotPlayer[] playerSlots = interactInv.GetAllSlots<SlotPlayer>();

        //Sort according to enum
        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (i != (int)playerSlots[i].slotType)
            {
                int changePos = (int)playerSlots[i].slotType;
                SlotPlayer temp = playerSlots[i];
                playerSlots[i] = playerSlots[changePos];
                playerSlots[changePos] = temp;
            }
        }

        //Since we are cloning item to player slot, we need to clone same item
        ItemBase item = ItemDatabase.instance.CreateItem(itemInfo);

        if(selectedRule.slotCoverage.Length > 1)
        {
            ItemConnectionHolder holder = item.gameObject.AddComponent<ItemConnectionHolder>();
            holder.connections = new ItemConnection[selectedRule.slotCoverage.Length - 1];
            for(int i = 1; i < selectedRule.slotCoverage.Length; i++)
            {
                GameObject connectionItem = ItemDatabase.instance.GetEmptyItem();
                ItemConnection connection = connectionItem.AddComponent<ItemConnection>();
                connection.slot = playerSlots[(int)selectedRule.slotCoverage[i]];
                connection.slot.AddItem(connectionItem.GetComponent<ItemBase>());

                connectionItem.GetComponent<ItemUI>().Initialize("BLOCKED");

                holder.connections[i - 1] = connection;
            }
        }
        playerSlots[(int)selectedRule.slotCoverage[0]].AddItem(item);
    }

    #region BUTTON METHODS
    private void OnEquipPressed(InventoryBase currentInv)
    {
        if (CheckEquip(currentInv.interactingInv, out EquipRules selectedR))
        {
            Equip(currentInv.interactingInv, selectedR);
        }
    }

    private void OnInspectPressed(InventoryBase _)
    {
        MainCanvas.instance.inspectionPanel.Inspect(EquippableInfo.InspectText);
    }
    #endregion
}
