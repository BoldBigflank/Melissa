using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
	AsyncOperation a;
	// Use this for initialization
	void Start () {
		a = SceneManager.LoadSceneAsync("FirstScene");
		a.allowSceneActivation = false;

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(SceneManager.GetActiveScene());
		if(a.isDone){
			Debug.Log("Done!");
		} else {
			Debug.Log("Not Done!");
		}
		if(Input.GetKeyDown (KeyCode.Escape)){
			Application.LoadLevel ("Credits");
		}
	
		if(Input.GetKeyDown(KeyCode.Z)){
			Application.LoadLevel ("Credits");

		}
		if(Input.GetKeyDown (KeyCode.X)){
//			Application.LoadLevel("FirstScene");
			a.allowSceneActivation = true;
		}
	}

}
