using UnityEngine;

public class EnrollmentTable : Table
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
