using UnityEngine;

public class ItemGroupManager : MonoBehaviour
{
    [System.Serializable]
    public class Group
    {
#if UNITY_EDITOR
        [Tooltip("Editor-only")] public string groupName;
#endif
        //public int groupID; //Switched to array index

        public int[] disallowedItems;

        public bool IsItemDisallowed(int itemID)
        {
            foreach(int item in disallowedItems)
            {
                if (itemID == item)
                    return true;
            }

            return false;
        }
    }

    public Group[] groups;

    #region SINGLETON
    public static ItemGroupManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    public bool IsItemAllowed(int currentGroup, int checkItem)
    {
        if(currentGroup < groups.Length)
        {
            return !groups[currentGroup].IsItemDisallowed(checkItem);
        }

        return true;
    }
}
