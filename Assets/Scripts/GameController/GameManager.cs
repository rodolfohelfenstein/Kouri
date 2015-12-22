using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour {

	public enum GameState {Start, Resume, Pause, Over}


	public GameObject PlatformContent;
	public GameObject Player;
	public GameObject BackgroundContent;

	[System.Serializable]
	public struct GameStateStruct {
		public GameState GameStateValue;
		public GameObject GameObjectValue;
	}
	public GameStateStruct[] GameStates;

	private GameState CurrentGameState;




	private Text ScoreText;
	private int Score; 
	private float interval = 0.01f; 
	private float nextTime = 0;
	private bool tapCheck;
	private float lastTap;

	// Use this for initialization
	void Start () {
		SetGameState (GameState.Start);
	}

	// Update is called once per frame
	void Update () {

		if (GetGameState() == GameState.Resume)
			GameObject.Find("FPS").GetComponent<Text>().text = "FPS: " + (1.0f / Time.deltaTime).ToString();      

		CalculateScore ();
		TapCheck ();

	}

	public void TapCheck () {


		bool touchClicked = false;
		for (int i = 0; i < Input.touches.Length; i++) {
			touchClicked = Input.touches[i].phase == TouchPhase.Began;
		} 

		if ((Input.GetKeyDown (KeyCode.Space) || touchClicked) && tapCheck)
	
		switch (GetGameState()) {
				case GameState.Start : {
					break;
				}
				case GameState.Resume : {
					break;
				}
				case GameState.Pause :{
					break;
				}
				case GameState.Over :{	
					this.StartCoroutine(this.CallGame());
					if(Advertisement.IsReady()){

						if (Random.Range(0,10) < 2)
							Advertisement.Show();
					}
					Analytics.CustomEvent("gameOver", new Dictionary<string, object>
					                      {
						{ "finishScore", Score }
					});
					break;
				}
			}

		else if ((Input.GetKeyDown(KeyCode.Space) || touchClicked)) {

			tapCheck = true;
			lastTap = Time.time;

			switch (GetGameState()) {
				case GameState.Start : {
					this.SetGameState(GameState.Resume);
					Player.SetActive(true);
					break;
				}
				case GameState.Resume :{
					break;
				}
				case GameState.Pause :{
					break;
				}
				case GameState.Over :{		
					break;
				}
			}

		}

		if (Time.time - lastTap > 0.4f) tapCheck = false;
	}

	public GameState GetGameState (){
		return CurrentGameState;
	}

	public void SetGameState(GameState pGameState){

		SetUIObject(pGameState);
		CurrentGameState = pGameState;
	
		switch (pGameState) {
			case GameState.Start : {
				Time.timeScale = 1.0f;
				break;
			}
			case GameState.Resume :{
				Time.timeScale = 1.0f;
				break;
			}
			case GameState.Pause :{
				Time.timeScale = 0.0f;
				if (PlayerPrefs.GetInt ("Sound") == 0)
					GameObject.Find ("Som").GetComponent<Toggle> ().isOn = true;
				else {
					GameObject.Find ("Som").GetComponent<Toggle> ().isOn = false;
					PlayerPrefs.SetInt ("Sound", 1);
					PlayerPrefs.Save();
				}
				break;
			}
			case GameState.Over :{
				if(PlayerPrefs.GetInt("HighScore") < Score){
					PlayerPrefs.SetInt("HighScore", Score);
					PlayerPrefs.Save();
					GameObject.Find("HighScore").GetComponent<Text>().text = "NEW HIGHSCORE: " + Score.ToString();
				} else {
				GameObject.Find("HighScore").GetComponent<Text>().text = "HIGHSCORE: " + PlayerPrefs.GetInt("HighScore").ToString();
				}

				GameObject.Find("FinishScore").GetComponent<Text>().text = "SCORE: " + Score.ToString();
				Time.timeScale = 0.5f;
				break;
			}
		}

		PlatformContent.SendMessage ("SetCurrentState", CurrentGameState);
//		Player.SendMessage ("SetCurrentState", CurrentGameState);
		BackgroundContent.BroadcastMessage ("SetCurrentState", CurrentGameState);


	}

	private void SetUIObject(GameState pGameState) {
		for (int i = 0; i < GameStates.Length; i++) {
			GameStates[i].GameObjectValue.SetActive(GameStates[i].GameStateValue == pGameState);
		}
	}

	private void CalculateScore(){
		if (Time.time >= nextTime && GetGameState() == GameState.Resume) {

			GameObject.Find("Score").GetComponent<Text>().text = Score++.ToString();

			if (Score % 1000 == 0)
				PlatformContent.SendMessage("SetSpeed", +1);

			nextTime += interval; 
		}
	}

	public void OnPause(){
		SetGameState (GameState.Pause);
	}

	public void OnResume(){
		SetGameState (GameState.Resume);
	}

	public void OnMenu(){
		this.StartCoroutine(this.CallMenu());
		Time.timeScale = 1.0f;
		BackgroundContent.BroadcastMessage ("SetCurrentState", GameState.Resume);
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

	IEnumerator CallGame () {
		float fadeTime = GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel("MainGame");
	}

	IEnumerator CallMenu () {
		float fadeTime = GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel("MainMenu");
	}


	public void SetScore(){
		this.Score += 100;
	}

}
