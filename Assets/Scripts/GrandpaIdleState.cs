using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrandpaIdleState : IGrandpaState
{
    private readonly FSMGrandpa fsm;
    private NavMeshAgent agent;
    private NavMeshPath path;
    private Transform transform;
    private float timer;

    public GrandpaIdleState(FSMGrandpa fsm)
    {
        this.fsm = fsm;
        agent = fsm.GetAgent();
        path = new NavMeshPath();
        transform = fsm.transform;

        timer = 0;
    }

    public void UpdateState()
    {
        if (timer < 20)
        {
            timer += Time.deltaTime;
        }
        else
        {
            agent.CalculatePath(transform.position + transform.forward * 2, path);
            agent.destination = transform.position + transform.forward * 2;

            fsm.seat.GetComponent<Seat>().SetOccupation(false);

            timer = 0;
            ToWanderState();
        }
    }

    public void TriggerIn(string tag) { }

    public void TriggerOut(string tag) { }

    public void ToWanderState()
    {
        fsm.currentState = fsm.wanderState;
    }

    public void ToSeatingState()
    {
        fsm.currentState = fsm.seatingState;
    }

    public void ToIdleState() {}
}
