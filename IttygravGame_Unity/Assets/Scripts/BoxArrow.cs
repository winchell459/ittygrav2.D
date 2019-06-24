using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxArrow : MonoBehaviour {
    private float zRot;
	// Use this for initialization
	void Start () {
        zRot = transform.eulerAngles.z;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.eulerAngles = new Vector3(0, 0, zRot);
	}
}
