using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour {

    public GameObject door;
    public float doorTime;
    Coroutine timerCor;
    bool somethingOnButton = false;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player" || collision.tag == "pushesButtons") {
            somethingOnButton = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player" || collision.tag == "pushesButtons") {
            somethingOnButton = false;
            timerCor = StartCoroutine(Timer(doorTime));
        }
    }

    IEnumerator Timer(float time) {
        if (time != 0) {
            for (int x = 0; x < time; x++) {
                this.GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(1f);
            }
        }
        door.GetComponent<doorScript>().open = false;
    }

    private void Update() {
        if (somethingOnButton) {
            if (timerCor != null) {
                StopCoroutine(timerCor);
            }
            door.GetComponent<doorScript>().open = true;
        }
    }
}
