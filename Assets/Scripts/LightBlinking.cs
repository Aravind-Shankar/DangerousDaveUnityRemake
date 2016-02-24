using UnityEngine;
using System.Collections;

public class LightBlinking : MonoBehaviour {
	
	public float brightTimeSeconds = 0.5f;
	public float darkTimeSeconds = 0.5f;
	
	private Light light;
	
	// Use this for initialization
	void Start () {
		light = GetComponent<Light> ();
		StartCoroutine (BlinkLight ());
	}
	
	void Update() {
	}
	
	IEnumerator BlinkLight() {
		while (true) {
			light.intensity = 8;
			yield return new WaitForSeconds(brightTimeSeconds);
			light.intensity = 1;
			yield return new WaitForSeconds(darkTimeSeconds);
		}
	}
	
}
