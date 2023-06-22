using UnityEngine;

public class StudentAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    #region Unity Callbacks
    private void OnEnable()
    {
        Table.OnSelectingDesk += Walk;
        StudentMovement.OnReachingDesk += StopWalk;
        StudentLineManager.OnStartRearrangeing += Walk;
    }
    private void OnDisable()
    {
        Table.OnSelectingDesk -= Walk;
        StudentMovement.OnReachingDesk -= StopWalk;
        StudentLineManager.OnStartRearrangeing -= Walk;
    }
    #endregion

    public void Walk()
    {
        // Play walk animation
        animator.SetBool("isRunning", true);
    }
    private void Walk(Student student)
    {
        if (student.animator == this)
            Walk();
    }
    private void Walk(Student student, Vector3 arg2)
    {
        if (student.animator == this)
            Walk();
    }

    public void StopWalking()
    {
        // Stop walk animation
        animator.SetBool("isRunning", false);
    }
    private void StopWalk(Student student)
    {
        if (student.animator == this)
            StopWalking();
    }
    public void ResetPosition()
    {
        transform.localPosition = Vector3.up * transform.localPosition.y;
    }
}