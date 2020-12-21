using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserScript : MonoBehaviour {

    LineRenderer lr;
    public GameObject lightning;
    public LayerMask tileMask;
    LightningBoltScript lightScript;
    public bool moving;
    bool goingRight = true;
    bool laserAllowed = false;
    float speed = 2f;
    RaycastHit2D laserHit;

    GameObject laserGO;
    public GameObject laserParticle;
    public GameObject laserParticleLoop;

    // Use this for initialization
    void Start () {
        lr = GetComponent<LineRenderer>();
        lightScript = lightning.GetComponent<LightningBoltScript>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!moving) {
            transform.Rotate(transform.forward, 0.2f);
        }
        if (laserGO != null) {
            laserGO.transform.position = laserHit.point;
        } else {
            laserGO = Instantiate(laserParticleLoop, (Vector3)laserHit.point-transform.forward, Quaternion.identity);
        }
    }

    private void Update() {
        if (moving) {
            RaycastHit2D sensorFront = Physics2D.Raycast(transform.position + transform.right * 0.4f + transform.up * 0.5f, transform.right, 0.12f, tileMask);
            RaycastHit2D sensorBack = Physics2D.Raycast(transform.position - transform.right * 0.4f + transform.up * 0.5f, -transform.right, 0.12f, tileMask);
            if ((sensorFront && goingRight) || (sensorBack && !goingRight)) {
                //issa square
                speed = speed * -1;
                goingRight = !goingRight;
            }
            GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        }
        laserHit = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, tileMask);
        /*
        lr.SetPosition(0, transform.position - transform.forward);
        lr.SetPosition(1, (Vector3)laserHit.point-transform.forward);
        if (laserAllowed) {
            StartCoroutine(ParticleTime(laserHit.point));
        }
        */
        lightScript.StartPosition = lightScript.transform.InverseTransformPoint(transform.position - transform.forward + transform.up*0.5f);
        lightScript.EndPosition = lightScript.transform.InverseTransformPoint((Vector3)laserHit.point - transform.forward);
        if (laserHit.collider.gameObject.tag == "Player") {
            FindObjectOfType<gameController>().Respawn("laser");
        }
    }

    IEnumerator ParticleTime(Vector3 point) {
        laserGO = Instantiate(laserParticle, point, Quaternion.identity);
        laserAllowed = false;
        yield return new WaitForSeconds(Random.Range(0, 1));
        laserAllowed = true;
        yield return new WaitForSeconds(.25f);
        Destroy(laserGO);
    }
}