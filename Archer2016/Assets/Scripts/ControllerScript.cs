using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ControllerScript : MonoBehaviour {

    float verticalVelocity = 0;
    public float movementSpeed = 5.0f;
    public float jumpSpeed = 9.0f;

    CharacterController characterController;
    private Animator anim;

    public Transform archer;
    public Transform head;
    Camera cam;

    public static bool onXY = true;
    public static bool onXZ = false;

    public Vector3 lookPos;

    void Awake() {
        cam = Camera.main;
    }

    // Use this for initialization
    void Start () {

        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {

        //rotation of the bow and arms
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity)) {
            archer.LookAt(hitInfo.point);
            head.LookAt(hitInfo.point);
            Vector3 lookP = hitInfo.point;
            //lookP.z = 2;
            lookPos = lookP;

            if (onXY) {
                if (lookPos.x < transform.position.x && anim.GetBool("flip") == false) {
                    anim.SetBool("flip", true);
                } else if (lookPos.x > transform.position.x && anim.GetBool("flip") == true) {
                    anim.SetBool("flip", false);
                }
            } else if (onXZ) {
                if(lookPos.z < transform.position.z && anim.GetBool("flip") == false) {
                    anim.SetBool("flip", true);
                } else if (lookPos.z > transform.position.z && anim.GetBool("flip") == true) {
                    anim.SetBool("flip", false);
                }
            }
            //else if (lookPos.z > transform.position.z + 1f && anim.GetBool("flip") == true) {
            //    anim.SetBool("flip", false);
            //} else if (lookPos.z > transform.position.z - 1f && anim.GetBool("flip") == true) {
            //    anim.SetBool("flip", false);
            //}
        }

        //animations for the archer
        anim.SetFloat("vSpeed", Input.GetAxis("Horizontal"));



        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Input.GetAxis("Horizontal") == 0f) {
                anim.SetBool("isJumping", true);
                Invoke("StopJumping", 0.5f);
            } else if (Input.GetAxis("Horizontal") > 0) {
                anim.SetBool("isRunJumping", true);
                Invoke("StopJumping", 0.5f);
            } //else {
            //    anim.SetBool("isJumping", false);
            //}
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            if (anim.GetInteger("currentAction") == 0) {
                anim.SetInteger("currentAction", 1);
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Mathf.Lerp(Camera.main.transform.position.z, Camera.main.transform.position.z + 1.5f, 3f));
            } else if (anim.GetInteger("currentAction") == 1) {
                anim.SetInteger("currentAction", 0);
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - 1.5f);
            }
        }

    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "TurnTrigger" && Input.GetKeyDown(KeyCode.R)) {
            Debug.Log("On trigger");
            if (onXY) {
                transform.Rotate(0, -90, 0);
                onXY = false;
                onXZ = true;
                Debug.Log("XZ true");
            } else if (onXZ) {
                transform.Rotate(0, 90, 0);
                onXY = true;
                onXZ = false;
                Debug.Log("XY true");
            }
            //transform.RotateAround(col.gameObject.transform.position, Vector3.up, -90);
            //transform.Rotate(new Vector3(0, -90, 0) * 100f * Time.deltaTime);
        }
    }

    void StopJumping() {
        anim.SetBool("isJumping", false);
        anim.SetBool("isRunJumping", false);
    }
}
