using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using UnityEngine.Advertisements;
using System.Collections.Generic;


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
	private static int canChangeDayTime = 1;
	private float dayModify = 0.001f;
	private bool tapCheck;
	private float lastTap;

	// Use this for initialization
	void Start () {
		SetGameState (GameState.Start);
	}

	// Update is called once per frame
	void Update () {

//		GameObject.Find("FPS").GetComponent<Text>().text = "FPS: " + (1.0f / Time.deltaTime).ToString();      

//		Color color = GameObject.Find ("BeginMessage").GetComponent<Text> ().material.color;
//		color.a = Mathf.PingPong (Time.time * 5.0f, 1.0f);
//		GameObject.Find("BeginMessage").GetComponent<Text>().material.color = color;
			

		CalculateScore ();
		TapCheck ();

		if (Camera.main.GetComponent<ContrastStretch> ().limitMinimum >= 1 || Camera.main.GetComponent<ContrastStretch> ().limitMinimum <= 0) 
			canChangeDayTime *= -canChangeDayTime;
	
		Camera.main.GetComponent<ContrastStretch> ().limitMinimum += (dayModify * canChangeDayTime);
	
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
				case GameState.Resume :{
					break;
				}
				case GameState.Pause :{
					break;
				}
				case GameState.Over :{	
					Application.LoadLevel("MainGame");
					if(Advertisement.IsReady()){
						Advertisement.Show();
					}
					break;
				}
			}

		else if ((Input.GetKeyDown(KeyCode.Space) || touchClicked)) {

			tapCheck = true;
			lastTap = Time.time;

			switch (GetGameState()) {
				case GameState.Start : {
					this.SetGameState(GameState.Resume);
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
				break;
			}
			case GameState.Over :{
				
				Time.timeScale = 0.5f;
				break;
			}
		}

		PlatformContent.SendMessage ("SetCurrentState", CurrentGameState);
		Player.SendMessage ("SetCurrentState", CurrentGameState);
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
		
	}


}
