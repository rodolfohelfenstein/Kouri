using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class PlayerController : MonoBehaviour {

	public GameManager.GameState CurrentGameState;

	public Transform groundCheck;
	public Transform sideCheck;
	private bool grounded;
	private RaycastHit2D sideBlock;

	public PhysicsMaterial2D normalMaterial;
	public PhysicsMaterial2D iceMaterial;
	public GameObject explosionPrefab;
	public GameObject explosionPlatformPrefab;

	private Rigidbody2D rigidBody2D;
	private Renderer renderer2D;


	private Vector3 beginPosition;

	private int jumpCount;
	// Use this for initialization
	void Awake () 
	{
		renderer2D = GetComponent<Renderer> ();
		rigidBody2D = GetComponent<Rigidbody2D> ();
		beginPosition = transform.position;
		jumpCount = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!renderer2D.isVisible) {
			Instantiate (explosionPrefab, transform.position, transform.rotation);
			Instantiate (explosionPrefab, transform.position, transform.rotation);
			explosionPrefab.GetComponent<AudioSource> ().Play ();
			Destroy (gameObject);
			GameObject.Find ("Game Manager").SendMessage ("SetGameState", GameManager.GameState.Over);
		}
	}


	public void FixedUpdate () {
	
		grounded =  Physics2D.Raycast (groundCheck.position, new Vector2 (0, -1), 0.0f, 1 << LayerMask.NameToLayer ("Ground"));
		sideBlock = Physics2D.Raycast (sideCheck.position, new Vector2 (1, 0), 0.1f, 1 << LayerMask.NameToLayer ("Ground"));

		if (grounded) {
			jumpCount = 1;
		}
		if (sideBlock.collider != null) {
			Vector2 pos = transform.position;
			pos.x = sideCheck.transform.position.x - 2f;
			transform.position = pos;

			Destroy(sideBlock.collider.gameObject);
			Instantiate (explosionPlatformPrefab, transform.position, transform.rotation);
		} 	

		if (transform.position.x < -5.0f && grounded) {
			rigidBody2D.AddForceAtPosition(new Vector2(100,0), new Vector2(beginPosition.x, beginPosition.y));
		}
	}

	public void SetCurrentState(GameManager.GameState pGameState){
		this.CurrentGameState = pGameState;
	}

	public void Jump(){
		if (jumpCount > 0) {
			rigidBody2D.AddForce (new Vector2 (0f, 800f));
			jumpCount--;
		}
	}
	
		


}