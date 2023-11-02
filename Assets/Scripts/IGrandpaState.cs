using System.Collections;
using UnityEngine;

public interface IGrandpaState
{
    void UpdateState();

    void TriggerIn(string tag);

    void TriggerOut(string tag);

    void ToWanderState();

    void ToSeatingState();

    void ToIdleState();
}
