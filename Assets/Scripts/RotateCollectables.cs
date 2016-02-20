using UnityEngine;
using System.Collections;

public class RotateCollectables : MonoBehaviour {

	public Vector3 rotationVector = new Vector3(0,0,45);

	void FixedUpdate () {
        transform.Rotate(rotationVector * Time.deltaTime);
    }
}
