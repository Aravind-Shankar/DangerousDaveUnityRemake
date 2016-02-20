using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private bool canJump = false, grounded = true;
	private bool facingRight = true;
	private Transform groundCheck;
	private Vector2 groundInitPosition;

	public float moveForce = 10f;
	public float maxSpeed = 10f;
	public float jumpForce = 300f;

	void Awake () {
		groundCheck = transform.Find ("ground_check");
		//groundInitPosition = groundCheck.localPosition;
	}

	void Update() {
		//groundCheck.localPosition = groundInitPosition;
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground Layer"));
		canJump = (Input.GetButton("Jump") && grounded);
	}
	
	void FixedUpdate () {
		float horiz = Input.GetAxis ("Horizontal");
		if (horiz * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			GetComponent<Rigidbody2D> ().AddForce (Vector2.right * horiz * moveForce);
		if (Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x) > maxSpeed)
			GetComponent<Rigidbody2D> ().velocity = Vector2.right * Mathf.Sign (GetComponent<Rigidbody2D> ().velocity.x) * maxSpeed;
		if (canJump) {
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
			canJump = false;
		}
	}
}
