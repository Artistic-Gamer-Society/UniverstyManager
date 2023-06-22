using UnityEngine;
using System.Collections.Generic;

public class UnlockableItemParent : MonoBehaviour
{
    [SerializeField]
    private List<UnlockableItem> unlockableItems = new List<UnlockableItem>();

    public void GetUnlockItemsAndAssignKeys()
    {
        unlockableItems.Clear();
        CollectUnlockableItems();
        AssignNextUnlockableItemParents();
        AssignItemKeys();
    }
    private void CollectUnlockableItems()
    {
        unlockableItems.AddRange(GetComponentsInChildren<UnlockableItem>(true));
    }

    private void AssignItemKeys()
    {
        for (int i = 0; i < unlockableItems.Count; i++)
        {
            unlockableItems[i].itemKey = gameObject.name + "_" + i.ToString();
        }
    }
    private void AssignNextUnlockableItemParents()
    {
        for (int i = 0; i < unlockableItems.Count; i++)
        {
            UnlockableItem currentUnlockableItem = unlockableItems[i];

            if (i < unlockableItems.Count - 1)
            {
                UnlockableItem nextUnlockableItem = unlockableItems[i + 1];
                currentUnlockableItem.nextToUnlock = nextUnlockableItem.transform.parent.gameObject;
            }
            currentUnlockableItem.name = "Case Unlock" + "_" + i.ToString();
        }
    }
}
