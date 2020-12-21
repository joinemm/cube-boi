using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class climberScript : MonoBehaviour {

    public LayerMask tileMask;
    public bool animated = true;
    float speed = 5f;
    bool goingRight = true;
    float angle;

    void Update() {
        if (animated) {
            RotateGraphic();
        }
        RaycastHit2D sensorDown = Physics2D.Raycast(transform.position + transform.up * 0.1f, -transform.up, 0.3f, tileMask);
        RaycastHit2D sensorFrontTop = Physics2D.Raycast(transform.position + transform.right * 0.4f + transform.up * 0.95f, transform.right, 0.12f, tileMask);
        RaycastHit2D sensorBackTop = Physics2D.Raycast(transform.position - transform.right * 0.4f + transform.up * 0.95f, -transform.right, 0.12f, tileMask);

        if ((sensorFrontTop && goingRight) || (sensorBackTop && !goingRight)) {
            //issa square
            speed = speed * -1;
            goingRight = !goingRight;
        }

        if (sensorDown) {
            transform.rotation = Quaternion.FromToRotation(transform.up, sensorDown.normal) * transform.rotation;
            transform.position = sensorDown.point;
        } else if (!sensorDown) {
            if (goingRight) {
                this.transform.Rotate(new Vector3(0, 0, -90), Space.World);
                Debug.Log("sensor: no, -90 rot");
            } else {
                this.transform.Rotate(new Vector3(0, 0, 90), Space.World);
                Debug.Log("sensor: no, 90 rot");
            }
        }
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + transform.up * 0.1f, transform.position + transform.up * 0.1f - this.transform.up * 0.2f);

        Gizmos.DrawLine(transform.position + transform.right * 0.4f + transform.up * 0.5f, transform.position + transform.right * 0.4f + transform.up * 0.5f + transform.right * 0.12f);
        Gizmos.DrawLine(transform.position - transform.right * 0.4f + transform.up * 0.5f, transform.position - transform.right * 0.4f + transform.up * 0.5f - transform.right * 0.12f);
    }

    void RotateGraphic() {
        this.transform.GetChild(0).rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        angle = angle - speed;
    }
}
