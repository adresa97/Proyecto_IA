using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    private bool isOccupied;

    private void Start()
    {
        isOccupied = false;
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void SetOccupation(bool occupation)
    {
        isOccupied = occupation;
    }
}
