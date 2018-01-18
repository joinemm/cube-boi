using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaScript : MonoBehaviour {

    public GameObject deathHandler;

	void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.GetComponent<playerMovement>().Die();
            GameObject deathHandlerGO = Instantiate(deathHandler, other.transform.position, Quaternion.identity);
            Destroy(deathHandlerGO, 1f);
        }
	}
}
