using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class RunnerPatrolling : MonoBehaviour
{
    [SerializeField]
    private PathCreator bezier;

    [SerializeField]
    private NavMeshAgent agent;
    private NavMeshPath path;
    private float speed;
    private int direction;

    private Vector3 target;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.value * 3 + 2;
        agent.speed = speed;

        direction = Random.value < 0.5f ? 1 : -1;

        path = new NavMeshPath();

        GoToPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Patrol();
        }
    }

    void GoToPath()
    {
        if (bezier != null)
        {
            distance = bezier.path.GetClosestDistanceAlongPath(transform.position);
            target = bezier.path.GetPointAtDistance(distance);
            agent.CalculatePath(target, path);
            agent.destination = target;
        }
    }

    public void Patrol()
    {
        if (bezier != null)
        {
            distance += direction * 3;
            target = bezier.path.GetPointAtDistance(distance);
            agent.CalculatePath(target, path);
            agent.destination = target;
        }
    }

    public void SetClosestDistance()
    {
        distance = bezier.path.GetClosestDistanceAlongPath(transform.position);
    }
}
