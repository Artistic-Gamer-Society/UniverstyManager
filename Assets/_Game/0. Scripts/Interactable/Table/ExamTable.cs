using UnityEngine;

public class ExamTable : Table
{
    private void OnMouseDown()
    {
        base.OnSelectTable();
    }
    private void OnEnable()
    {
        StudentMovement.OnReachingDesk += base.StartProcess;
    }
    private void OnDisable()
    {
        StudentMovement.OnReachingDesk -= base.StartProcess;
    }
 
}
