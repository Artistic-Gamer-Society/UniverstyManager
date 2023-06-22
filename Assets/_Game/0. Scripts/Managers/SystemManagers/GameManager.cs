using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mostly I use It To Control Game States Related Thing But
/// In This I Did'nt Use It Conventionally Because There Is No GameOver
/// And LevelFail Sort Of Things
/// </summary>
public class GameManager : MonoBehaviour
{
    public UnityEvent OnFirstTimeGameStart;
    
    private const string FirstTimeKey = "FirstTime";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(FirstTimeKey))
        {
            PerformFirstTimeAction();

            PlayerPrefs.SetInt(FirstTimeKey, 1);
            PlayerPrefs.Save();
        }
    }
    private void PerformFirstTimeAction()
    {
        OnFirstTimeGameStart?.Invoke();
    }
}
public enum UniversityPhase
{
    Enrollment,
    Examination,
    Ceremony
}
public static class Actions
{
    /// <summary>
    /// - Invokes From OnMouseDown At Student Script
    /// - Prepare a new selected student in Selection Manager
    /// </summary>
    public static Action<Student> OnStudentSelection;
    public static Action<Student,int> OnStudentCeremony;

    public static Func<Student, Student> GetStudentAtTable;
}


