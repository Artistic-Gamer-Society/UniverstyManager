using DG.Tweening;
using System;
using UnityEngine;
/// <summary>
/// - phase: In which Student is.
/// </summary>

public class Student : MonoBehaviour
{
    public UniversityPhase phase = UniversityPhase.Enrollment;
    public bool isActive = true;
    [SerializeField] CapsuleCollider capsuleCollider;

    public SkinnedMeshRenderer skinMeshRenderer;
    public StudentData data;

    public StudentAnimator animator;
    public StudentMovement movement;

    internal Vector3 startPos;
    internal Vector3 doorPos;

    public StudentLineManager studentCurrentLine;
    public int IndexInLine;


    internal Table table;

    public bool isReadyToChangePhase;
    public bool isSwitchingLine;

    public string studentKey;

    public static event Action<Student> OnStudentStart;

    private void GetComponentReferences()
    {
        skinMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponentInChildren<StudentAnimator>();
        movement = GetComponent<StudentMovement>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    private void Awake()
    {
        startPos = transform.localPosition;
        StudentData.SetOutline(0, this);
        if (PlayerPrefs.HasKey(studentKey))
        {
            gameObject.SetActive(true);
        }
    }
    private void Start()
    {
        OnStudentStart?.Invoke(this);

    }
    private void OnEnable()
    {
        RadialProgressBar.OnProgressComplete += MakeReadyForNextPhase;
    }
    private void OnDisable()
    {
        RadialProgressBar.OnProgressComplete -= MakeReadyForNextPhase;
    }
    private void OnMouseDown()
    {
        Actions.OnStudentSelection?.Invoke(this);
        capsuleCollider.enabled = false;
        movement.navMeshAgent.speed = data.studentSpeed;
        if (isReadyToChangePhase)
        {
            isSwitchingLine = true;
            movement.navMeshAgent.enabled = true;
            transform.parent = null;
            animator.Walk();
            movement.MoveToDestination(this, doorPos);
            movement.navMeshAgent.speed += data.studentSpeedTowardsDoor;
            table.boxCollider.enabled = true;
            if (phase == UniversityPhase.Enrollment)
            {
                Actions.OnStudentCeremony?.Invoke(this, 100);
            }
        }
        else
        {
            SelectCharacterAnimation();
        }
    }
    private void MakeReadyForNextPhase(Student obj)
    {
        if (obj == this)
        {
            StudentData.SetOutline(60, this);
            capsuleCollider.enabled = true;
            isReadyToChangePhase = true;
            data.UpdateStudentSkin(GetNextPhase(), obj);
            phase = GetNextPhase();
            obj.transform.DOLocalRotate(Vector3.zero, 0.2f);
        }
    }
    UniversityPhase GetNextPhase()
    {
        int _phase = (int)phase;
        _phase++;
        if (_phase > 2)
        {
            _phase = 0;
        }
        return (UniversityPhase)_phase;
    }
    public void InstantMoveOnSwitchingRoom()
    {
        if (isSwitchingLine)
            transform.localPosition = doorPos - Vector3.left * 0.2f;
    }
    public void ResetStudentDefaultState()
    {
        //st.transform.localPosition = st.startPos;
        movement.enabled = false;
        capsuleCollider.enabled = true;
        ResetSelectionAnimation();
    }
    //======== Data Persistance
    public void SaveUnlockStatus()
    {
        PlayerPrefs.SetInt(studentKey, 1);
        PlayerPrefs.Save();
    }
    #region Animations
    //===============Tweens===============
    internal Tween RearrangeAnimation;

    public void StartRearranging(Vector3 targetPosition, float duration)
    {
        StopSwitchLineTween();
        StopRearranging();
        RearrangeAnimation = transform.DOLocalMove(targetPosition, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.localPosition = targetPosition;
                animator.StopWalking();
            });
    }

    public void StopRearranging()
    {
        if (RearrangeAnimation != null && RearrangeAnimation.IsActive())
        {
            RearrangeAnimation.Kill();
        }
    }
    private Tween SwtichLineAnimation;

    bool switchLineAnimationCheck = false;
    public void SwitchLineAnimation(Vector3[] pathPoints, float duration, Student st, Vector3 targetPos)
    {
        // Stop the previous tween if it exists
        if (!switchLineAnimationCheck)
        {
            switchLineAnimationCheck = true;
            isReadyToChangePhase = false;
            StopSwitchLineTween();
            StopRearranging();

            SwtichLineAnimation = transform.DOLocalPath(pathPoints, duration, PathType.Linear, PathMode.Full3D)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    capsuleCollider.enabled = true;
                    switchLineAnimationCheck = false;
                    StartRearranging(targetPos, 1f);
                    movement.navMeshAgent.enabled = true;
                    // Animation completed
                    // Add any additional logic here
                });
        }
    }

    public void StopSwitchLineTween()
    {
        if (SwtichLineAnimation != null && SwtichLineAnimation.IsActive())
        {
            SwtichLineAnimation.Kill();
        }
    }
    private Tween SelectionAnimation;

    public void SelectCharacterAnimation()
    {
        // Stop the previous pop-up tween if it exists
        if (SelectionAnimation != null && SelectionAnimation.IsActive())
        {
            SelectionAnimation.Kill();
        }

        // Pop-up animation
        SelectionAnimation = transform.DOScale(Vector3.one * 1.2f, 0.2f)
            .SetEase(Ease.OutBack); // Use OutBack easing for a smooth pop-up effect
    }
    public void StopSelectionAnimation()
    {
        if (SelectionAnimation != null && SelectionAnimation.IsActive())
        {
            SelectionAnimation.Kill();
        }
    }
    private Tween ResetSelectionTween;

    public void ResetSelectionAnimation()
    {
        StopSelectionAnimation();
        // Stop the previous pop-up tween if it exists
        if (ResetSelectionTween != null && ResetSelectionTween.IsActive())
        {
            ResetSelectionTween.Kill();
        }

        // Pop-up animation
        ResetSelectionTween = transform.DOScale(Vector3.one, 0.2f)
            .SetEase(Ease.OutBack); // Use OutBack easing for a smooth pop-up effect
    }
    public void StopResetSelection()
    {
        if (ResetSelectionTween != null && ResetSelectionTween.IsActive())
        {
            ResetSelectionTween.Kill();
        }
    }
    #endregion
}
