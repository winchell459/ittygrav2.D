using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class leverScript : MonoBehaviour {
    public bool triggered = false;

    public float MaxRotation = 160f; //switched
    public float MinRotation = 20f;

    public float RotationSpeed = 100f;
    public GameObject LeverPivot;
    //public GameObject Splash;
    private LevelHandler lh;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(triggered){
            if(LeverPivot.transform.localEulerAngles.z > MinRotation){
                LeverPivot.transform.Rotate(new Vector3(0, 0, -RotationSpeed * Time.deltaTime));
            }else{
                trigger(true);
            }
        }else{
            if(LeverPivot.transform.localEulerAngles.z < MaxRotation){
                LeverPivot.transform.Rotate(new Vector3(0, 0, RotationSpeed * Time.deltaTime));
            }else{
                trigger(false);
            }
        }
	}

    private void trigger(bool isTriggered)
    {
        if (GameObject.FindGameObjectWithTag("LevelHandler"))
        {
            if (!lh) lh = GameObject.FindGameObjectWithTag("LevelHandler").GetComponent<LevelHandler>();
            lh.LeverTriggered(isTriggered);
        }
        
        
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Box")
        {
            triggered = true; // !triggered;
        }
    }
}
