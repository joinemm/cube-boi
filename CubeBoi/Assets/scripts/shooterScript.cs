using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooterScript : MonoBehaviour {

    public GameObject bullet;
    public bool oldDestroyed = true;
    Vector2 direction;
    float speed = 10f;

	// Use this for initialization
	void Start () {
        direction = new Vector2(-1, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (oldDestroyed) {
            Shoot();
            oldDestroyed = false;
        }
    }

    void Shoot() {
        GameObject shotBullet = Instantiate(bullet, this.transform.position, Quaternion.identity);
        shotBullet.GetComponent<bulletScript>().myShooter = this.gameObject;
        shotBullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
