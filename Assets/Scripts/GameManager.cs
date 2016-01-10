using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
	public static GameManager current;
//	string urlRoot = "https://s3-us-west-2.amazonaws.com/melissaagameofchoice/";
//	string urlRoot = "https://dl.dropboxusercontent.com/u/7776712/Converted/";
	public GameObject movieScreen;
	public AudioSource movieSound;
	public Canvas canvas;
	GameObject mainCamera;

	Renderer movieRenderer;

	MovieTexture movieTexture;
	Dictionary<string, MovieTexture> movieQueue;

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
		Application.runInBackground = true;
		Application.targetFrameRate = 24;
		Time.captureFramerate = 24;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		movieQueue = new Dictionary<string, MovieTexture>();
		current = this;
		movieRenderer = movieScreen.GetComponent<Renderer>();
		stage = 0;
#if UNITY_EDITOR
		stage = 0;
#endif
		choice = "Punch";
		uiEnabled = false;

		// Play the intro, 
		StartCoroutine( PlayMovieFromResources("Intro", false));
	}
	
	IEnumerator PlayMovieFromResources(string path, bool loop){
		Debug.Log ("Play movie from Queue " + path);
		StopCoroutine("PlayMovieFromQueue");  // Stop the others

//		if(!movieQueue.ContainsKey(path)){
//			Debug.LogError ("No file found");
//		}


		Resources.UnloadAsset(movieTexture); // Remove the old one
		Debug.Log ("Loading " + path);
		movieTexture = Resources.Load<MovieTexture>(path) as MovieTexture;
		
		while(!movieTexture.isReadyToPlay){
			yield return 0;
		}
		
		movieSound.clip = movieTexture.audioClip;
		movieTexture.loop = loop;

		movieTexture.Stop();
		movieSound.Stop();

		movieRenderer.material.mainTexture = movieTexture;
		movieSound.clip = movieTexture.audioClip;

#if UNITY_EDITOR
		debugText.text = path;
#endif
		if(!loop){
			if(choice == "Banal"){
				StartCoroutine( DisableUI(banalUIDelay[stage]));
				
			}
			if(choice == "Punch"){
				StartCoroutine( DisableUI(punchUIDelay[stage]));
				StartCoroutine( PunchScreenShake(punchScreenShakeTimestamp[stage], punchScreenShakeForce[stage]));
			}
		}
		
		movieTexture.Play();
		movieSound.Play ();

		// Do other stuff because why not
		if(path.Contains("0PunchLOOP")){
			// Start the swaying and the bobbing
			mainCamera.GetComponent<Sway>().enabled = true;
			mainCamera.GetComponent<Rigidbody2D>().isKinematic = false;
			mainCamera.GetComponent<Camera>().orthographicSize = 3.5F;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(uiEnabled){
			Cursor.visible = true;
		}

		// Keyboard shortcuts
		if(Input.GetKeyDown (KeyCode.Escape)){
			Application.LoadLevel ("Credits");
		}
		if(uiEnabled && Input.GetKeyDown(KeyCode.Z)){
			ButtonPress("Z");
			Cursor.visible = false;
		}
		if(uiEnabled && Input.GetKeyDown(KeyCode.X)){
			ButtonPress("X");
			Cursor.visible = false;
		}
		
#if UNITY_EDITOR
			if(Input.GetMouseButtonDown(0)){
				movieTexture.Stop();
			}
#endif
		

		if (movieTexture && !movieTexture.isPlaying && movieTexture.isReadyToPlay){
			if(stage == 10){
				// End the game
				Debug.Log ("Ending the game");
				Application.LoadLevel ("Credits");
				return;
			}
			Debug.Log ("Starting a Loop");
			StartCoroutine(PlayMovieFromResources(stage + choice + "LOOP", true));
		}

	}

	public void ButtonPress(string button){
		// We're not doing the loop any more
		StopCoroutine("SetupLoop"); // No loop

		Debug.Log(button + " button pressed");

		// If it's the Banal Z Button
		if(uiEnabled && button == "Z"){
			choice = "Banal";
			canvas.GetComponent<Animation>().Play("PressZ");
			StartCoroutine( PlayMovieFromResources(stage+choice, false));
//			AddMovieToQueue (stage + choice + "LOOP");
		}

		// If it's the Punch X Button
		if(uiEnabled && button == "X"){
			stage++;
			choice = "Punch";

			StartCoroutine( PlayMovieFromResources(stage+choice, false));
			// The next phase
			if(stage < 11){
//				AddMovieToQueue ((stage+1) + "Punch");
				// The available banal
//				AddMovieToQueue (stage + "Banal");
				// The loop of the current choice
//				AddMovieToQueue (stage + choice + "LOOP");

			} else {
			}
			canvas.GetComponent<Animation>().Play("PressX");
		}

	}

	private IEnumerator PunchScreenShake (float seconds, Vector2 force){
		yield return new WaitForSeconds(seconds);

		mainCamera.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);

	}

	private IEnumerator DisableUI (float seconds){
		uiEnabled = false;

		yield return new WaitForSeconds(seconds);

		canvas.GetComponent<Animation>().Play("FadeIn");

		uiEnabled = true;

	}

}
