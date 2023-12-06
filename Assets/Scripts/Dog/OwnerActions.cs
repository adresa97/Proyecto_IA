using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerActions : MonoBehaviour
{
    public DogActions dog;
    private bool isPursuing;

    public void Start()
    {
        isPursuing = false;
    }

    public bool IsPursuing() { return isPursuing; }
    public void SetPursuing(bool biting) { isPursuing = biting; }

    public void CatchDog() { dog.SetBiting(false); }
}
