using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooterScript : MonoBehaviour {

    public GameObject bullet;
    public bool oldDestroyed = true;
    public Vector2 direction;
    float speed = 10f;

	// Update is called once per frame
	void Update () {
		if (oldDestroyed) {
            StartCoroutine(Shoot());
            oldDestroyed = false;
        }
    }

    IEnumerator Shoot() {
        yield return new WaitForSeconds(0.5f);
        GameObject shotBullet = Instantiate(bullet, this.transform.position, Quaternion.identity);
        shotBullet.GetComponent<bulletScript>().myShooter = this.gameObject;
        shotBullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
