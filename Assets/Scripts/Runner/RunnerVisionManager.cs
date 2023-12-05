using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerVisionManager : MonoBehaviour
{
    [SerializeField]
    private RunnerPatrolling runner;

    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Human") && other.gameObject != runner.gameObject) || other.CompareTag("Insect"))
        {
            runner.SetClosestDistance();
            runner.Patrol();
        }
    }
}
