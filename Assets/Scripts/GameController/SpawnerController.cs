using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerController : MonoBehaviour {

	public int Speed;
	private int lastSpeed;

	private float nextActionTime = 0.0f;

	private float delayInstantiate = 0.1f;  
	
	public List<GameObject> SpawnObjects;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Transform lastChild = transform.GetChild (transform.childCount - 1);

		if (lastChild.position.x + 2 - transform.position.x  < 0 ) {
			Object NewObject = Instantiate (SpawnObjects [0], lastChild.position + new Vector3(2 - delayInstantiate, -lastChild.position.y + Random.Range(0,3) -2, 0), transform.rotation);
			((GameObject)NewObject).transform.parent = transform;
			((GameObject)NewObject).transform.SendMessage ("SetSpeed", Speed);
			
		}
	}


	void FixedUpdate () {

				foreach (Transform child in transform)
				{
					child.SendMessage("SetSpeed", Speed);
				}

	}	
}
