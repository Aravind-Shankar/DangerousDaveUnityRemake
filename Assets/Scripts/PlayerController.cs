using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private bool canJump = false;
	private Transform groundCheck;

	public float moveForce = 5f;
	public float jumpForce = 50f;

	// Use this for initialization
	void Start () {
		groundCheck = transform.Find ("ground_check");
	}

	void Update() {
		groundCheck.transform.rotation.eulerAngles = Vector3.zero;
		canJump = (Input.GetButtonDown("Jump") &&
		        Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground Layer"))
		        );
	}
	
	void FixedUpdate () {
		float horiz = Input.GetAxis ("Horizontal") * moveForce, vert = (canJump) ? jumpForce : 0;
		this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (horiz, vert));
	}
}
