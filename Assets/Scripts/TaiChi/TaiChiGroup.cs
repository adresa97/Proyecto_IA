using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaiChiGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject taiTeacher;
    [SerializeField]
    private float separation;
    private Vector3 margin;

    void Start()
    {
        CalculatePositionAndMargin();
        transform.position = taiTeacher.transform.position + margin;
    }

    void Update()
    {
        transform.position = taiTeacher.transform.position + margin;
    }

    private void CalculatePositionAndMargin()
    {
        Vector3 initialPosition = taiTeacher.transform.position - taiTeacher.transform.forward * separation;
        margin = taiTeacher.transform.position - initialPosition;

        transform.forward = margin.normalized;
    }
}
