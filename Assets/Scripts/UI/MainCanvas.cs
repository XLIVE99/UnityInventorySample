using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public ItemInteractionPanel interactionPanel;
    public InspectionPanel inspectionPanel;

    #region SINGLETON
    public static MainCanvas instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    #endregion
}
