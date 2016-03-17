using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour {
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 5.0f;
    public float jumpSpeed = 9.0f;

    //public Vector3 lookPos;

    //float verticalRotation = 0;
    public float upDownRange = 60.0f;

    float verticalVelocity = 0;

    public bool run = false;
    public bool playerDead = false;
    private bool aDown = false;
    private bool dDown = false;

    float cooldownRemaining = 2.0f;

    //public Transform Bow;
    //public Transform rightShoulder;

    CharacterController characterController;
    //Animator anim;

    // Use this for initialization
    void Start() {
        characterController = GetComponent<CharacterController>();
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        aDown = false;
        dDown = false;
        playerDead = false;
        run = false;

        cooldownRemaining -= Time.deltaTime;

        if (!playerDead) {
            if (Time.timeScale != 0f) {

                //verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
                //verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
                //Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
                float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

                verticalVelocity += Physics.gravity.y * 2f * Time.deltaTime;

                if (characterController.isGrounded && Input.GetButtonDown("Jump")) {
                    verticalVelocity = jumpSpeed;
                }

                Vector3 walk = new Vector3(sideSpeed, verticalVelocity, 0);
                walk = transform.rotation * walk;

                //if (run == false) {
                //    characterController.Move(walk * Time.deltaTime);
                //}

                //if (Input.GetKey(KeyCode.A) && !dDown) {
                //    anim.SetTrigger("Left");
                //    aDown = true;
                //} else if (Input.GetKey(KeyCode.D) && !aDown) {
                //    anim.SetTrigger("Right");
                //    dDown = true;
                //}  else
                //    anim.SetTrigger("Idle");
                //dDown = false;
            }
        }
    }

    //void FixedUpdate() {
    //    HandleRotation();
    //    HandleAimingPos();
    //    HandleBow();
    //}

    //void HandleRotation () {
    //    Vector3 directionToLook = lookPos - transform.position;
    //    directionToLook.y = 0;
    //    Quaternion targetRotation = Quaternion.LookRotation(directionToLook);

    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15);
    //}

    //void HandleAimingPos () {
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    RaycastHit hit;

    //    if(Physics.Raycast(ray, out hit, Mathf.Infinity)) {
    //        Vector3 lookP = hit.point;
    //        lookP.z = transform.position.z;
    //        lookPos = lookP;
    //    }
    //}

    //void HandleBow() {
    //    Bow.LookAt(lookPos);
    //    Vector3 rightShoulderPos = rightShoulder.TransformPoint(Vector3.zero);

    //    Bow.position = rightShoulderPos;
    //}

    void OnTriggerEnter (Collider col) {
        if(col.gameObject.tag == "TurnTrigger" && Input.GetKeyDown(KeyCode.R)) {
            transform.Rotate(0, -90, 0);
            //transform.Rotate(new Vector3(0, -90, 0) * 100f * Time.deltaTime);
        }
    }
}
