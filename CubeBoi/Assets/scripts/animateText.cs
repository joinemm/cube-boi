using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animateText : MonoBehaviour {

	void FixedUpdate () {
        Text text = this.GetComponent<Text>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, (Mathf.PingPong(Time.time * 2f, 1.0f)));
    }
}
