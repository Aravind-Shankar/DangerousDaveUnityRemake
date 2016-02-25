using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	/**
	*	Script for getting input from the user and controlling the player.
	*
	*	This script has to be attached as a component to the player to control.
	*	Also the player object must have a child empty GameObject called "ground_check"
	*	vertically below it, so that the jump mechanism can work properly.
	*	This empty child must be close enough to the player object so that if and only if the player is grounded,
	*	the ground_check object also touches/goes into the ground.
	*
	*	See "TestCharacter.unity" for an example scene with a ball as the player.
	*/

	private static int points = 0;
	private static int lives = Constants.MAX_LIVES;

	private bool canJump = false;
	private bool grounded = true;
	private bool facingRight = true;
	private bool gotTrophy = false;
	private Transform groundCheck;
	private Vector3 groundCheckRelativePosition;
    private int groundLayerMask;

	public float moveForce = 10f;
	public float maxSpeed = 10f;
	public float jumpForce = 300f;
	public string nextLevelName = "Level1";
    public TextMesh score;
	public TextMesh trophyMessageBox;
	public TextMesh lifeCountBox;

	public static bool DieAndCheck(GameObject player) {
		if (lives > 0) {
			--lives;
			player.GetComponent<PlayerController> ().UpdateLives ();
			return (lives > 0);
		} else
			return false;
	}

    void Start() {
//		if (score == null || trophyMessageBox == null)
//		{
//			score = PlayerRespawn.tempscore;
//			trophyMessageBox = PlayerRespawn.temptrophy;
//		}
//		else
//		{
//			PlayerRespawn.tempscore = score;
//			PlayerRespawn.temptrophy = trophyMessageBox;
//		}
		groundCheck = transform.Find ("ground_check");
		groundCheckRelativePosition = transform.position - groundCheck.position;
        groundLayerMask = (1 << LayerMask.NameToLayer ("Ground Layer"));
        UpdateScore();
		UpdateLives ();
    }

	void Update() {
		groundCheck.position = transform.position - groundCheckRelativePosition;
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, groundLayerMask);
		if (Input.GetButtonDown ("Jump") && grounded)
			canJump = true;
	}
	
	void FixedUpdate () {
		float horiz = Input.GetAxis ("Horizontal");
		if (horiz * GetComponent<Rigidbody2D> ().velocity.x < maxSpeed)
			GetComponent<Rigidbody2D> ().AddForce (Vector2.right * horiz * moveForce);
		if (Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x) > maxSpeed)
			GetComponent<Rigidbody2D> ().velocity = new Vector2(Mathf.Sign (GetComponent<Rigidbody2D> ().velocity.x) * maxSpeed,
			                                                    GetComponent<Rigidbody2D>().velocity.y);
		if ((horiz < 0 && facingRight) || (horiz > 0 && !facingRight))
			Flip ();
		if (canJump) {
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
			canJump = false;
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag ("Pick up")) {
			other.gameObject.SetActive (false);
			UpdateScore (Constants.POINTS_DUMMY_PICKUP);
		} else if (other.gameObject.CompareTag("White Gem Pickup")) {
			other.gameObject.SetActive (false);
			UpdateScore (Constants.POINTS_WHITE_GEM);
		} else if (other.gameObject.CompareTag("Red Gem Pickup")) {
			other.gameObject.SetActive (false);
			UpdateScore (Constants.POINTS_RED_GEM);
		} else if (other.gameObject.CompareTag("Pink Ball Pickup")) {
			other.gameObject.SetActive (false);
			UpdateScore (Constants.POINTS_PINK_BALL);
		} else if (other.gameObject.CompareTag ("Trophy")) {
			other.gameObject.SetActive(false);
			gotTrophy = true;
			trophyMessageBox.gameObject.SetActive(true);
			UpdateScore(Constants.POINTS_TROPHY);
		} else if (other.gameObject.CompareTag ("Door")) {
			if (gotTrophy) {
				UpdateScore(Constants.POINTS_DOOR);
				Application.LoadLevel(nextLevelName);
			}
		}
    }

	void UpdateScore() {
		score.text = "Score: " + points.ToString();
	}

	void UpdateLives() {
		lifeCountBox.text = "x " + lives.ToString();
	}
	
	void UpdateScore(int gainedPoints) {
		points += gainedPoints;
		UpdateScore ();
	}

	void Flip() {
		Vector3 newScale = transform.localScale;
		newScale.x *= -1;
		transform.localScale = newScale;
		facingRight = !facingRight;
	}
}
