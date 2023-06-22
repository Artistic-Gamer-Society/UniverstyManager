using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnlockableItemData", menuName = "ScriptableObjects/UnlockableItemData")]
public class UnlockableItemData : ScriptableObject
{
    public List<UnlockableItem> unlockableItems = new List<UnlockableItem>();
    public List<GameObject> nextUnlockableItemParents = new List<GameObject>();
}
