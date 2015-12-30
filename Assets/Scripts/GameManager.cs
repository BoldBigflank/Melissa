using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
	public static GameManager current;
//	string urlRoot = "https://s3-us-west-2.amazonaws.com/melissaagameofchoice/";
	string urlRoot = "https://dl.dropboxusercontent.com/u/7776712/Converted/";

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
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		movieQueue = new Dictionary<string, MovieTexture>();
		current = this;
		movieRenderer = movieScreen.GetComponent<Renderer>();
		stage = 0;
		choice = "Punch";
		uiEnabled = false;

		AddMovieToQueue ("Intro");
		// load 0Punch
		AddMovieToQueue (urlRoot + (stage+1) + "Punch" + ".ogg");
		AddMovieToQueue (urlRoot + (stage+1) + "PunchLoop" + ".ogg");
		// The available banal
		AddMovieToQueue (urlRoot + stage + "Banal" + ".ogg");
		// The loop of the current choice
		AddMovieToQueue (urlRoot + stage + choice + "Loop.ogg");
		// Play the intro, 
		StartCoroutine( PlayMovieFromQueue("Intro", false));
//		StartCoroutine( PlayMovieFromQueue(urlRoot + (stage) + "PunchLoop" + ".ogg", true));
	}

	void AddMovieToQueue(string path){
		if(movieQueue.ContainsKey(path)) return; // Don't add already existing ones

		Debug.Log ("AddMovieToQueue " + path);
		MovieTexture mt = new MovieTexture();
		if(path.StartsWith("http")){
			WWW www = new WWW(path);
			mt = www.movie;
//			mt = Resources.Load("0Punch") as MovieTexture; // DEBUG
		} else {
			mt = Resources.Load(path) as MovieTexture;
		}

		movieQueue.Add (path, mt);
	}

	void ClearQueue(){
		List<string> keysToRemove = new List<string>();
		// Delete all the movietextures that are not currently playing
		foreach(string key in movieQueue.Keys){
			if(movieQueue[key] != movieTexture){
				keysToRemove.Add(key);
			}
		}
		foreach(string key in keysToRemove){
			movieQueue.Remove(key);
		}
	}
	
	IEnumerator PlayMovieFromQueue(string path, bool loop){
		Debug.Log ("Play movie from Queue " + path);
		StopCoroutine("PlayMovieFromQueue");  // Stop the others

		if(!movieQueue.ContainsKey(path)){
			Debug.LogError ("No file found");
		}


		movieTexture = movieQueue[path];
		
		while(!movieTexture.isReadyToPlay){
//			Debug.Log ("Waiting");
			yield return 0;
		}
		
		movieSound.clip = movieTexture.audioClip;
		movieTexture.loop = loop;

		movieTexture.Stop();
		movieSound.Stop();

		movieRenderer.material.mainTexture = movieTexture;
		movieSound.clip = movieTexture.audioClip;

		debugText.text = path;
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
		if(path.Contains("0PunchLoop")){
			// Start the swaying and the bobbing
			mainCamera.GetComponent<Sway>().enabled = true;
			mainCamera.GetComponent<Rigidbody2D>().isKinematic = false;
			mainCamera.GetComponent<Camera>().orthographicSize = 3.0F;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Keyboard shortcuts
		if(Input.GetKeyDown (KeyCode.Escape)){
			Application.LoadLevel ("Credits");
		}
		if(uiEnabled && Input.GetKeyDown(KeyCode.Z)){
			ButtonPress("Z");
		}
		if(uiEnabled && Input.GetKeyDown(KeyCode.X)){
			ButtonPress("X");
		}


		if (movieTexture && !movieTexture.isPlaying && movieTexture.isReadyToPlay){
			if(stage == 10){
				// End the game
				Application.LoadLevel ("Credits");
				return;
			}
			Debug.Log ("Starting a Loop");
			StartCoroutine(PlayMovieFromQueue(urlRoot + stage + choice + "Loop.ogg", true));
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
			StartCoroutine( PlayMovieFromQueue(urlRoot+stage+choice+".ogg", false));
			AddMovieToQueue (urlRoot + stage + choice + "Loop.ogg");
		}

		// If it's the Punch X Button
		if(uiEnabled && button == "X"){
			stage++;
			choice = "Punch";

			StartCoroutine( PlayMovieFromQueue(urlRoot+stage+choice+".ogg", false));
			// The next phase
			ClearQueue();
			if(stage < 9){
				AddMovieToQueue (urlRoot + (stage+1) + "Punch" + ".ogg");
				// The available banal
				AddMovieToQueue (urlRoot + stage + "Banal" + ".ogg");
				// The loop of the current choice
				AddMovieToQueue (urlRoot + stage + choice + "Loop.ogg");

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

		Debug.Log("UI is Disabled");

		yield return new WaitForSeconds(seconds);

		canvas.GetComponent<Animation>().Play("FadeIn");
		Debug.Log("UI is Enabled");

		uiEnabled = true;

	}


	private IEnumerator SetupLoop(string path){
		// Wait for the end of the movie
		while(!movieTexture.isReadyToPlay || movieTexture.isPlaying) yield return 0;


		Debug.Log("Opening " + path + "Loop");

		WWW www = new WWW("https://dl.dropboxusercontent.com/u/7776712/demo.ogg");
		movieTexture = www.movie;

//		movieTexture = Resources.Load(path + "Loop") as MovieTexture;

		// https://s3.amazonaws.com/SwanHomeMovies/Alex+Kevin+Home+1986-1-1.m4v
		if(!movieTexture) {
			movieTexture = Resources.Load(path + "End") as MovieTexture;
			movieTexture.loop = false;
		} else {
			movieTexture.loop = true;
		}
		debugText.text = path + "Loop";


		// Is this one necessary?
		movieRenderer.material.mainTexture = movieTexture;
//		movie.Play();
	}

}
