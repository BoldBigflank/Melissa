using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (AudioSource))]
public class MusicLayers : MonoBehaviour {
	public AudioClip[] sounds;

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
			audioClips[i].volume = (stage >= i) ? 1.0F : 0.0F;
		}
	}
}