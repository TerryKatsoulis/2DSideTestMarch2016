using UnityEngine;
using System.Collections;

public class IKHandler : MonoBehaviour {

    Animator anim;

    public Transform LeftIKTarget;
    public Transform RightIKTarget;

    //public Transform hintLeft;
    //public Transform hintRight;

    public float IKWeight = 1;

	void Start () {
        anim = GetComponent<Animator>();
	}
	
	void Update () {
        	
	}

    void OnAnimatorIK() {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeight);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, LeftIKTarget.position);
        anim.SetIKPosition(AvatarIKGoal.RightHand, RightIKTarget.position);

        //anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, IKWeight);
        //anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, IKWeight);

        //anim.SetIKHintPosition(AvatarIKHint.LeftElbow, hintLeft.position);
        //anim.SetIKHintPosition(AvatarIKHint.RightElbow, hintRight.position);

        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, IKWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, IKWeight);

        anim.SetIKRotation(AvatarIKGoal.LeftHand, LeftIKTarget.rotation);
        anim.SetIKRotation(AvatarIKGoal.RightHand, RightIKTarget.rotation);

    }
}
