using UnityEngine;
using System.Collections;
using UnityEngine.UI;




public class GameManager : MonoBehaviour {
	public static GameManager current;

	public GameObject movieScreen;
	public Canvas canvas;
	GameObject mainCamera;

	Renderer movieRenderer;

	MovieTexture movie;
	int stage;
	string choice;

	bool uiEnabled;

	public float[] banalUIDelay;
	public float[] punchUIDelay;
	public float[] punchScreenShakeTimestamp;
	public Vector2[] punchScreenShakeForce;


	public Text debugText;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		current = this;
		movieRenderer = movieScreen.GetComponent<Renderer>();
		stage = 0;
		choice = "Punch";
		uiEnabled = false;
		PlayMovieThenLoop(stage + choice);
	}
	
	// Update is called once per frame
	void Update () {
		// Keyboard shortcuts
		if(uiEnabled && Input.GetKeyDown(KeyCode.Z)){
			ButtonPress("Z");
		}
		if(uiEnabled && Input.GetKeyDown(KeyCode.X)){
			ButtonPress("X");
		}
	}

	public void ButtonPress(string button){
		// We're not doing the loop any more
		StopCoroutine("SetupLoop"); // No loop

		Debug.Log(button + " button pressed");

		// If it's the Banal Z Button
		if(uiEnabled && button == "Z"){
			choice = "Banal";
			PlayMovieThenLoop(stage + choice);
			canvas.GetComponent<Animation>().Play("PressZ");
		}

		// If it's the Punch X Button
		if(uiEnabled && button == "X"){
			stage++;
			choice = "Punch";
			PlayMovieThenLoop(stage + choice);
			canvas.GetComponent<Animation>().Play("PressX");
		}

	}

	void PlayMovieThenLoop(string path){
		StopAllCoroutines(); // UI and Loop

		// TODO: Can I load all movies before they are needed? Or the next possible 3?
		// TODO: Blink closed/open during this
		movie = Resources.Load(path) as MovieTexture;
		movie.loop = false;

		movieRenderer.material.mainTexture = movie;
		movie.Stop();
		movie.Play();
		debugText.text = path;

		// Set up the loop
		StartCoroutine(SetupLoop(path));

		if(choice == "Banal"){
			StartCoroutine( DisableUI(banalUIDelay[stage]));

		}
		if(choice == "Punch"){
			StartCoroutine( DisableUI(punchUIDelay[stage]));
			StartCoroutine( PunchScreenShake(punchScreenShakeTimestamp[stage], punchScreenShakeForce[stage]));
		}

	}

	private IEnumerator PunchScreenShake (float seconds, Vector2 force){
		yield return new WaitForSeconds(seconds);

		mainCamera.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);

	}

	private IEnumerator DisableUI (float seconds){
		uiEnabled = false;

		Debug.Log("UI is Disabled");

		yield return new WaitForSeconds(seconds);

		canvas.GetComponent<Animation>().Play("FadeIn");
		Debug.Log("UI is Enabled");

		uiEnabled = true;

	}


	private IEnumerator SetupLoop(string path){
		// Wait for the end of the movie
		while(movie.isPlaying) yield return 0;


		Debug.Log("Opening " + path + "Loop");
		movie = Resources.Load(path + "Loop") as MovieTexture;
		if(!movie) {
			movie = Resources.Load(path + "End") as MovieTexture;
			movie.loop = false;
		} else {
			movie.loop = true;
		}
		debugText.text = path + "Loop";


		// Is this one necessary?
		movieRenderer.material.mainTexture = movie;
		movie.Play();
	}

}
