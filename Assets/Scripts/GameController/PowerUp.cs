using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour {

	bool getPowerUp;
	// Use this for initialization
	void Start () {
		getPowerUp = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (getPowerUp && GameObject.Find ("Score")) {
			transform.parent = GameObject.Find ("Score").transform;
			transform.position = Vector3.MoveTowards (transform.position, GameObject.Find ("Score").transform.position, Time.deltaTime * 30);

		}

		if (GameObject.Find ("Score") != null)

		if (Vector3.Distance (transform.position, GameObject.Find ("Score").transform.position) < 1f) {

			Destroy (gameObject);
			GameObject.Find ("Score").GetComponent<Animator>().Play("Pulse");
			GameObject.Find ("Game Manager").SendMessage("SetScore");

			
		}

	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject == GameObject.Find ("Player")) {
			getPowerUp = true;
			GetComponent<AudioSource>().Play();
		}
	}



}
