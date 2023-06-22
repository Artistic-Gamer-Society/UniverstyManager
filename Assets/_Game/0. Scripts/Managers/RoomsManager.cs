using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - Add And Remove Student From One List To Another
/// - It Checks Current Line And Respectively Move To The Next
/// - For Future Updates:
/// I can create a Array Of Lines. Because Currently Lines Are Limited And We Are Using If 
/// But In Array We can Simple Use Next Element By Increamenting Index
/// </summary>
[DefaultExecutionOrder(10)]
public class RoomsManager : MonoBehaviour
{
    public StudentLineManager enrollmentLineManager;
    public StudentLineManager examinationLineManager;
    public StudentLineManager ceremonyLineManager;

    public Spawner spawner;

    private Dictionary<Student, StudentLineManager> studentLineDictionary = new Dictionary<Student, StudentLineManager>();

    [SerializeField] GameConfig gameConfig;
    private void Start()
    {
        foreach (var st in enrollmentLineManager.students)
        {
            studentLineDictionary.Add(st, enrollmentLineManager);
        }
        gameConfig.numOfStudentsInRooms = studentLineDictionary.Count;
    }
    private void OnEnable()
    {
        DestinationManager.OnReachingNextPhase += RemoveStudent;
        Spawner.OnStudentSpawn += AddStudent;
        Student.OnStudentStart += SetStudentCurrentLineManager;
    }
    private void OnDisable()
    {
        DestinationManager.OnReachingNextPhase -= RemoveStudent;
        Spawner.OnStudentSpawn -= AddStudent;
        Student.OnStudentStart -= SetStudentCurrentLineManager;
    }
    public void AddStudent(Student student, StudentLineManager lineManager)
    {
        lineManager.AddStudent(student);
        studentLineDictionary.Add(student, lineManager);
        gameConfig.numOfStudentsInRooms = studentLineDictionary.Count;
    }
    public void RemoveStudent(Student student)
    {
        if (studentLineDictionary.ContainsKey(student))
        {
            StudentLineManager currentLineManager = studentLineDictionary[student];
            currentLineManager.RemoveStudent(student, Vector3.zero);

            // Determine the new line manager based on the current line
            StudentLineManager newLineManager = GetNewLineManager(currentLineManager);

            if (newLineManager != null)
            {
                // Move the student to the new line manager
                newLineManager.AddStudent(student);
                studentLineDictionary[student] = newLineManager;
            }
            else
            {
                // If there is no new line manager, remove the student from the dictionary
                // 
                spawner.studentPool.Add(student);
                student.gameObject.SetActive(false);

                studentLineDictionary.Remove(student);
            }
        }
    }
    private StudentLineManager GetNewLineManager(StudentLineManager currentLineManager)
    {
        if (currentLineManager == enrollmentLineManager)
        {
            return examinationLineManager;
        }
        else if (currentLineManager == examinationLineManager)
        {
            return ceremonyLineManager;
        }
        else
        {
            return null; // No new line manager
        }
    }
    private void SetStudentCurrentLineManager(Student student)
    {
        switch (student.phase)
        {
            case UniversityPhase.Enrollment:
                student.studentCurrentLine = enrollmentLineManager;
                break;
            case UniversityPhase.Examination:
                student.studentCurrentLine = examinationLineManager;
                break;
            case UniversityPhase.Ceremony:
                student.studentCurrentLine = ceremonyLineManager;
                break;
        }
    }
}