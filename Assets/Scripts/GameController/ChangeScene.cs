using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(Application.loadedLevel == 0)
			this.StartCoroutine(this.CallMenu());
	}


	IEnumerator CallMenu () {
		yield return new WaitForSeconds (3.0f);

		float fadeTime = GameObject.Find("_GM").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel(Application.loadedLevel + 1); 
	}

	void Update () {

 	}
}
