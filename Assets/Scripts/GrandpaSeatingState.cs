using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrandpaSeatingState : IGrandpaState
{
    private readonly FSMGrandpa fsm;
    private NavMeshAgent agent;
    private Transform transform;
    private NavMeshPath path;

    private bool started;

    public GrandpaSeatingState(FSMGrandpa fsm)
    {
        this.fsm = fsm;
        agent = fsm.GetAgent();
        transform = fsm.transform;
        path = new NavMeshPath();

        started = false;
    }

    public void UpdateState()
    {
        if (!started)
        {
            agent.CalculatePath(fsm.seat.transform.position, path);
            agent.destination = fsm.seat.transform.position;

            started = true;
        }
        else if (agent.remainingDistance <= 0)
        {
            transform.forward = fsm.seat.transform.forward;

            started = false;
            ToIdleState();
        }
    }

    public void TriggerIn(string tag) { }

    public void TriggerOut(string tag) { }

    public void ToWanderState()
    {
        fsm.currentState = fsm.wanderState;
    }

    public void ToSeatingState() {}

    public void ToIdleState()
    {
        fsm.currentState = fsm.idleState;
    }
}
