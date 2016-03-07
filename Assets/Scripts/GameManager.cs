using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
	public static GameManager current;
//	string urlRoot = "https://s3-us-west-2.amazonaws.com/melissaagameofchoice/";
//	string urlRoot = "https://dl.dropboxusercontent.com/u/7776712/Converted/";
	public Canvas canvas;
	
	// Game Objects/components
	GameObject mainCamera;
	Wiggle wiggle;

	// Punch Movie stuff
	Dictionary<string, MovieTexture> allMovies;
	public GameObject movieScreen;
	public AudioSource movieSound;
	Renderer movieRenderer;
	MovieTexture movieTexture;
	int stage;

	// Banal Movie stuff
	Dictionary<string, MovieTexture> banalMovies;
	public GameObject banalScreen;
	public AudioSource banalSound;
	Renderer banalRenderer;
	MovieTexture banalTexture;
	int banalStage;

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
		wiggle = mainCamera.GetComponent<Wiggle>();
		current = this;
		movieRenderer = movieScreen.GetComponent<Renderer>();
		banalRenderer = banalScreen.GetComponent<Renderer>();
		stage = 0;
		banalStage = 0;
		choice = "Punch";
		uiEnabled = false;

		// Play the intro, 
//		StartCoroutine( PlayMovieFromResources("Intro", false));
		movieTexture = (MovieTexture)movieRenderer.material.mainTexture;
		movieTexture.Play();
		banalTexture = banalRenderer.material.mainTexture as MovieTexture;
		banalTexture.Play();

		// All the movies
		allMovies = new Dictionary<string, MovieTexture>();
//		allMovies.Add(new  Resources.Load<MovieTexture>("MovieTextures/" + path) as MovieTexture);

		allMovies.Add("1Punch", Resources.Load<MovieTexture>("MovieTextures/1Punch") as MovieTexture);
		allMovies.Add("2Punch", Resources.Load<MovieTexture>("MovieTextures/2Punch") as MovieTexture);
		allMovies.Add("3Punch", Resources.Load<MovieTexture>("MovieTextures/3Punch") as MovieTexture);
		allMovies.Add("4Punch", Resources.Load<MovieTexture>("MovieTextures/4Punch") as MovieTexture);
		allMovies.Add("5Punch", Resources.Load<MovieTexture>("MovieTextures/5Punch") as MovieTexture);
		allMovies.Add("6Punch", Resources.Load<MovieTexture>("MovieTextures/6Punch") as MovieTexture);
		allMovies.Add("7Punch", Resources.Load<MovieTexture>("MovieTextures/7Punch") as MovieTexture);
		allMovies.Add("8Punch", Resources.Load<MovieTexture>("MovieTextures/8Punch") as MovieTexture);
		allMovies.Add("9Punch", Resources.Load<MovieTexture>("MovieTextures/9Punch") as MovieTexture);
		allMovies.Add("10Punch", Resources.Load<MovieTexture>("MovieTextures/10Punch") as MovieTexture);
		allMovies.Add("0PunchLOOP", Resources.Load<MovieTexture>("MovieTextures/0PunchLOOP") as MovieTexture);
		allMovies.Add("1PunchLOOP", Resources.Load<MovieTexture>("MovieTextures/1PunchLOOP") as MovieTexture);
		allMovies.Add("2PunchLOOP", Resources.Load<MovieTexture>("MovieTextures/2PunchLOOP") as MovieTexture);
		allMovies.Add("3PunchLOOP", Resources.Load<MovieTexture>("MovieTextures/3PunchLOOP") as MovieTexture);
		allMovies.Add("4PunchLOOP", Resources.Load<MovieTexture>("MovieTextures/4PunchLOOP") as MovieTexture);
		allMovies.Add("5PunchLOOP", Resources.Load<MovieTexture>("MovieTextures/5PunchLOOP") as MovieTexture);
		allMovies.Add("6PunchLOOP", Resources.Load<MovieTexture>("MovieTextures/6PunchLOOP") as MovieTexture);
		allMovies.Add("7PunchLOOP", Resources.Load<MovieTexture>("MovieTextures/7PunchLOOP") as MovieTexture);
		allMovies.Add("8PunchLOOP", Resources.Load<MovieTexture>("MovieTextures/8PunchLOOP") as MovieTexture);
		allMovies.Add("9PunchLOOP", Resources.Load<MovieTexture>("MovieTextures/9PunchLOOP") as MovieTexture);

		banalMovies = new Dictionary<string, MovieTexture>();
		banalMovies.Add("Intro", Resources.Load<MovieTexture>("MovieTextures/Intro") as MovieTexture);
		banalMovies.Add("0Banal", Resources.Load<MovieTexture>("MovieTextures/0Banal") as MovieTexture);
		banalMovies.Add("1Banal", Resources.Load<MovieTexture>("MovieTextures/1Banal") as MovieTexture);
		banalMovies.Add("2Banal", Resources.Load<MovieTexture>("MovieTextures/2Banal") as MovieTexture);
		banalMovies.Add("3Banal", Resources.Load<MovieTexture>("MovieTextures/3Banal") as MovieTexture);
		banalMovies.Add("4Banal", Resources.Load<MovieTexture>("MovieTextures/4Banal") as MovieTexture);
		banalMovies.Add("5Banal", Resources.Load<MovieTexture>("MovieTextures/5Banal") as MovieTexture);
		banalMovies.Add("6Banal", Resources.Load<MovieTexture>("MovieTextures/6Banal") as MovieTexture);
		banalMovies.Add("7Banal", Resources.Load<MovieTexture>("MovieTextures/7Banal") as MovieTexture);
		banalMovies.Add("8Banal", Resources.Load<MovieTexture>("MovieTextures/8Banal") as MovieTexture);
		banalMovies.Add("9Banal", Resources.Load<MovieTexture>("MovieTextures/9Banal") as MovieTexture);
		banalMovies.Add("10Banal", Resources.Load<MovieTexture>("MovieTextures/10Banal") as MovieTexture);
		banalMovies.Add("11Banal", Resources.Load<MovieTexture>("MovieTextures/11Banal") as MovieTexture);
		banalMovies.Add("12Banal", Resources.Load<MovieTexture>("MovieTextures/12Banal") as MovieTexture);

	}



	IEnumerator PlayPunchMovie(string path, bool loop){
		Debug.Log ("Play movie from Resources " + path);

//		Resources.UnloadAsset(movieTexture); // Remove the old one
		Debug.Log ("Loading " + path);
//		movieTexture = Resources.Load<MovieTexture>("MovieTextures/" + path) as MovieTexture;
		MovieTexture newMovieTexture = allMovies[path];
		
		while(!newMovieTexture.isReadyToPlay){
			yield return 0;
		}

		movieTexture.Stop();
		movieSound.Stop();

		movieTexture = newMovieTexture;
		movieTexture.loop = loop;

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
//			mainCamera.GetComponent<Sway>().enabled = true;
			wiggle.enabled = true;
//			mainCamera.GetComponent<Rigidbody2D>().isKinematic = false;
			mainCamera.GetComponent<Camera>().orthographicSize = 4.0F;
			movieScreen.transform.localScale = new Vector3(16.0f, 10.0f, 1.0f);
			movieScreen.transform.position = new Vector3(0.0f, -0.796f, 0.0f);

			// Start the music
			gameObject.GetComponent<MusicLayers>().StartMusic();
		}
		
	}

	IEnumerator PlayBanalMovie(string path){
		Debug.Log ("Play Banal " + path);

		//		Resources.UnloadAsset(movieTexture); // Remove the old one
		Debug.Log ("Loading " + path);
		//		movieTexture = Resources.Load<MovieTexture>("MovieTextures/" + path) as MovieTexture;
		MovieTexture newBanalTexture = banalMovies[path];

		while(!newBanalTexture.isReadyToPlay){
			yield return 0;
		}


		banalTexture.Stop();
		banalSound.Stop();

		banalTexture = newBanalTexture;
		banalTexture.loop = false;

		banalRenderer.material.mainTexture = banalTexture;
		banalSound.clip = banalTexture.audioClip;
		DisableUI(banalTexture.duration);

		#if UNITY_EDITOR
		debugText.text = path;
		#endif

		banalTexture.Play();
		banalSound.Play ();
		banalStage = (banalStage + 1) % banalMovies.Count;
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
			if(Input.GetMouseButtonDown(1)){
				movieTexture.Stop();
				EnableUI();
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
			StartCoroutine(PlayPunchMovie(stage + choice + "LOOP", true));
		}

	}

	public int GetStage(){
		return stage;
	}

	public void ButtonPress(string button){
		// We're not doing the loop any more
		StopCoroutine("SetupLoop"); // No loop

		Debug.Log(button + " button pressed");

		// If it's the Banal Z Button
		if(uiEnabled && button == "Z"){
			choice = "Banal";
			canvas.GetComponent<Animation>().Play("PressZ");
			StartCoroutine( PlayPunchMovie(stage+choice, false));
//			AddMovieToQueue (stage + choice + "LOOP");
		}

		// If it's the Punch X Button
		if(uiEnabled && button == "X"){
			stage++;
			choice = "Punch";

			StartCoroutine( PlayPunchMovie(stage+choice, false));
			// The next phase
			canvas.GetComponent<Animation>().Play("PressX");
		}

	}

	private IEnumerator PunchScreenShake (float seconds, Vector2 force){
		yield return new WaitForSeconds(seconds);

//		mainCamera.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);

	}

	private IEnumerator DisableUI (float seconds){
		uiEnabled = false;

		yield return new WaitForSeconds(seconds);

		EnableUI();
	}

	private void EnableUI(){
		canvas.GetComponent<Animation>().Play("FadeIn");
		uiEnabled = true;
	}

}
