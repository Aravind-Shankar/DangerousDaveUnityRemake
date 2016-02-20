using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform player;
	public float thresh = 0.25f;
	public int maxRightShifts = 2;
	public int maxLeftShifts = 2;

	private Camera camera;
 	private int i = 0;
 	void Start () {
        camera = GetComponent<Camera>();
    }
     
    void Update () {
    	Vector3 playerpos = player.position;
        if ( camera.WorldToScreenPoint(playerpos).x-camera.WorldToScreenPoint(transform.position).x >= thresh*camera.pixelWidth && i < maxRightShifts){
        	i++;
         	Vector3 campos = camera.WorldToScreenPoint(transform.position);
         	campos.x += (float)(1-2*thresh)*camera.pixelWidth;
         	transform.position = camera.ScreenToWorldPoint(campos);
        }
        else if(camera.WorldToScreenPoint(playerpos).x-camera.WorldToScreenPoint(transform.position).x <= -thresh*camera.pixelWidth && i > -maxLeftShifts){
         	i--;
         	Vector3 campos = camera.WorldToScreenPoint(transform.position);
         	campos.x -= (float)(1-2*thresh)*camera.pixelWidth;
         	transform.position = camera.ScreenToWorldPoint(campos);
        }
         
    }
}
