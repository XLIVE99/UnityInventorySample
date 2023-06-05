using UnityEngine;
using UnityEngine.UI;

public class InspectionPanel : MonoBehaviour
{
    [SerializeField] private Text inspectText;

    public void Inspect(string inspect)
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);

        inspectText.text = inspect;
    }

    public void ClosePanel()
    {
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
