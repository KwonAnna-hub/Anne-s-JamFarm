using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand : MonoBehaviour
{
    public Animator animator;
    public Transform rightHandTarget; // 손의 목표 위치
    public Transform objectToFollow; // 따라가야 하는 물체

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if (rightHandTarget != null)
            {
                // 손의 목표 위치 설정
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
            }
        }
    }

    void LateUpdate()
    {
        // 손의 위치를 따라가도록 물체 이동
        if (rightHandTarget != null && objectToFollow != null)
        {
            objectToFollow.position = rightHandTarget.position;
            objectToFollow.rotation = rightHandTarget.rotation;
        }
    }
}
