using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour {
	
	public GameObject player;
	public Transform spawnpoint;
//	static public TextMesh tempscore;
//	static public TextMesh temptrophy;
	
	void OnTriggerEnter2D(Collider2D other)
	{
//		GameObject pl = other.gameObject;
//		Destroy(pl);
//		GameObject P = Instantiate(player, spawnpoint.position, Quaternion.identity) as GameObject;
//		var cam = Camera.main.GetComponent<CameraController>();
//		cam.player = P.transform;
		if (other.CompareTag ("Player")) {
			other.gameObject.transform.position = spawnpoint.transform.position;
			other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}
}
