using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StudentDataManager : MonoBehaviour
{
    private const string SaveFileName = "student_data.json";

    public static void SaveStudentData(Dictionary<Student, StudentLineManager> studentLineDictionary)
    {
        StudentDataContainer dataContainer = new StudentDataContainer();

        foreach (var kvp in studentLineDictionary)
        {
            Student student = kvp.Key;
            StudentState studentState = new StudentState();
            studentState.name = student.gameObject.name;
            studentState.phase = student.phase;
            studentState.isActive = student.isActive;
            dataContainer.students.Add(studentState);
        }

        string jsonData = JsonUtility.ToJson(dataContainer);
        File.WriteAllText(GetSaveFilePath(), jsonData);
    }

    public static void LoadStudentData(Dictionary<Student, StudentLineManager> studentLineDictionary)
    {
        string filePath = GetSaveFilePath();

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            StudentDataContainer dataContainer = JsonUtility.FromJson<StudentDataContainer>(jsonData);

            foreach (var kvp in studentLineDictionary)
            {
                Student student = kvp.Key;
                StudentState studentState = dataContainer.students.Find(x => x.name == student.gameObject.name);

                if (studentState != null)
                {
                    student.phase = studentState.phase;
                    student.isActive = studentState.isActive;
                    student.gameObject.SetActive(student.isActive);
                }
            }
        }
    }

    private static string GetSaveFilePath()
    {
        return Path.Combine(Application.persistentDataPath, SaveFileName);
    }
}

[Serializable]
public class StudentState
{
    public string name;
    public UniversityPhase phase;
    public bool isActive;
}

[Serializable]
public class StudentDataContainer
{
    public List<StudentState> students = new List<StudentState>();
}