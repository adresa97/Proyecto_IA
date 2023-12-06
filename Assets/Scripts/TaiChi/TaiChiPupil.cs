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

    private NavMeshPath path;

    void Start()
    {
        transform.forward = -transform.forward;
        transform.position = slot.transform.position;

        path = new NavMeshPath();

        agent.CalculatePath(slot.transform.position, path);
        agent.destination = slot.transform.position;
    }

    void Update()
    {
        if (!agent.pathPending || agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.CalculatePath(slot.transform.position, path);
            agent.destination = slot.transform.position;
        }

        if (IsAtSlot()) UpdateOrientation();
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
}
