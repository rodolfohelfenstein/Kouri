using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {

	[HideInInspector] public int Speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x + 2 - transform.parent.position.x < -30)
			Destroy (gameObject);
	}
	
	void SetSpeed (int pSpeed) {
		Speed = pSpeed;
	}

	void FixedUpdate (){
		transform.Translate (new Vector3 (-Time.fixedDeltaTime * Speed, 0, 0));
//		transform.position -= new Vector3 (Time.fixedDeltaTime * Speed, 0, 0);
	}

}
