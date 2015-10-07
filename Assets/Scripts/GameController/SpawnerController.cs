using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerController : MonoBehaviour {

	public int Speed;
	private int lastSpeed;

	private float nextActionTime = 0.0f;

	private float delayInstantiate = 0.1f;  
	
	public List<GameObject> SpawnObjects;


	[System.Serializable]
	public struct PlatformType {
		[Range(0, 100)]
		public int Percent;
		public int Width;
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

		PlatformType randomType = new PlatformType();

		foreach (PlatformType PlatformType in PlatformsTypes)
		{
			auxEndPercent += PlatformType.Percent;

			if(randomValue >= auxBeginPercent && auxBeginPercent < auxEndPercent)
				randomType = PlatformType;

			auxBeginPercent += PlatformType.Percent;
		}

		if (lastChild.position.x + 2 - transform.position.x  < 0 ) {
			Object NewObject = Instantiate (SpawnObjects [0], lastChild.position + new Vector3( 2 + randomType.Width, -lastChild.position.y - 3.5f + randomType.Height, 0), transform.rotation);
			((GameObject)NewObject).transform.parent = transform;
			((GameObject)NewObject).transform.SendMessage ("SetSpeed", Speed);
			
		}

		if(GUI.changed)
		{
			Debug.Log ("changed");
		}

	}


	void FixedUpdate () {

				foreach (Transform child in transform)
				{
					child.SendMessage("SetSpeed", Speed);
				}

	}	
}
