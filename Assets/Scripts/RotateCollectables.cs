using UnityEngine;
using System.Collections;

public class RotateCollectables : MonoBehaviour {

	void Update ()
    {
        transform.Rotate(new Vector3(0,0,15) * Time.deltaTime * 3);
    }
}
