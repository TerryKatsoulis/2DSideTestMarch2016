using UnityEngine;
using System.Collections;

public class ArrowCurve : MonoBehaviour {

    public Transform archer;

    //Update is called once per frame
    void FixedUpdate() {

        Vector3 vel = gameObject.GetComponent<Rigidbody>().velocity;
        float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
        //gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        gameObject.transform.eulerAngles = new Vector3(0, 0, angle);

        if (archer.position.x < transform.position.x) {
            transform.Rotate(0, 0, 90);
        } else if (archer.position.x > transform.position.x) {
            transform.Rotate(0, 0, -90);
        }

    }

}

