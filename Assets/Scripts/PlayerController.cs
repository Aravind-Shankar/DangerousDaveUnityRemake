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
	private static int lives = Constants.START_LIVES;

	private bool canJump = false;
	private bool grounded = false;
	private bool facingRight = true;
	private bool gotTrophy = false;
	private Transform groundCheckLeft, groundCheckRight;
	private Transform radarPlayer;
//	private Vector3 groundCheckRelativePosition;
    private int groundLayerMask;

//	public float moveForce = 10f;
	public float horizontalSpeed = 3.5f;
	public float jumpSpeed = 8f;
	public float respawnDelaySeconds = 1.0f;
	public string nextLevelName = "Level1";
    public TextMesh score;
	public TextMesh trophyMessageBox;
	public TextMesh lifeCountBox;
	public GameObject door;
	public Transform spawnPoint;

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
		radarPlayer = transform.Find("RadarPlayer");
		groundCheckLeft = transform.Find ("Ground Check Left");
		groundCheckRight = transform.Find ("Ground Check Right");
//		groundCheckRelativePosition = transform.position - groundCheck.position;
        groundLayerMask = (1 << LayerMask.NameToLayer ("Ground Layer"));
        UpdateScore();
		UpdateLives ();
    }

	void Update() {
//		groundCheck.position = transform.position - groundCheckRelativePosition;
		grounded = Physics2D.Linecast(transform.position, groundCheckLeft.position, groundLayerMask) ||
			Physics2D.Linecast(transform.position, groundCheckRight.position, groundLayerMask);
		if (Input.GetButtonDown ("Jump") && grounded)
			canJump = true;
	}
	
	void FixedUpdate () {
		float horiz = Input.GetAxis ("Horizontal");
		Vector2 newVelocity = GetComponent<Rigidbody2D> ().velocity;
		newVelocity.x = (horiz == 0.0f) ? 0.0f : Mathf.Sign (horiz) * horizontalSpeed;
		if ((horiz < 0 && facingRight) || (horiz > 0 && !facingRight))
			Flip ();
		if (canJump) {
			newVelocity.y = jumpSpeed;
			canJump = false;
		}
		GetComponent<Rigidbody2D> ().velocity = newVelocity;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
		GameObject otherObject = other.gameObject;
        if (otherObject.CompareTag ("Pick up")) {
			otherObject.SetActive (false);
			UpdateScore (Constants.POINTS_DUMMY_PICKUP);
		} else if (otherObject.CompareTag ("White Gem Pickup")) {
			otherObject.SetActive (false);
			UpdateScore (Constants.POINTS_WHITE_GEM);
		} else if (otherObject.CompareTag ("Red Gem Pickup")) {
			otherObject.SetActive (false);
			UpdateScore (Constants.POINTS_RED_GEM);
		} else if (otherObject.CompareTag ("Pink Ball Pickup")) {
			otherObject.SetActive (false);
			UpdateScore (Constants.POINTS_PINK_BALL);
		} else if (otherObject.CompareTag("Extra Life Pickup")) {
			otherObject.SetActive(false);
			++lives;
			UpdateLives();
			UpdateScore(Constants.POINTS_EXTRA_LIFE);
		} else if (otherObject.CompareTag("Wormhole Point")) {
			otherObject.GetComponentInParent<WormholeController>().EnterWormhole(otherObject);
		} else if (otherObject.CompareTag("Checkpoint")) {
			this.spawnPoint = otherObject.transform;
			otherObject.GetComponent<SpriteRenderer>().color = Color.green;
			otherObject.GetComponent<Collider2D>().enabled = false;
		} else if (otherObject.CompareTag ("Trophy")) {
			otherObject.SetActive(false);
			gotTrophy = true;
			trophyMessageBox.gameObject.SetActive(true);
			door.GetComponent<Renderer>().enabled = true;
			UpdateScore(Constants.POINTS_TROPHY);
		} else if (otherObject.CompareTag ("Door")) {
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
		newScale = radarPlayer.localScale;
		newScale.x *= -1;
		radarPlayer.localScale = newScale;
		facingRight = !facingRight;
	}

	public void Initialize() {
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		canJump = false;
		grounded = false;
		if (!facingRight)
			Flip ();
	}

	public void Disappear() {
		gameObject.GetComponent<Renderer> ().enabled = false;
		radarPlayer.gameObject.GetComponent<Renderer> ().enabled = false;
		gameObject.GetComponent<Collider2D> ().enabled = false;
	}

	public bool DieAndCheck() {
		Disappear ();
		if (lives > 0) {
			--lives;
			UpdateLives ();
			return (lives > 0);
		} else
			return false;
	}
	
	public IEnumerator Respawn(Transform spawnPoint) {
		yield return new WaitForSeconds(respawnDelaySeconds);
		radarPlayer.gameObject.GetComponent<Renderer> ().enabled = true;
		gameObject.GetComponent<Renderer> ().enabled = true;
		gameObject.GetComponent<Collider2D> ().enabled = true;
		transform.position = spawnPoint.transform.position;
		Initialize();
	}
}
