using UnityEngine;
using System.Collections;


public class BackgroundScrolling : MonoBehaviour {
	public GameManager.GameState CurrentGameState;

	public float SpeedModify;
	public int Layer;
	public bool childInstantiate;
	public GameObject BackgroundContent;

	void Start() {

		childInstantiate = false;
	}
	
	void Update()
	{
	
		if (CurrentGameState == GameManager.GameState.Resume) {
			int Speed = GameObject.Find ("Platform Content").GetComponent<SpawnerController> ().Speed;
			transform.Translate (new Vector3 ((-Time.fixedDeltaTime) * (Speed * SpeedModify), 0, 0));


			if (transform.position.x < Random.Range (-30, -28) && !childInstantiate) {
				Object objectInstantiate = Instantiate (gameObject, new Vector3 (10, transform.position.y, Layer), transform.rotation);
				((GameObject)objectInstantiate).transform.parent = BackgroundContent.transform;

				childInstantiate = true;
			}

		
			if (transform.position.x < -50) {
				Destroy (gameObject);
			}

		}
	}

	public void SetCurrentState(GameManager.GameState pGameState){
		this.CurrentGameState = pGameState;
	}
}

