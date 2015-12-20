using UnityEngine;
using System.Collections;

public class Sway : MonoBehaviour {
	public bool enabled;
	Vector3 originalPosition;
	Vector3 swayDirection;

	public SpringJoint2D joint;
	public float swayRadius = 1.5F;
	public float verticalDistance = 1.5F;
	public float horizontalDistance = 2.0F;
	public float verticalPeriod = 2.0F;
	public float horizontalPeriod = 2.2F;



	// Use this for initialization
	void Start () {
		enabled = false;
		originalPosition = transform.position;
		swayDirection = Random.onUnitSphere * 6.0F;
	}
	
	// Update is called once per frame
	void Update () {
		if(enabled){
			float x = horizontalDistance * Mathf.Sin( Time.timeSinceLevelLoad / horizontalPeriod );
			float y = verticalDistance * Mathf.Sin( Time.timeSinceLevelLoad / verticalPeriod );
			joint.anchor = new Vector2(x, y);
		} else {
			joint.anchor = Vector2.zero;
		}
	}
}
