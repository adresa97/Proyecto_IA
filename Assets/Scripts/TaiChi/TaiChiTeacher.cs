using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaiChiTeacher : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Transform firstPosition;

    [SerializeField]
    private Transform secondPosition;

    [SerializeField]
    private TaiChiAnimatorController animator;

    [SerializeField]
    private int animationLoop;

    [SerializeField]
    private int stoppingTime;

    private Vector3 initialForward;

    private int animationCounter;
    private int stationaryCounter;
    private int state;
    private NavMeshPath path;

    private void Start()
    {
        initialForward = transform.forward;

        animationCounter = 0;
        stationaryCounter = 0;
        state = 1;
        path = new NavMeshPath();
        SetDestination(secondPosition.position);
    }

    private void Update()
    {
        animationCounter++;

        if (animationCounter >= animationLoop)
        {
            SetAnimation();
            animationCounter = 0;
        }

        switch(state)
        {
            case 0:
                {
                    transform.forward = initialForward;
                    stationaryCounter++;
                    if (stationaryCounter >= stoppingTime)
                    {
                        stationaryCounter = 0;
                        SetDestination(secondPosition.position);
                        agent.isStopped = false;
                        state = 1;
                    }
                } break;
            case 1:
                {
                    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        agent.isStopped = true;
                        state = 2;
                    }
                } break;
            case 2:
                {
                    transform.forward = initialForward;
                    stationaryCounter++;
                    if (stationaryCounter >= stoppingTime)
                    {
                        stationaryCounter = 0;
                        SetDestination(firstPosition.position);
                        agent.isStopped = false;
                        state = 3;
                    }
                } break;
            case 3:
                {
                    if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                    {
                        agent.isStopped = true;
                        state = 0;
                    }
                } break;
            default: break;
        }
    }

    private void SetDestination(Vector3 destination) 
    {
        agent.CalculatePath(destination, path);
        agent.destination = destination;
    }

    private void SetAnimation()
    {
        int random = Random.Range(0, 5);

        switch(random)
        {
            case 0: animator.ToStatic(); break;
            case 1: animator.ToPose1(); break;
            case 2: animator.ToPose2(); break;
            case 3: animator.ToPose3(); break;
            case 4: animator.ToPose4(); break;
            default: break;
        }
    }
}
