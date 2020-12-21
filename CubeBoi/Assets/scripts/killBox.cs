using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killBox : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (this.gameObject.GetComponentInParent<doorScript>().open == false && collision.tag == "Player") {
            FindObjectOfType<gameController>().Respawn("door");
        }
    }
}
