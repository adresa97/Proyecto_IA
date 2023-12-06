using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogActions : MonoBehaviour
{
    public OwnerActions owner;
    private bool isBiting;

    public void Start()
    {
        isBiting = false;
    }

    public bool IsBiting() { return isBiting; }
    public void SetBiting(bool biting) { isBiting = biting; }

    public void AlarmOwner() { owner.SetPursuing(true); }
}
