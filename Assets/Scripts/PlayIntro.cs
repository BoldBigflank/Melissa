using UnityEngine;
using System.Collections;

public class PlayIntro : MonoBehaviour {
	public GameObject movieScreen;
	public MovieTexture movie;
	Renderer movieRenderer;
	public Material endMaterial;
	public bool goToNextScene = false;

	GameObject mainCamera;
	public AudioSource audioSource;
	bool done;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		movieRenderer = movieScreen.GetComponent<Renderer>();
		PlayMovie();
		done = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(done && Input.anyKeyDown){
			Application.LoadLevel("Intro");
		}
	}

	void PlayMovie(){
		StopAllCoroutines(); // UI and Loop

		movie.loop = false;

		movieRenderer.material.mainTexture = movie;
		movie.Stop();
		movie.Play();
		audioSource.Play();

		// Set up the loop
		StartCoroutine(NextScene());
	}

	private IEnumerator NextScene(){
		// Wait for the end of the movie
		while(movie.isPlaying) yield return 0;

		done = true;
		movieRenderer.material = endMaterial;
		if(goToNextScene) Application.LoadLevel("Scenes/FirstScene");

		// TODO: Go to the halfsheet/start again?
	}
}
