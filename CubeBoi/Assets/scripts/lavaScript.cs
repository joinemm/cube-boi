using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            FindObjectOfType<gameController>().Respawn("lava");
        }
	}

    private void OnParticleCollision(GameObject other) {
        Destroy(other);
    }
}
