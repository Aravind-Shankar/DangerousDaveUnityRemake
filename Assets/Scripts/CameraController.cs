using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Transform player;
	Camera camera;
 	int i = 0;
 	int maxProg = 2;
 	int minProg = 0;
 	float thresh = 0.25f;
     void Start () {
         player = GameObject.Find ("Player").transform;
         camera = GetComponent<Camera>();
     }
     
     void Update () {
     	 Vector3 playerpos = player.position;
         if ( camera.WorldToScreenPoint(playerpos).x-camera.WorldToScreenPoint(transform.position).x >= thresh*camera.pixelWidth && i < maxProg){
         	i++;
         	Vector3 campos = camera.WorldToScreenPoint(transform.position);
         	campos.x += (float)(1-2*thresh)*camera.pixelWidth;
         	transform.position = camera.ScreenToWorldPoint(campos);
         }
         else if(camera.WorldToScreenPoint(playerpos).x-camera.WorldToScreenPoint(transform.position).x <= -thresh*camera.pixelWidth && i > minProg){
         	i--;
         	Vector3 campos = camera.WorldToScreenPoint(transform.position);
         	campos.x -= (float)(1-2*thresh)*camera.pixelWidth;
         	transform.position = camera.ScreenToWorldPoint(campos);
         }
         
     }
}
