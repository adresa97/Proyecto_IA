using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FlockBoid : MonoBehaviour
{
    private FlockManager manager;

    private Vector3 cohesion, align, separation, toCenter;
    public Vector3 direction;

    private float speed;

    public void Start()
    {
        StartCoroutine(SetDirection());
    }

    public void Update()
    {
        if (!manager.IsInsideRange(transform.position))
        {
            toCenter = (manager.transform.position - transform.position).normalized * manager.centerWeight;
            transform.forward = Vector3.Slerp(transform.forward, toCenter, manager.rotationSpeed * manager.centerWeight * Time.deltaTime);
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            transform.forward = Vector3.Slerp(transform.forward, direction, manager.rotationSpeed * Time.deltaTime);
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        manager.LimitHeight(transform);
    }

    public void SetManager(FlockManager manager)
    {
        this.manager = manager;
        speed = manager.minSpeed;
    }

    private void CalculeComponents()
    {
        cohesion = Vector3.zero;
        align = Vector3.zero;
        separation = Vector3.zero;
        toCenter = Vector3.zero;

        int num = 0;

        foreach(GameObject boid in manager.GetBoids())
        {
            if (boid != gameObject)
            {
                float distance = Vector3.Distance(boid.transform.position, transform.position);
                float angle = Vector3.Angle(transform.forward, boid.transform.position - transform.position);

                if (distance <= manager.distance && angle <= manager.fieldOfView)
                {
                    cohesion += boid.transform.position;
                    align += boid.GetComponent<FlockBoid>().direction;
                    separation -= (transform.position - boid.transform.position) / (distance * distance);

                    num++;
                }
            }
        }

        if (num > 0)
        {
            align /= num;
            speed = Mathf.Clamp(align.magnitude, manager.minSpeed, manager.maxSpeed);

            cohesion = (cohesion / num - transform.position).normalized * speed;
        }
    }

    private IEnumerator SetDirection()
    {
        while(true)
        {
            CalculeComponents();
            direction = (cohesion + align + separation).normalized * speed;

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
