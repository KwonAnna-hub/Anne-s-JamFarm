using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand : MonoBehaviour
{
    public Animator animator;
    public Transform rightHandTarget; // ���� ��ǥ ��ġ
    public Transform objectToFollow; // ���󰡾� �ϴ� ��ü

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if (rightHandTarget != null)
            {
                // ���� ��ǥ ��ġ ����
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
            }
        }
    }

    void LateUpdate()
    {
        // ���� ��ġ�� ���󰡵��� ��ü �̵�
        if (rightHandTarget != null && objectToFollow != null)
        {
            objectToFollow.position = rightHandTarget.position;
            objectToFollow.rotation = rightHandTarget.rotation;
        }
    }
}
