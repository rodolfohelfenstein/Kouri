using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerController : MonoBehaviour {
	public GameManager.GameState CurrentGameState;

	public int Speed;
	private int lastSpeed;
	private float nextActionTime = 0.0f;
	private float delayInstantiate = 0.1f;  
	public List<GameObject> SpawnObjects;


	[System.Serializable]
	public struct PlatformType {
		[Range(0, 100)]
		public int Percent;
		[Range(0, 6)]
		public int Width;
		[Range(-1, 4)]
		public int Height;
	}


	public PlatformType[] PlatformsTypes;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
			Transform lastChild = transform.GetChild (transform.childCount - 1);

			int auxBeginPercent = 0;
			int auxEndPercent = 0;
			int randomValue = Random.Range (0, 100);

			PlatformType randomType = new PlatformType ();

			foreach (PlatformType PlatformType in PlatformsTypes) {
				auxEndPercent += PlatformType.Percent;

				if (randomValue >= auxBeginPercent && auxBeginPercent < auxEndPercent)
					randomType = PlatformType;

				auxBeginPercent += PlatformType.Percent;
			}



			if (lastChild.position.x + 2 - transform.position.x < 0) {
				GameObject NewObject = Instantiate (SpawnObjects [0], lastChild.position + new Vector3 (2 + randomType.Width, -lastChild.position.y - (Speed > 0 ? 4.5f : -4.5f) + randomType.Height, 0), transform.rotation) as GameObject;

				if(Random.Range(0, 100) < 10 && CurrentGameState == GameManager.GameState.Resume)
					NewObject.transform.GetChild(NewObject.transform.childCount - 1).gameObject.SetActive(true);

				int randomGravityValue = Random.Range (0, 100);
		
				NewObject.transform.parent = transform;
				NewObject.transform.SendMessage ("SetSpeed", Speed);
			}
	}


	void FixedUpdate () {
			foreach (Transform child in transform) {
				child.SendMessage ("SetSpeed", Speed);
			}
	}


	public void SetSpeed(int pSpeed){
		this.Speed += pSpeed;
	}

	public void SetCurrentState(GameManager.GameState pGameState){
		this.CurrentGameState = pGameState;
	}

}
