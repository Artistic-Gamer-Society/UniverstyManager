using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// The Main Role Is To Track The Destination and
/// Play Related Events Like OnReachingDestination
/// </summary>
[DefaultExecutionOrder(9)]
public class DestinationManager : MonoBehaviour
{
    public List<Student> agents; // List of agents to track

    [SerializeField] Transform enrollmentDoor;
    [SerializeField] Transform examinationDoor;
    [SerializeField] Transform ceremonyDoor;

    Transform nextRoomDoor;

    /// <summary>
    /// if(remainingDistance<=stoppingDistance)
    /// </summary>
    public static event Action<Student> OnReachingDestination;
    /// <summary>
    /// - When Reached To The NextDoor
    /// - I Use It To Put Students From OneLine To Another 
    /// - By RoomsManager
    /// </summary>
    public static event Action<Student> OnReachingNextPhase;

    #region Unity Callbacks
    private void Start()
    {
        // Initialize the list of agents (you can populate it through code or the Inspector)
        agents = new List<Student>();        
    }
    private void OnEnable()
    {
        Actions.OnStudentSelection += AddAgent;

        UIManager.GetInstance().examinationRight.btn.onClick.AddListener(InstantMove);
        UIManager.GetInstance().examinationLeft.btn.onClick.AddListener(InstantMove);
        UIManager.GetInstance().enrollment.btn.onClick.AddListener(InstantMove);
        UIManager.GetInstance().ceremony.btn.onClick.AddListener(InstantMove);
    }
    private void OnDisable()
    {
        Actions.OnStudentSelection -= AddAgent;

        UIManager.GetInstance().examinationRight.btn.onClick.RemoveListener(InstantMove);
        UIManager.GetInstance().examinationLeft.btn.onClick.RemoveListener(InstantMove);
        UIManager.GetInstance().enrollment.btn.onClick.RemoveListener(InstantMove);
        UIManager.GetInstance().ceremony.btn.onClick.RemoveListener(InstantMove);
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            Student student = agents[i];
            var agent = student.movement.navMeshAgent;

            if (agent.isActiveAndEnabled)
            {
                if (agent.hasPath)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        // The agent has reached its destination or is very close to it
                        OnReachingDestination?.Invoke(student);
                        RemoveAgentWithoutEvent(student);
                        if (agent.destination.x == GetNextRoomDoorDestination(student.phase).position.x)
                        {
                            OnReachingNextPhase?.Invoke(student);    
                        }
                        agent.enabled = false;
                        student.transform.rotation = Quaternion.identity;
                    }
                }
            }
        }
    }
    #endregion
    public Transform GetNextRoomDoorDestination(UniversityPhase phase)
    {
        if (phase == UniversityPhase.Enrollment)
        {
            nextRoomDoor = enrollmentDoor;
        }
        else if (phase == UniversityPhase.Examination)
        {
            nextRoomDoor = examinationDoor;
        }
        else if (phase == UniversityPhase.Ceremony)
        {
            nextRoomDoor = ceremonyDoor;
        }
        return nextRoomDoor;
    }
    public void AddAgent(Student student)
    {
        student.doorPos = GetNextRoomDoorDestination(student.phase).position;
        agents.Add(student);
    }
    public void SetRoomDoor(Transform door)
    {
        examinationDoor = door;
    }
    public void RemoveAgentWithoutEvent(Student student)
    {
        agents.Remove(student);
    }

    private void InstantMove()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            Student student = agents[i];
            var agent = student.movement.navMeshAgent;

            if (agent.isActiveAndEnabled)
            {
                if (agent.hasPath)
                {
                    student.InstantMoveOnSwitchingRoom();
                }
            }
        }
    }

}
