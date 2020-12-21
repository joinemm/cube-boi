using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour {

    public GameObject myShooter;
    gameController gameController;

    private void Start() {
        gameController = FindObjectOfType<gameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "tiles") {
            myShooter.GetComponent<shooterScript>().oldDestroyed = true;
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Player") {
            gameController.Respawn("bullet");
        }
    }
}
