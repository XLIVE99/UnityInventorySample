using UnityEngine;

public class ItemEquippable : ItemBase, IEquippable, IClickableItem
{
    public struct RuleWrapper
    {
        public EquipRules[] rules;
    }

    public EquipRules[] rules;

    public override void Initialize(ItemDatabase.ItemInfo info)
    {
        base.Initialize(info);

        RuleWrapper ruleW = JsonUtility.FromJson<RuleWrapper>(info.itemAdds);
        rules = ruleW.rules;
    }

    public void OnClicked(InventoryBase currentInv)
    {
        ItemInteractionPanel.instance.FocusPanel(this, currentInv);

        bool canEquip = CheckEquip(currentInv.interactingInv, out EquipRules _);
        ItemInteractionPanel.instance.AddButton("Equip", OnEquipPressed, canEquip);
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

        SlotPlayer[] playerSlots = checkInv.GetAllSlots<SlotPlayer>();

        //Sort according to enum
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
            if (slot.isAvailable)
                availableBits |= 1 << (int)slot.slotType;
        }

        foreach (EquipRules rule in rules)
        {
            int itemMask = 0;

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
                while(itemMask != 0)
                {
                    if((itemMask & 1) == 1 && !playerSlots[slotIndex].isAvailable)
                    {
                        //Check if item on slot is allowed
                        if (!ItemGroupManager.instance.IsItemAllowed(itemGroupID, playerSlots[slotIndex].item.itemID))
                        {
#if UNITY_EDITOR
                            Debug.LogWarning("Item is not allowed");
#endif
                            conflict = true;
                            break;
                        }
                    }
                    itemMask >>= 1;
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
                Debug.LogWarning("There is not enough space for " + itemName);
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

        ItemBase item = ItemDatabase.instance.GetItem(itemID);

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
                connection.Initialize(new ItemDatabase.ItemInfo());

                connectionItem.GetComponent<ItemUI>().Initialize();

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
    #endregion
}
