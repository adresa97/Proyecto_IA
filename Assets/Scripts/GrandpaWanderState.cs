using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class GrandpaWanderState : IGrandpaState
{
    private readonly FSMGrandpa fsm;
    private Transform transform;
    private NavMeshAgent agent;
    private NavMeshPath path;

    public GrandpaWanderState(FSMGrandpa fsm)
    {
        this.fsm = fsm;
        transform = fsm.transform;
        agent = fsm.GetAgent();
        path = new NavMeshPath();
    }

    public void UpdateState()
    {
        if (agent.pathPending)
        {
            Wander();
        }
        else if (agent.remainingDistance < 0.5f)
        {
            Wander();
        }
        else if (path.status == NavMeshPathStatus.PathInvalid)
        {
            TurnArround();
        }
    }

    public void TriggerIn(string tag)
    {
        if (tag == "Human")
        {
            TurnArround();
        }

        if (tag == "Seat")
        {
            ToSeatingState();
        }
    }

    public void TriggerOut(string tag) { }

    public void ToWanderState() { }

    public void ToSeatingState() 
    {
        fsm.currentState = fsm.seatingState;
    }

    public void ToIdleState()
    {
        fsm.currentState = fsm.idleState;
    }

    void Wander()
    {
        Vector3 target = GetVisibleDirection(); ;

        target *= 3;
        target += transform.position;

        Seek(target);
    }

    void TurnArround()
    {
        Vector3 target = -transform.forward;
        target += transform.position;

        Seek(target);
    }

    Vector3 GetVisibleDirection()
    {
        Vector3 target = transform.forward;
        float deviation = Random.Range(-50, 51);

        target = Quaternion.AngleAxis(deviation, Vector3.up) * target;

        return target;
    }

    void Seek(Vector3 target)
    {
        agent.CalculatePath(target, path);
        agent.destination = target;
    }
}
