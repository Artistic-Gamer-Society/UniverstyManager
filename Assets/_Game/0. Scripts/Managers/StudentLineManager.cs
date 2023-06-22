using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(9)]
public class StudentLineManager : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] float spacing;
    [SerializeField] float rearrangeDuration = 1.5f;
    [SerializeField] Axis axis;

    public UniversityPhase linePhase;
    public List<Student> students = new List<Student>();

    private Vector3 startPos;
    /// <summary>
    /// - Start Walk
    /// </summary>
    public static event Action<Student> OnStartRearrangeing;
    public enum Axis
    {
        X,
        Y,
        Z
    }
    #region Unity CallBacks
    private void Start()
    {
        startPos = startPoint.localPosition;
    }
    private void OnEnable()
    {
        RearrangeStudents(0);
        Table.OnSelectingDesk += RemoveStudent;
    }
    private void OnDisable()
    {
        Table.OnSelectingDesk -= RemoveStudent;
    }
    #endregion
    public void AddStudent(Student student)
    {
        students.Add(student);
        student.transform.SetParent(transform); // Set student object as a child of the StudentLineManager

        int studentIndex = students.Count - 1; // Index of the added student in the students list

        // Update the IndexInLine property of the added student
        student.IndexInLine = studentIndex;

        RearrangeStudents(studentIndex);
    }
    public void RemoveStudent(Student student, Vector3 tablePos)
    {
        int studentIndex = students.IndexOf(student); // Get the index of the student in the students list

        if (studentIndex >= 0)
        {
            students.Remove(student);
            student.transform.SetParent(null); // Remove student object from the StudentLineManager's children
            student.IndexInLine = -1;            
            student.StopRearranging();

            // Rearrange the remaining students
            DOVirtual.DelayedCall(0.5f, () =>
            {
                RearrangeStudents(studentIndex);
            });

            // Update the indices of the remaining students after the removal
            for (int i = studentIndex; i < students.Count; i++)
            {
                students[i].IndexInLine = i; // Update the IndexInLine property of each student
            }
        }
        else
        {
            Debug.LogWarning("Student not found in the students list: " + student.name);
        }
    }
    private void RearrangeStudents(int startIndex)
    {
        for (int i = startIndex; i < students.Count; i++)
        {
            Vector3 targetPosition = GetTargetStudentLocalPosition(i);
            if (students[i].isReadyToChangePhase == false)
            {
                OnStartRearrangeing?.Invoke(students[i]);
                students[i].StartRearranging(targetPosition, rearrangeDuration);
                students[i].startPos = targetPosition;
            }
            else
            {
                var pos = students[i].transform.localPosition;
                Vector3[] positions = new Vector3[]
                {
                new Vector3(pos.x, -5, -15),
                new Vector3(targetPosition.x, targetPosition.y, -10),             
                };
                students[i].SwitchLineAnimation(positions, 0, students[i],targetPosition);

                students[i].startPos = targetPosition;
            }
        }
    }
    private Vector3 GetTargetStudentLocalPosition(int index)
    {
        float offset = index * spacing;
        Vector3 localPosition = startPos;

        switch (axis)
        {
            case Axis.X:
                localPosition.x += offset;
                break;
            case Axis.Y:
                localPosition.y += offset;
                break;
            case Axis.Z:
                localPosition.z += offset;
                break;
        }

        return localPosition;
    }
}
