using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsObject : MonoBehaviour {

    Vector2 resetPosition;

	void Start () {
        resetPosition = this.transform.position;
	}

    public void Reset() {
        this.transform.position = resetPosition;
    }
}
