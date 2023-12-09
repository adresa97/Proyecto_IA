using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaiStates { STATIC, POSE_1, POSE_2, POSE_3, POSE_4 };

public class TaiChiAnimatorController : MonoBehaviour
{
    [SerializeField]
    private Animator taiAnimator;

    private TaiStates currentState;

    public TaiStates GetTaiState() { return currentState; }

    private void ResetTriggers()
    {
        taiAnimator.ResetTrigger("toStatic");
        taiAnimator.ResetTrigger("toPose1");
        taiAnimator.ResetTrigger("toPose2");
        taiAnimator.ResetTrigger("toPose3");
        taiAnimator.ResetTrigger("toPose4");
    }

    public void ToStatic()
    {
        currentState = TaiStates.STATIC;
        SetCurrentAnimation();
    }

    public void ToPose1()
    {
        currentState = TaiStates.POSE_1;
        SetCurrentAnimation();
    }

    public void ToPose2()
    {
        currentState = TaiStates.POSE_2;
        SetCurrentAnimation();
    }

    public void ToPose3()
    {
        currentState = TaiStates.POSE_3;
        SetCurrentAnimation();
    }

    public void ToPose4()
    {
        currentState = TaiStates.POSE_4;
        SetCurrentAnimation();
    }

    private void SetCurrentAnimation()
    {
        ResetTriggers();

        switch(currentState)
        {
            case TaiStates.STATIC:
                {
                    taiAnimator.SetTrigger("toStatic");
                } break;
            case TaiStates.POSE_1:
                {
                    taiAnimator.SetTrigger("toPose1");
                } break;
            case TaiStates.POSE_2:
                {
                    taiAnimator.SetTrigger("toPose2");
                } break;
            case TaiStates.POSE_3:
                {
                    taiAnimator.SetTrigger("toPose3");
                } break;
            case TaiStates.POSE_4:
                {
                    taiAnimator.SetTrigger("toPose4");
                } break;
            default: break;
        }
    }
}
