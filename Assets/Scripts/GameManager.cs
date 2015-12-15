using UnityEngine;
using System.Collections;
using UnityEngine.UI;




public class GameManager : MonoBehaviour {
	public static GameManager current;

	public GameObject movieScreen;
	public Canvas canvas;

	Renderer movieRenderer;

	MovieTexture movie;
	int stage;
	string choice;

	bool uiEnabled;

	public float[] banalUIDelay;
	public float[] punchUIDelay;
	public float[] punchScreenShakeTimestamp;


	public Text debugText;

	// Use this for initialization
	void Start () {
		current = this;
		movieRenderer = movieScreen.GetComponent<Renderer>();
		stage = 0;
		choice = "Intro";
		uiEnabled = true;
//		canvas.GetComponent<Animation>().Stop();
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
		if(button == "Z"){
			choice = "Banal";
			PlayMovieThenLoop(stage + choice);
			canvas.GetComponent<Animation>().Play("PressZ");
		}

		// If it's the Punch X Button
		if(button == "X"){
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
			StartCoroutine( PunchScreenShake(punchUIDelay[stage]));
		}

	}

	private IEnumerator PunchScreenShake (float seconds){
		yield return new WaitForSeconds(seconds);

		// TODO: Tell the GameManager to screenshake down/left

	}

	private IEnumerator DisableUI (float seconds){
		uiEnabled = false;
		// TODO:  Animate out the GUI


		Debug.Log("UI is Disabled");

		yield return new WaitForSeconds(seconds);

		// TODO: Animate in the GUI
		canvas.GetComponent<Animation>().Play("FadeIn");
		Debug.Log("UI is Enabled");

		uiEnabled = true;

	}


	private IEnumerator SetupLoop(string path){
		// Wait for the end of the movie
		while(movie.isPlaying) yield return 0;

		if (stage == 9 && choice == "Punch") {
			// The end
			movie = Resources.Load("9PunchEND") as MovieTexture;
			movie.loop = false;

		} else {
			Debug.Log("Opening " + path + "Loop");
			movie = Resources.Load(path + "Loop") as MovieTexture;
			movie.loop = true;
			debugText.text = path + "Loop";
		}

		// Is this one necessary?
		movieRenderer.material.mainTexture = movie;
		movie.Play();
	}

}
