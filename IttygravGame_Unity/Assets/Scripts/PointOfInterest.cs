using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour {

    public float ZoomRate = 0.5f;
    //public float PanRate = 0.5f;
    public float CameraSize = 10;
    public float ZoomedCameraSize = 5; 
    public GameObject PanTarget;
    private bool triggered = false;
    public bool ZoomIn = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<CameraController>().POI = this;
            FindObjectOfType<CameraController>().FollowPlayer = false;
            triggered = true;
            ZoomIn = false;
        }
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!triggered)
            {
                FindObjectOfType<CameraController>().POI = this;
                FindObjectOfType<CameraController>().FollowPlayer = false;
                triggered = true;
                ZoomIn = false;
            }
        }
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameObject.FindObjectOfType<CameraController>().POI == this)
            {
                //FindObjectOfType<CameraController>().POI = null;
                ZoomIn = true;
                FindObjectOfType<CameraController>().FollowPlayer = true;
                triggered = false;
            }
        }
        
    }
}
