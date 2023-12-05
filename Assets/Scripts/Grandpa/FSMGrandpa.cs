using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMGrandpa : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    public Rigidbody body;

    [HideInInspector] public GameObject seat;

    // States 
    [HideInInspector] public IGrandpaState currentState;
    [HideInInspector] public GrandpaWanderState wanderState;
    [HideInInspector] public GrandpaSeatingState seatingState;
    [HideInInspector] public GrandpaIdleState idleState;

    private void Awake()
    {
        wanderState = new GrandpaWanderState(this);
        seatingState = new GrandpaSeatingState(this);
        idleState = new GrandpaIdleState(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = wanderState;
        seat = null;

        agent.speed = Random.value * 2 + 1;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }

    public void TriggerIn(string tag)
    {
        currentState.TriggerIn(tag);
    }

    public void TriggerOut(string tag)
    {
        currentState.TriggerOut(tag);
    }
}
