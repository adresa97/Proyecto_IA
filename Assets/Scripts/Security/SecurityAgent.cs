using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Experimental.AI;
using UnityEngine.AI;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;

public class SecurityAgent : Agent
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform floor;
    [SerializeField]
    private float targetPadding;

    [SerializeField]
    private float angularSpeed;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxSpeed;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rBody;

    private float floorWorkingSizeX;
    private float floorWorkingSizeZ;

    private bool isAtTarget;

    /* TRAINING ONLY
    private bool isAtObstacle;
    */

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rBody = GetComponent<Rigidbody>();

        floorWorkingSizeX = floor.localScale.x;
        if (targetPadding < floor.localScale.x / 2) floorWorkingSizeX -= 2 * targetPadding;

        floorWorkingSizeZ = floor.localScale.z;
        if (targetPadding < floor.localScale.z / 2) floorWorkingSizeZ -= 2 * targetPadding;

        isAtTarget = false;

        /* TRAINING ONLY
        isAtObstacle = false;
        */
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

        /* TRAINING ONLY
        // Reset isAtObstacle
        isAtObstacle = false;
        */

        // Relocate target
        target.position = RandomPositionOnFloor(floor);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent and target positions
        sensor.AddObservation(NormalizedPosition(target.position));
        sensor.AddObservation(NormalizedPosition(transform.position));

        // Agent direction
        sensor.AddObservation(transform.forward);

        // Velocities, normalized to [-1, 1]
        sensor.AddObservation(rBody.velocity.x / maxSpeed);
        sensor.AddObservation(rBody.velocity.z / maxSpeed);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Actions
        // Discrete action 0: rotation: 0 -> static, 1 -> rotate right, 2 -> rotate left
        int rotateDirection = 0;
        if (actions.DiscreteActions[0] == 1) rotateDirection = 1;
        else if (actions.DiscreteActions[0] == 2) rotateDirection = -1;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += rotateDirection * angularSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation);

        // Discrete action 1: movement: 0 -> static, 1 -> move forwward, 2 -> move backward
        int moveDirection = 0;
        if (actions.DiscreteActions[1] == 1) moveDirection = 1;
        else if (actions.DiscreteActions[1] == 2) moveDirection = -1;

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
        else if (transform.position.y < floor.position.y + floor.localScale.y/2)
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        /* TRAINING ONLY
        // Collides with obstacle or human
        else if (isAtObstacle)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
        */
    }

    public void OnTriggerStay(Collider other)
    {
        // If activates trigger at target gameObject
        if (other.gameObject == target.gameObject) isAtTarget = true;
    }

    /* TRAINING ONLY
    public void OnCollisionEnter(Collision collision)
    {
        // If collides with obstacle or human
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Human")) isAtObstacle = true;
    }
    */

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Decide rotation state: 0 -> no input, 1 -> right arrow, 2 -> left arrow
        int rotationOutput = 0;
        if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) rotationOutput = 1;
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) rotationOutput = 2;

        // Decide movement state: 0 -> no input, 1 -> up arrow, 2 -> down arrow
        int movementOutput = 0;
        if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) movementOutput = 1;
        else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)) movementOutput = 2;

        // Store outputs in discrete actions
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = rotationOutput;
        discreteActionsOut[1] = movementOutput;
    }

    private Vector3 RandomPositionOnFloor(Transform floor)
    {
        // Get a random position on floor
        Vector3 randomPosition = Vector3.zero;
        randomPosition.x = floor.position.x + UnityEngine.Random.value * floorWorkingSizeX - floorWorkingSizeX / 2;
        randomPosition.z = floor.position.z + UnityEngine.Random.value * floorWorkingSizeZ - floorWorkingSizeZ / 2;

        // Check if position is walkable (get nearest walkable position if not)
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, floorWorkingSizeX / 2, 1);
        randomPosition = hit.position;

        // Keep target y position value
        randomPosition.y = target.position.y;

        return randomPosition;
    }

    private Vector2 NormalizedPosition(Vector3 position)
    {
        // Turn 3 dimension position to 2 dimension position in range from -1 to 1
        Vector2 normalizedPosition = Vector2.zero;
        normalizedPosition.x = 2 * (position.x - floor.position.x) / floor.localScale.x;
        normalizedPosition.y = 2 * (position.z - floor.position.z) / floor.localScale.z;

        return normalizedPosition;
    }
}
