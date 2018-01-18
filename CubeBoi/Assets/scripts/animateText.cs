using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animateText : MonoBehaviour {

	void FixedUpdate () {
        this.GetComponent<Text>().color = Color.Lerp(Color.gray, Color.white, Mathf.PingPong(Time.time, 1));
    }
}
