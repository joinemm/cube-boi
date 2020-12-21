using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour {

    public bool open = false;

    private void Update() {
        this.GetComponent<Animator>().SetBool("open", open);
    }

    public void DustEffect() {
        foreach (Animator anim in GetComponentsInChildren<Animator>()) {
                anim.SetTrigger("dust");
        }
        this.GetComponent<AudioSource>().Play();
    }
}
