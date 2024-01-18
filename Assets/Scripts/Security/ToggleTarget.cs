using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTarget : MonoBehaviour
{
    private MeshRenderer meshRender;
    private bool isPressed;

    void Start()
    {
        meshRender = GetComponent<MeshRenderer>();
        isPressed = false;
    }

    void Update()
    {
        if (isPressed)
        {
            if (Input.GetKeyUp(KeyCode.T)) isPressed = false;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            meshRender.enabled = !meshRender.enabled;
            isPressed = true;
        }
    }
}
