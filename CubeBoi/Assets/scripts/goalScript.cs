using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goalScript : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        FindObjectOfType<gameController>().StageClear();
    }
}
