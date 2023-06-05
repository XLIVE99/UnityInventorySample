using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemInteractionPanel : MonoBehaviour
{
    [SerializeField] private Transform buttonsParent;

    private ItemBase currentItem;
    private int buttonIndex = 0;

    private InventoryBase currentInv;

    public void FocusPanel(ItemBase item, InventoryBase curInv)
    {
        if ((currentItem != null && currentItem != item) || (currentInv != null && curInv != currentInv))
            ClosePanel();
        else if (currentItem == item && currentInv == curInv) //Double clicked same item
        {
            ClosePanel();
            return;
        }

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        currentItem = item;
        currentInv = curInv;

        transform.position = currentItem.transform.position;
    }

    public void AddButton(string buttonName, UnityAction<InventoryBase> onClick, bool interactable = true)
    {
        if(buttonIndex >= buttonsParent.childCount)
        {
            //All buttons in use, create new buttons
            Instantiate(buttonsParent.GetChild(0).gameObject, buttonsParent);
        }

        Transform selectedButton = buttonsParent.GetChild(buttonIndex);
        selectedButton.gameObject.SetActive(true);

        selectedButton.GetComponentInChildren<Text>().text = buttonName;

        Button b = selectedButton.GetComponent<Button>();
        b.onClick.AddListener(() => onClick.Invoke(currentInv));
        b.onClick.AddListener(ClosePanel);
        b.interactable = interactable;

        buttonIndex++;
    }

    public void ClosePanel()
    {
        foreach(Transform button in buttonsParent)
        {
            if(button.gameObject.activeSelf)
            {
                button.GetComponent<Button>().onClick.RemoveAllListeners();
                button.gameObject.SetActive(false);
            }
        }

        if (gameObject.activeSelf)
            gameObject.SetActive(false);

        buttonIndex = 0;
        currentItem = null;
    }
}
