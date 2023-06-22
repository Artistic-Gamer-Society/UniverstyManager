using DG.Tweening;
using System;
using UnityEngine;

public abstract class Table : MonoBehaviour
{
    public BoxCollider boxCollider;

    [SerializeField] protected Transform studentStandingPoint;
    [SerializeField] protected RadialProgressBar progressBar;

    public UniversityPhase tableType;

    private Student _currentStudent;
    private string studentKey;
    public Student currentStudent
    {
        get { return _currentStudent; }
        set { _currentStudent = value; }
    }
    public static event Action<Student, Vector3> OnSelectingDesk;
    public static event Action<Student> OnSelectingDesk1;
    protected virtual void OnSelectTable()
    {
        currentStudent = SelectionManager.selectedStudent; // Pick Student Ref
        if (currentStudent == null) //Is There any selected Student
            return;

        OnSelectingDesk?.Invoke(currentStudent, transform.position);
        OnSelectingDesk1?.Invoke(currentStudent);
        currentStudent.table = this;

        boxCollider.enabled = false;

        progressBar.student = currentStudent; // It will be use to make student ready for next phase, whenever progress will be completed.         
        currentStudent.ResetSelectionAnimation();

        var _scale = transform.localScale;
        transform.localScale *= 1.1f;
        transform.DOScale(_scale, 0.2f).SetEase(Ease.InOutBack);

        SelectionManager.selectedStudent = null;
    }
    protected virtual void StartProcess(Student student)
    {
        if (currentStudent != null)
        {
            if (student == currentStudent)
            {
                student.movement.navMeshAgent.enabled = false;
                progressBar.gameObject.SetActive(true);
                student.transform.parent = studentStandingPoint;
                student.transform.DOLocalMove(Vector3.zero, 0.5f);
                student.transform.DOLocalRotate(Vector3.zero, 0.2f);
                student.animator.ResetPosition();


                currentStudent = null;

            }
        }
    }


    //==========Data Save===================
    private void AssignStudentToTable()
    {
        Student student = SelectionManager.selectedStudent;
        if (student != null)
        {
            studentKey = Guid.NewGuid().ToString(); // Generate a unique key for the student
            SaveStudentKey(student, studentKey);
        }
    }
    private void SaveStudentKey(Student student, string key)
    {
        // Save the assigned key for the student
        PlayerPrefs.SetString(key, student.gameObject.name);
        PlayerPrefs.Save();
    }
    private Student FindStudentByKey(string key)
    {
        // Find the student using the key
        string studentName = PlayerPrefs.GetString(key);
        GameObject studentObject = GameObject.Find(studentName);
        if (studentObject != null)
        {
            return studentObject.GetComponent<Student>();
        }
        return null;
    }
}
