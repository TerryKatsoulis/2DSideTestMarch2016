using UnityEngine;
using System.Collections;

public class IKHandles : MonoBehaviour {

    Animator anim;
    Vector3 lookPos;
    Vector3 IK_lookPos;
    Vector3 targetPos;
    Rotation pl;

    public float lerpRate = 15;
    public float updateLookPosThreshold = 2;
    public float lookWeight = 1;
    public float bodyWeight = 0.9f;
    public float headWeight = 1;
    public float clampWeight = 1;

    public float rightHandWeight = 1;
    public float leftHandWeight = 1;

    public Transform rightHandTarget;
    public Transform leftHandTarget;

	// Use this for initialization
	void Start () {
        this.anim = GetComponent<Animator>();
        pl = GetComponent<Rotation>();
	}

    void OnAnimatorIK() {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);

        //anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, IKWeight);
        //anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, IKWeight);

        //anim.SetIKHintPosition(AvatarIKHint.LeftElbow, hintLeft.position);
        //anim.SetIKHintPosition(AvatarIKHint.RightElbow, hintRight.position);

        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);

        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);

        this.lookPos = pl.lookPos;
        lookPos.z = transform.position.z;

        float distanceFromPlayer = Vector3.Distance(lookPos, transform.position);

        if(distanceFromPlayer > updateLookPosThreshold) {
            targetPos = lookPos;
        }

        IK_lookPos = Vector3.Lerp(IK_lookPos, targetPos, Time.deltaTime * lerpRate);

        anim.SetLookAtWeight(lookWeight, bodyWeight, headWeight, clampWeight);
        anim.SetLookAtPosition(IK_lookPos);
    }
}
