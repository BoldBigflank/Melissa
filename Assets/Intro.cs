using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Escape)){
			Application.LoadLevel ("Credits");
		}
	
		if(Input.GetKeyDown(KeyCode.Z)){
			Application.LoadLevel ("Credits");

		}
		if(Input.GetKeyDown (KeyCode.X)){
			Application.LoadLevel("FirstScene");
		}
	}

}
