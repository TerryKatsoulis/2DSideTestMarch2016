using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

    public GameObject bullet;
    public float speed = 10.0f;
    public float RateofFire;
    public float fireTimer;
    public Vector3 aimPos;
    public Vector3 myPos;
    public Vector3 direction;
    Camera cam;
    public Transform archer;

    void Start() {

        fireTimer = Time.time + RateofFire;
        cam = Camera.main;

    }

    void FixedUpdate() {
        // to find the position on the plane of the camera so as to aim there
        if (Input.GetMouseButtonDown(0) && Time.time > fireTimer) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity)) {
                Vector3 lookP = hitInfo.point;
                //lookP.z = 2;
                aimPos = lookP;
                //GameObject instance = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
                //instance.GetComponent<Rigidbody>().AddForce(Vector3.right * speed);
                //Vector3 aimPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y));
                myPos = new Vector3(archer.position.x, archer.position.y, archer.position.z);
                direction = aimPos - myPos;

                direction.Normalize();

                if (ControllerScript.onXY) {
                    Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

                    GameObject projectile = (GameObject)Instantiate(bullet, transform.position, rotation);
                    if (archer.position.x < projectile.transform.position.x) {
                        projectile.transform.Rotate(0, 0, 60);
                    } else if (archer.position.x > projectile.transform.position.x) {
                        projectile.transform.Rotate(0, 0, 120);
                    }
                    projectile.GetComponent<Rigidbody>().velocity = transform.forward * speed;
                    fireTimer = Time.time + RateofFire;
                } else if (ControllerScript.onXZ) {
                    Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

                    GameObject projectile = (GameObject)Instantiate(bullet, transform.position, rotation);
                    if (archer.position.z < projectile.transform.position.z) {
                        projectile.transform.Rotate(60, 0, 0);
                    } else if (archer.position.z > projectile.transform.position.z) {
                        projectile.transform.Rotate(120, 0, 0);
                    }
                    projectile.GetComponent<Rigidbody>().velocity = transform.forward * speed;
                    fireTimer = Time.time + RateofFire;
                }


                //Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));
                //Vector3 myPos = new Vector3(archer.position.x, archer.position.y, 2);
                //Vector3 direction = target - myPos;
                //direction.Normalize();
                //Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
                //GameObject projectile = (GameObject)Instantiate(bullet, transform.position, rotation);
                //projectile.GetComponent<Rigidbody>().velocity = direction * speed;
                //fireTimer = Time.time + RateofFire;
            }
        }
    }
}
