using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour {

	public static string nextLevelName = "Level1";
	public static int finalScore = 0;

	private static GameObject[] splashScreens;

	public TextMesh scoreBox;
	private int current = 0;

	// Use this for initialization
	void Start () {
		if (splashScreens == null) {
			splashScreens = new GameObject[transform.childCount];
			for (int i = 0; i < splashScreens.Length; ++i) {
				splashScreens[i] = transform.GetChild(i).gameObject;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			splashScreens[current].SetActive(false);
			++current;
			if (current == splashScreens.Length - 1) {
				Application.LoadLevel(nextLevelName);
			}
		}
	}
}
