using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Experimental.AI;
using UnityEngine.AI;
using System.Runtime.CompilerServices;

public class SecurityAgent : Agent
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform floor;

    [SerializeField]
    private float angularSpeed;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxSpeed;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rBody;

    private bool isAtTarget;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rBody = GetComponent<Rigidbody>();

        isAtTarget = false;
    }

    public override void OnEpisodeBegin()
    {
        // Reset agent if it falls
        if (transform.position.y < 0) 
        {
            rBody.angularVelocity = Vector3.zero;
            rBody.velocity = Vector3.zero;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }

        // Reset isAtTarget
        isAtTarget = false;

        // Relocate target
        Vector3 newPosition = Vector3.zero;
        newPosition.x = floor.position.x + Random.value * floor.localScale.x - floor.localScale.x/2;
        newPosition.z = floor.position.z + Random.value * floor.localScale.z - floor.localScale.z/2;

        NavMeshHit hit;
        NavMesh.SamplePosition(newPosition, out hit, floor.localScale.x/2, 1);
        newPosition = hit.position;

        newPosition.y = target.position.y;
        target.position = newPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent and target positions
        sensor.AddObservation(target.position);
        sensor.AddObservation(transform.position);

        // Velocities
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Actions
        // 2 continuous actions, 0 -> xForce, 1 -> zForce
        Vector3 force = Vector3.zero;
        force.x = actions.ContinuousActions[0];
        force.z = actions.ContinuousActions[1];

        rBody.AddForce(force * speed);
        rBody.velocity = Vector3.ClampMagnitude(rBody.velocity, maxSpeed);

        // Look to movement direction
        Vector3 rotation = rBody.velocity;
        rotation.y = 0;
        if (rotation != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation, Vector3.up), angularSpeed * Time.deltaTime);
        }

        // Rewards
        // Reached target
        if (isAtTarget)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell out floor
        else if (transform.position.y < 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target.gameObject) isAtTarget = true;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
