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

    private float floorLeftBorder, floorRightBorder, floorTopBorder, floorBottomBorder;

    private bool isAtTarget;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rBody = GetComponent<Rigidbody>();

        floorLeftBorder = floor.position.x - floor.localScale.x/2;
        floorRightBorder = floor.position.x + floor.localScale.x/2;
        floorTopBorder = floor.position.z + floor.localScale.z/2;
        floorBottomBorder = floor.position.z - floor.localScale.z/2;

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
        newPosition.x = floorLeftBorder + Random.value * floor.localScale.x;
        newPosition.z = floorBottomBorder + Random.value * floor.localScale.z;

        NavMeshHit hit;
        NavMesh.SamplePosition(newPosition, out hit, floor.localScale.x/2, 1);
        newPosition = hit.position;

        newPosition.y = target.position.y;
        target.position = newPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent and target positions
        sensor.AddObservation(new Vector2((target.position.x - floorLeftBorder) / (floorRightBorder - floorLeftBorder),
                                          (target.position.z - floorBottomBorder) / (floorTopBorder - floorBottomBorder)));
        sensor.AddObservation(new Vector2((transform.position.x - floorLeftBorder) / (floorRightBorder - floorLeftBorder),
                                          (transform.position.z - floorBottomBorder) / (floorTopBorder - floorBottomBorder)));

        sensor.AddObservation((target.localPosition - transform.localPosition).normalized);
        sensor.AddObservation(transform.forward);

        // Velocities
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Actions
        // Discrete action 0: rotation: 1 -> static, 2 -> rotate right, 3 -> rotate left
        int rotateDirection = 0;
        if (actions.DiscreteActions[0] == 2) rotateDirection = 1;
        else if (actions.DiscreteActions[0] == 3) rotateDirection = -1;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += rotateDirection * angularSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation);

        // Discrete action 1: movement: 1 -> static, 2 -> move forwward, 3 -> move backward
        int moveDirection = 0;
        if (actions.DiscreteActions[1] == 2) moveDirection = 1;
        else if (actions.DiscreteActions[1] == 3) moveDirection = -1;

        rBody.AddForce(transform.forward * moveDirection * speed);
        if (rBody.velocity.magnitude > maxSpeed) rBody.velocity = rBody.velocity.normalized * maxSpeed;

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
        int rotationOutput = 1;
        if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) rotationOutput = 2;
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) rotationOutput = 3;

        int movementOutput = 1;
        if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) movementOutput = 2;
        else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)) movementOutput = 3;

        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = rotationOutput;
        discreteActionsOut[1] = movementOutput;
    }
}
