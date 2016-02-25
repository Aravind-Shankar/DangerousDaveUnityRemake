using UnityEngine;
using System.Collections;

public class WormholeController : MonoBehaviour {

	public float wormholeDelaySeconds = 2.0f;	// MUST BE greater than player respawn time
	public GameObject player;

	private GameObject point1, point2;

	// Use this for initialization
	void Start () {
		point1 = transform.Find ("Wormhole - Point 1").gameObject;
		point2 = transform.Find ("Wormhole - Point 2").gameObject;
	}

	public void EnterWormhole(GameObject entryPoint) {
		Transform exitPoint = ((entryPoint.name.Equals (point1.name)) ? point2 : point1).transform;
		StartCoroutine(player.GetComponent<PlayerController> ().Respawn (exitPoint));
		player.GetComponent<PlayerController>().Disappear();
		StartCoroutine (ReEnableWormhole ());
		point1.GetComponent<Renderer> ().enabled = false;
		point2.GetComponent<Renderer> ().enabled = false;
		point1.GetComponent<Collider2D> ().enabled = false;
		point2.GetComponent<Collider2D> ().enabled = false;
	}
	
	IEnumerator ReEnableWormhole() {
		yield return new WaitForSeconds(wormholeDelaySeconds);
		point1.GetComponent<Renderer> ().enabled = true;
		point2.GetComponent<Renderer> ().enabled = true;
		point1.GetComponent<Collider2D> ().enabled = true;
		point2.GetComponent<Collider2D> ().enabled = true;
	}
}
