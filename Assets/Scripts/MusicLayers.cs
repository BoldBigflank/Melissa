using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicLayers : MonoBehaviour {
	public AudioClip[] sounds;

	[Range(0.0f, 1.0f)]
	public float volume = 1.0f;

//	public float damping = 6.0f;

	List<AudioSource> audioClips;

	// Use this for initialization
	void Start () {
		audioClips = new List<AudioSource>();
		
		
		for (int i=1; i < sounds.Length; i++){
			AudioSource a = gameObject.AddComponent<AudioSource>();
			a.clip = sounds[i];
			a.volume = 0.0f;
			a.loop = true;
			audioClips.Add(a);
		}
	}

	public void StartMusic(){
		for (int i = 0; i < audioClips.Count; i++){
			audioClips[i].Play ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		int stage = GameManager.current.GetStage();
		for (int i = 0; i < audioClips.Count; i++){
			float desiredVolume = (stage >= i) ? volume : 0.0F;
//			desiredVolume = Mathf.Lerp(audioClips[i].volume, desiredVolume, Time.deltaTime * damping);
			audioClips[i].volume = desiredVolume;
		}
	}
}