using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject BackgroundContent;

	// Use this for initialization
	void Start () {

		if (PlayerPrefs.GetInt ("Sound") == 0) {
			GameObject.Find ("Som").GetComponent<Toggle> ().isOn = true;
			AudioListener.volume = 0;
		} else {
			GameObject.Find ("Som").GetComponent<Toggle> ().isOn = false;
			PlayerPrefs.SetInt ("Sound", 1);
			PlayerPrefs.Save();
		}

		BackgroundContent.BroadcastMessage ("SetCurrentState", GameManager.GameState.Resume);

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void OnPlayClick () {
		this.StartCoroutine(this.CallGame());
	}


	IEnumerator CallGame () {
		float fadeTime = GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel("MainGame");
	}

	public void OnToggleSound() {
	
		if (GameObject.Find ("Som").GetComponent<Toggle> ().isOn) {
			PlayerPrefs.SetInt ("Sound", 0);
			PlayerPrefs.Save();
			AudioListener.volume = 0;
		
		}else {
			PlayerPrefs.SetInt ("Sound", 1);
			PlayerPrefs.Save();
			AudioListener.volume = 1;
		}

	}
}
