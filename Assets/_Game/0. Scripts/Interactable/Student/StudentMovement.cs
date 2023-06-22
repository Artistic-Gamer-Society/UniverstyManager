using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.AI;

public class StudentMovement : MonoBehaviour
{
    internal NavMeshAgent navMeshAgent;
    [SerializeField]
    private Transform target;

    public static Action<Student> OnReachingDesk;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        Table.OnSelectingDesk += MoveToDestination;
        DestinationManager.OnReachingDestination += OnReachingDestinationPoint;
    }
    private void OnDisable()
    {
        Table.OnSelectingDesk -= MoveToDestination;
        DestinationManager.OnReachingDestination -= OnReachingDestinationPoint;
    }

    public void MoveToDestination(Student student, Vector3 destination)
    {
        if (student.movement == this)
        {
            student.movement.navMeshAgent.SetDestination(destination);
            student.transform.LookAt(destination);
            student.enabled = true;
            student.movement.navMeshAgent.enabled = true;
        }
    }   
    public void OnReachingDestinationPoint(Student student)
    {
        if (student.movement == this)
        {
            if (!student.isReadyToChangePhase)
            {
                switch (student.phase)
                {
                    case UniversityPhase.Enrollment:
                        OnReachingDesk?.Invoke(student);
                        break;
                    case UniversityPhase.Examination:
                        OnReachingDesk?.Invoke(student);
                        break;
                    case UniversityPhase.Ceremony:
                        OnReachingDesk?.Invoke(student);
                        break;
                }
            }
        }
    }
}