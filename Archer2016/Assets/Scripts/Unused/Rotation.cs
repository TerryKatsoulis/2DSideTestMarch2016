using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

    //Public Variables
    public float MaxSpeed = 20;
    public float Acceleration = 64;
    public float JumpSpeed = 8;
    public float JumpDuration = 150;

    //Input Variables
    private float horizontal;
    private float vertical;
    private float jumpInput;

    //Internal Variables
    private bool onTheGround;
    private float jmpDuration;
    private bool jumpKeyDown = false;
    private bool canVariableJump = false;
    private float movement_Anim;

    Rigidbody rigid;
    Animator anim;
    LayerMask layerMask;
    Transform modelTrans;

    public Transform shoulderTrans;
    public Transform rightShoulder;
    public Vector3 lookPos;
    GameObject rsp;

    public Transform cameraHolder;
    public float cameraSpeed;

    public enum CameraMovementType {
        Lerp, MoveTowards, AccelDecel, Acceleration
    }

    public CameraMovementType cMT;

    void Start () {
        rigid = GetComponent<Rigidbody>();
        SetupAnimator();

        layerMask = ~(1 << 8);

        rsp = new GameObject();
        rsp.name = transform.root.name + " Right Shoulder IK Helper";

        cameraHolder = Camera.main.transform.parent.transform;
    }

    void CameraMovement() {
        switch(cMT) {
            case CameraMovementType.Lerp:
                cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, transform.position, Time.deltaTime * cameraSpeed);
                break;
            case CameraMovementType.MoveTowards:
                cameraHolder.transform.position = Vector3.MoveTowards(cameraHolder.transform.position, transform.position, Time.deltaTime * cameraSpeed);
                break;
            case CameraMovementType.AccelDecel:
                cameraHolder.transform.position = CameraLibrary.AccelDecelInterpolation(cameraHolder.position, transform.position, Time.deltaTime * cameraSpeed);
                break;
            case CameraMovementType.Acceleration:
                cameraHolder.transform.position = CameraLibrary.AccelerationInterpolation(cameraHolder.position, transform.position, Time.deltaTime * cameraSpeed, 1);
                break;
        }
    }

    //void Update() {
    //    shoulderTrans.LookAt(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));
    //}

    void FixedUpdate() {
        CameraMovement();

        InputHandler();
        UpdateRigidbodyValues();
        MovementHandler();
        HandleRotation();
        HandleAimingPos();
        HandleAnimations();
        HandleShoulder();
    }

    void InputHandler() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        jumpInput = Input.GetAxis("Fire2");
    }

    void MovementHandler() {
        onTheGround = isOnGround();

        if(horizontal < -0.1f) {
            if(rigid.velocity.x > -this.MaxSpeed) {
                rigid.AddForce(new Vector3(-this.Acceleration, 0, 2));
            } else {
                rigid.velocity = new Vector3(-this.MaxSpeed, rigid.velocity.y, 2);
            }
        } else if (horizontal > 0.1f) {
            if(rigid.velocity.x < this.MaxSpeed) {
                rigid.AddForce(new Vector3(this.Acceleration, 0, 2));
            } else {
                rigid.velocity = new Vector3(this.MaxSpeed, rigid.velocity.y, 2);
            }
        }

        if(jumpInput > 0.1f) {
            if (!jumpKeyDown) {
                jumpKeyDown = true;

                if(onTheGround) {
                    rigid.velocity = new Vector3(rigid.velocity.y, this.JumpSpeed, 0);
                    jmpDuration = 0.0f;
                }
            } else if (canVariableJump) {
                jmpDuration += Time.deltaTime;

                if(jmpDuration < this.JumpDuration / 1000) {
                    rigid.velocity = new Vector3(rigid.velocity.x, this.JumpSpeed, 0);
                }
            }
        } else {
            jumpKeyDown = false;
        }
    }

    void HandleRotation() {
        Vector3 directionToLook = lookPos - transform.position;
        directionToLook.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToLook);

        Debug.Log(lookPos.x + " " + transform.position.x);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15);
    }

    void HandleAimingPos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            Vector3 lookP = hit.point;
            lookP.z = 2;
            lookPos = lookP;
        }
    }

    void HandleAnimations() {
        //anim.SetBool("OnAir", !onTheGround);
        float animValue = horizontal;

        if(lookPos.x < transform.position.x) {
            animValue = -animValue;
        }

        //anim.SetFloat("Movement", animValue, 0.1f, Time.deltaTime);
    }

    void HandleShoulder() {
        shoulderTrans.LookAt(lookPos);

        Vector3 rightShoulderPos = rightShoulder.TransformPoint(Vector3.zero);
        rsp.transform.position = rightShoulderPos;
        rsp.transform.parent = transform;

        shoulderTrans.position = rsp.transform.position;
    }

    void UpdateRigidbodyValues() {
        if (onTheGround) {
            rigid.drag = 4;
        } else {
            rigid.drag = 0;
        }
    }

    private bool isOnGround() {
        bool retVal = false;
        float lengthToSearch = 1.5f;

        Vector3 lineStart = transform.position + Vector3.up;
        Vector3 vectorToSearch = -Vector3.up;

        RaycastHit hit;

        if(Physics.Raycast(lineStart, vectorToSearch, out hit, lengthToSearch, layerMask)) {
            retVal = true;
        }
        return retVal;
    }

    void SetupAnimator() {
        anim = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>()) {
            if(childAnimator != anim) {
                anim.avatar = childAnimator.avatar;
                modelTrans = childAnimator.transform;
                Destroy(childAnimator);
                break;
            }
        }
    }
}
