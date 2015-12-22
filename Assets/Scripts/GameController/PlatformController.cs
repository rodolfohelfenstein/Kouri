using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {


	[HideInInspector] public int Speed;
	[HideInInspector] public int GravitySide;

	// Use this for initialization
	void Start () {
		GravitySide = 1;
	}
	
	// Update is called once per frame
	void Update () {
			if (transform.position.x + 2 - transform.parent.position.x < -30)
				Destroy (gameObject);
	}
	
	void SetSpeed (int pSpeed) {
		Speed = pSpeed;
	}

	void SetGravitySide (int pGravitySide) {
		GravitySide = pGravitySide;
	}

	void FixedUpdate (){
			transform.Translate (new Vector3 ((-Time.fixedDeltaTime * GravitySide) * Speed, 0, 0));
	}


}
