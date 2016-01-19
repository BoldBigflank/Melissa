using UnityEngine;
using System.Collections;

// Make sure we have gui texture and audio source
[RequireComponent (typeof(GUITexture))]
[RequireComponent (typeof(AudioSource))]
public class TestMovie : MonoBehaviour {
	
	string url = "http://www.unity3d.com/webplayers/Movie/sample.ogg";
	WWW www;

	void Start () {
		// Start download
		www = new WWW(url);

		StartCoroutine(PlayMovie());
	}

	IEnumerator PlayMovie(){
		MovieTexture movieTexture = www.movie;

		// Make sure the movie is ready to start before we start playing
		while (!movieTexture.isReadyToPlay){
			yield return 0;
		}

		GUITexture gt = gameObject.GetComponent<GUITexture>();

		// Initialize gui texture to be 1:1 resolution centered on screen
		gt.texture = movieTexture;

		transform.localScale = Vector3.zero;
		transform.position = new Vector3 (0.5f,0.5f,0f);
//		gt.pixelInset.xMin = -movieTexture.width / 2;
//		gt.pixelInset.xMax = movieTexture.width / 2;
//		gt.pixelInset.yMin = -movieTexture.height / 2;
//		gt.pixelInset.yMax = movieTexture.height / 2;

		// Assign clip to audio source
		// Sync playback with audio
		AudioSource aud = gameObject.GetComponent<AudioSource>();
		aud.clip = movieTexture.audioClip;

		// Play both movie & sound
		movieTexture.Play();
		aud.Play();

	}

}
