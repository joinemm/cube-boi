using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {
    //booleans
    bool boostEnabled = false;
    [System.NonSerialized] public bool isGrounded = false;
    [System.NonSerialized] public bool boosted = false;
    [System.NonSerialized] public bool facingRight = true;
    //numeral variables
    [System.NonSerialized] public float moveSpeed = 7f;
    [System.NonSerialized] public float moveSpeedAir = 750f;
    [System.NonSerialized] public float jumpForce = 10f;
    [System.NonSerialized] public float fallMultiplier = 2.5f;
    //gameobject references
    Rigidbody2D rb;
    public LayerMask whatIsGround;
    public LayerMask wallJumpMask;
    public gameController gameController;
    public GameObject groundCheck;

    public string hitRight;
    public string hitLeft;

    bool canJump = true;

    void Start () {
        //set references on spawn
        rb = this.GetComponent<Rigidbody2D>();
        gameController = FindObjectOfType<gameController>();
        groundCheck = this.GetComponentInChildren<fixRotation>().gameObject;     
    }
	
	void Update () {
        Vector2 pos = this.transform.position;
        //collision method groundcheck
        if (groundCheck != null) {
            isGrounded = groundCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(whatIsGround);
        }
        //walljump
        RaycastHit2D hitWallRight = Physics2D.Raycast(this.transform.position,Vector2.right,0.5f,wallJumpMask);
        if (hitWallRight == true) {
            hitRight = hitWallRight.collider.gameObject.name;
        }
        RaycastHit2D hitWallLeft = Physics2D.Raycast(this.transform.position, Vector2.left, 0.5f, wallJumpMask);
        if (hitWallLeft == true) {
            hitLeft = hitWallLeft.collider.gameObject.name;
        }
        //set falling velocity multiplier
        if (rb.velocity.y < 1 && !hitWallLeft && !hitWallRight) {
            rb.gravityScale = fallMultiplier;
        } else {
            rb.gravityScale = 1f;
        }
        //horiaontal input -> move
        if (Input.GetAxis("Horizontal") != 0) {
            Move(Input.GetAxisRaw("Horizontal"));
        }
        //jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && canJump) {
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(JumpCooldown());
        }
        //walljump
        if (hitWallRight && Input.GetKeyDown(KeyCode.Space) && canJump) {
            rb.AddForce(new Vector2(-1,1) * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(JumpCooldown());
        }
        if (hitWallLeft && Input.GetKeyDown(KeyCode.Space) && canJump) {
            rb.AddForce(new Vector2(1, 1) * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(JumpCooldown());
        }
        //fixrotation
        if (Input.GetKeyDown(KeyCode.Q)) {
            FixRotation();
        }
        //boost
        if (Input.GetKeyDown(KeyCode.LeftShift) && boosted == false && boostEnabled) {
            Boost();
        }
	}

    IEnumerator BoostCooldown() {
        yield return new WaitForSeconds(3f);
        boosted = false;
    }

    IEnumerator JumpCooldown() {
        canJump = false;
        yield return new WaitForSeconds(0.05f);
        canJump = true;
    }

    void Move(float movementAmount) {
        if (!isGrounded) {
            rb.AddForce(new Vector2(((movementAmount * moveSpeedAir) / (Mathf.Abs(rb.velocity.x) + 1))*Time.deltaTime*3f,0));
        } else {
            rb.velocity = new Vector2(movementAmount * moveSpeed, rb.velocity.y);
        }
    }
    
    public void FixRotation() {
        //try make cube stand on one of it's sides
        float rot = this.transform.rotation.z;
        if (rot != 0f) {
            var desiredRotQ = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
            this.transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * 10f);
        }
    }

    void Boost() {
        //double the current velocity
        rb.velocity = rb.velocity * 2;
        boosted = true;
        StartCoroutine(BoostCooldown());
    }
}
