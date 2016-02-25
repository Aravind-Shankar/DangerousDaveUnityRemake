using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour {
	
//	public GameObject player;
//	public Transform spawnPoint;
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
			if (!other.gameObject.GetComponent<PlayerController>().DieAndCheck())
				Application.Quit();
			else
				StartCoroutine(other.gameObject.GetComponent<PlayerController>().Respawn(
					other.gameObject.GetComponent<PlayerController>().spawnPoint));
		}
	}

}
