using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class PlayerController : MonoBehaviour {

	public GameManager.GameState CurrentGameState;

	[HideInInspector] public int jumpCount = 2;
	public float jumpForce = 500f;

	public Transform groundCheck;
	private bool grounded = false;
	public Transform sideCheck;
	private bool sideBlock = false;

	public PhysicsMaterial2D normalMaterial;
	public PhysicsMaterial2D iceMaterial;
	public GameObject explosionPrefab;

	private Rigidbody2D rb2d;
	private Renderer renderer;

	private bool InitAux = false;

	private bool GameOver;
	private bool tapping = false;


	private Vector2 lastPosition;

	// Use this for initialization
	void Awake () 
	{
		rb2d = GetComponent<Rigidbody2D>();
		renderer = GetComponent<Renderer> ();
		GameOver = false;

		lastPosition = new Vector2(transform.position.x,transform.position.y);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(CurrentGameState == GameManager.GameState.Resume) {
//		if (GameManager.CheckState(GameManager.GameState.Resume)) {

			grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

			sideBlock = Physics2D.Linecast (transform.position, sideCheck.position, 1 << LayerMask.NameToLayer ("Ground"));


			if (sideBlock) {
				GetComponent<CircleCollider2D> ().sharedMaterial = iceMaterial;

			} else 
				GetComponent<CircleCollider2D> ().sharedMaterial = normalMaterial;

			if (grounded)
				jumpCount = 2; 

			bool touchClicked = false;
			for (int i = 0; i < Input.touches.Length; i++) {
				touchClicked = Input.touches [i].phase == TouchPhase.Began;
			}

			if ((Input.GetKeyDown (KeyCode.Space) || touchClicked)) {
				if (jumpCount > 0) {
					
					GetComponent<AudioSource> ().Play ();
					Vector3 vel = rb2d.velocity;
					vel.y = 0;
					rb2d.velocity = vel;
					if (sideBlock)
						rb2d.AddForce (new Vector2 (0f, jumpForce * 1.5f));
					else 
						rb2d.AddForce (new Vector2 (0f, jumpForce));
					jumpCount--;
				}
			}


			if (grounded && !sideBlock) 
			if (transform.position.x < -5) {
				rb2d.AddForce (new Vector2 (60f, 0f));
			} else {
				Vector3 vel = rb2d.velocity;
				vel.x = 0;
				rb2d.velocity = vel;
			}
			else {
				Vector3 vel = rb2d.velocity;
				vel.x = 0;
				rb2d.velocity = vel;
			}
		}
	}


	public void FixedUpdate () {

		if (CurrentGameState == GameManager.GameState.Resume) {
			if (renderer.isVisible) {
				InitAux = true;
			} else if (InitAux) {

				Instantiate (explosionPrefab, transform.position, transform.rotation);
				explosionPrefab.GetComponent<AudioSource> ().Play ();
				Destroy (gameObject);
				
				GameObject.Find ("Game Manager").SendMessage ("SetGameState", GameManager.GameState.Over);
			}

			Vector2 direction = new Vector2 (1, 0);
			RaycastHit2D hit = Physics2D.Raycast (sideCheck.position, direction, 0.0f);


			if (hit.collider != null) {

				Vector2 pos = transform.position;
				pos.x = hit.collider.transform.position.x - 1.1f;
				transform.position = pos;

			}
		}
	}

	public void SetCurrentState(GameManager.GameState pGameState){
		this.CurrentGameState = pGameState;
	}

}