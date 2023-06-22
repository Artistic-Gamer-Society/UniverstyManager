using System;
using UnityEngine;

/// <summary>
/// - It Is Specific That We Don't Need To Use 3 Fingers At A Time
/// - In short We Have One Thing Selected At A Time
/// - I didn't To Track The Table Because That Is Independant From Core Game Loop
/// - I Track Selected Student And Reset Previously Selected Student
/// - And For Student It Still Only Track The Student Which Are Not Ready
/// To Change The Phase
/// </summary>
public class SelectionManager : MonoBehaviour
{
    internal static Student selectedStudent;

    #region Unity Callbacks
    private void OnEnable()
    {
        Actions.OnStudentSelection += SetSelectedStudent;

        UIManager.GetInstance().examinationRight.btn.onClick.AddListener(ResetSelectionOnBtn);
        UIManager.GetInstance().examinationLeft.btn.onClick.AddListener(ResetSelectionOnBtn);
        UIManager.GetInstance().enrollment.btn.onClick.AddListener(ResetSelectionOnBtn);
        UIManager.GetInstance().ceremony.btn.onClick.AddListener(ResetSelectionOnBtn);

    }
    private void OnDisable()
    {
        Actions.OnStudentSelection -= SetSelectedStudent;

        UIManager.GetInstance().examinationRight.btn.onClick.RemoveListener(ResetSelectionOnBtn);
        UIManager.GetInstance().examinationLeft.btn.onClick.RemoveListener(ResetSelectionOnBtn);
        UIManager.GetInstance().enrollment.btn.onClick.RemoveListener(ResetSelectionOnBtn);
        UIManager.GetInstance().ceremony.btn.onClick.RemoveListener(ResetSelectionOnBtn);
    }
    #endregion
    private void SetSelectedStudent(Student obj)
    {
        if (obj.isReadyToChangePhase)
            return;
        var previous = selectedStudent;
        if (previous != null)
        {
            StudentData.SetOutline(0, previous);
            previous.ResetStudentDefaultState();
        }
        selectedStudent = obj;
        selectedStudent.movement.enabled = true;
        selectedStudent.movement.navMeshAgent.enabled = true;
        StudentData.SetOutline(60, selectedStudent);
    }
    private void ResetSelectionOnBtn()
    {
        if (selectedStudent != null)
        {
            selectedStudent.ResetStudentDefaultState();
            StudentData.SetOutline(0, selectedStudent);
            selectedStudent = null;
        }
    }
}
#region Commented
/*
 *     private Vector3 refVel = Vector3.zero;
 * //private void Update()
//{
//    if (GameManager.Instance.isPlay && selectedStudent != null)
//    {
//        if (Input.GetMouseButton(0))
//        {
//            if (GameManager.Instance.isPlay == false)
//            {
//                return;
//            }
//            var currentPos = selectedStudent.transform.position;
//            MoveObjectToMousePosition(currentPos, refVel, gameConfig.movementSmoothing);
//        }
//        else if (Input.GetMouseButtonUp(0) && selectedStudent != null)
//        {
//            selectedStudent.ResetStudentDefaultState();
//            selectedStudent = null;
//        }
//    }
//}
    /// <summary>
    /// Returns Current Mouse Position, Screen to world point.
    /// </summary>
    /// <param name="gridEnabled"></param>
    /// <returns></returns>    
    Vector3 GetTargetPos()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = gameConfig.selectedStudentHeight - Camera.main.transform.position.z;

        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
    bool IsUnderBoundary(Vector3 pos)
    {
        if (pos.x < gameConfig.leftBoundary || (pos.x > gameConfig.rightBoundary)
            || pos.y < gameConfig.bottomBounday || (pos.y > gameConfig.topBoundary))
        {
            return false;
        }
        else return true;
    }
    void MoveObjectToMousePosition(Vector3 currentPos, Vector3 refVel, float smoothTime)
    {
        Vector3 targetPos = GetTargetPos();
        currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref refVel, smoothTime);
        GameManager.selectedStudent.transform.position = currentPos;
    }
 */
#endregion