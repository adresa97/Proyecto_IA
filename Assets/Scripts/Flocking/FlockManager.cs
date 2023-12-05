using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    private GameObject[] boids;

    [SerializeField]
    private GameObject boidPrefab;

    [SerializeField]
    private int boidCount;

    [SerializeField]
    private Transform boidContainer;

    [SerializeField]
    private Vector2 flockLimits;

    public float minSpeed, maxSpeed, rotationSpeed, distance, fieldOfView;

    public float cohesionWeight, alignWeight, separationWeight, centerWeight;

    public void Start()
    {
        boids = new GameObject[boidCount];
        for (int i = 0; i < boidCount; i++)
        {
            Vector3 boidPosition = Random.insideUnitSphere;
            boidPosition.x *= flockLimits.x;
            boidPosition.y *= flockLimits.y;
            boidPosition.z *= flockLimits.x;
            boidPosition += transform.position;

            Vector3 boidDirection = Random.onUnitSphere;
            boidDirection.y = 0;

            boids[i] = Instantiate(boidPrefab, boidPosition, Quaternion.LookRotation(boidDirection), boidContainer);
            boids[i].GetComponent<FlockBoid>().SetManager(this);
        }
    }

    public GameObject[] GetBoids()
    {
        return boids;
    }

    public Vector3 GetLimits()
    {
        return flockLimits;
    }

    public bool IsInsideRange(Vector3 boidPosition)
    {
        return Vector2.Distance(new Vector2(boidPosition.x, boidPosition.z), new Vector2(transform.position.x, transform.position.z)) <= flockLimits.x;
    }

    public void LimitHeight(Transform boidTransform)
    {
        float targetY = boidTransform.position.y;
        if (boidTransform.position.y > transform.position.y + flockLimits.y) targetY = transform.position.y + flockLimits.y;
        if (boidTransform.position.y < transform.position.y - flockLimits.y) targetY = transform.position.y - flockLimits.y;

        boidTransform.position = new Vector3(boidTransform.position.x, targetY, boidTransform.position.z);
    }
}
