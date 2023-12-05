using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandpaVisionManager : MonoBehaviour
{
    [SerializeField]
    private FSMGrandpa grandpa;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Human") && other.gameObject != grandpa.gameObject) || other.CompareTag("Insect"))
        {
            grandpa.TriggerIn("Human");
        }

        if (other.CompareTag("Seat"))
        {
            if (!other.GetComponent<Seat>().IsOccupied() && other.gameObject != grandpa.seat)
            {
                other.GetComponent<Seat>().SetOccupation(true);
                grandpa.seat = other.gameObject;
                grandpa.TriggerIn("Seat");
            }
            else
            {
                grandpa.TriggerIn("Human");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Human") && other.gameObject != grandpa.gameObject) || other.CompareTag("Insect"))
        {
            grandpa.TriggerOut("Human");
        }
    }
}
