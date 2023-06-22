using System.Collections.Generic;
using UnityEngine;

public class StudentKeyParent : MonoBehaviour
{
    [SerializeField]
    private List<Student> students = new List<Student>();

    public void GetUnlockItemsAndAssignKeys()
    {
        students.Clear();
        CollectUnlockableItems();
        AssignItemKeys();
    }
    private void CollectUnlockableItems()
    {
        students.AddRange(GetComponentsInChildren<Student>(true));
    }

    private void AssignItemKeys()
    {
        for (int i = 0; i < students.Count; i++)
        {
            students[i].studentKey = gameObject.name + "_" + i.ToString();
        }
    }
}
