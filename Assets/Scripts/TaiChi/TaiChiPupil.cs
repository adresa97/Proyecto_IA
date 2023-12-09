using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TaiChiPupil : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private GameObject teacher;

    [SerializeField]
    private GameObject slot;

    [SerializeField]
    private TaiChiAnimatorController teacherAnimator;

    [SerializeField]
    private TaiChiAnimatorController pupilAnimator;

    [SerializeField]
    private float animationDelay;

    private TaiStates previousState;

    private NavMeshPath path;

    void Start()
    {
        transform.forward = -transform.forward;
        transform.position = slot.transform.position;

        path = new NavMeshPath();

        agent.CalculatePath(slot.transform.position, path);
        agent.destination = slot.transform.position;

        previousState = TaiStates.STATIC;
    }

    void Update()
    {
        if (!agent.pathPending || agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.CalculatePath(slot.transform.position, path);
            agent.destination = slot.transform.position;
        }

        if (IsAtSlot()) UpdateOrientation();

        if (teacherAnimator.GetTaiState() != previousState) StartCoroutine(UpdateAnimation());
    }

    private bool IsAtSlot()
    {
        bool isX = transform.position.x == slot.transform.position.x;
        bool isZ = transform.position.z == slot.transform.position.z;

        return isX && isZ;
    }

    private void UpdateOrientation()
    {
        Vector3 targetRotation = teacher.transform.forward + teacher.transform.position - transform.position;
        transform.forward = Vector3.Lerp(transform.forward, targetRotation , 0.1f);
    }

    private IEnumerator UpdateAnimation()
    {
        previousState = teacherAnimator.GetTaiState();

        yield return new WaitForSecondsRealtime(Random.Range(0, animationDelay));

        switch (previousState)
        {
            case TaiStates.STATIC:
            {
                pupilAnimator.ToStatic();
            } break;
            case TaiStates.POSE_1:
            {
                pupilAnimator.ToPose1();
            } break;
            case TaiStates.POSE_2:
            {
                pupilAnimator.ToPose2();
            } break;
            case TaiStates.POSE_3:
            {
                pupilAnimator.ToPose3();
            } break;
            case TaiStates.POSE_4:
            {
                pupilAnimator.ToPose4();
            } break;
            default: break;
        }
    }
}
