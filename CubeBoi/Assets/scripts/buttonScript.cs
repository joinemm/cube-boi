using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour {

    public GameObject door;
    public float doorTime;
    Coroutine timerCor;
    bool somethingOnButton = false;
    AudioSource[] aSources;

    private void Start() {
        aSources = this.GetComponents<AudioSource>();
    }

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
                if (x % 2 == 0) {
                    aSources[0].Play();
                } else {
                    aSources[1].Play();
                }
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
