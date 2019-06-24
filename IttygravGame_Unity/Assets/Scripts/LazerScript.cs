using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerScript : MonoBehaviour {
    public Transform PlayerRespawnPoint;

    private Player HitPlayer;
    private bool hasPlayer = false;

    public float FreezeTime = 1.0f;
    private float frozeTime = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (hasPlayer)
        {
            if(Time.fixedTime > frozeTime + FreezeTime)
            {
                HitPlayer.transform.position = PlayerRespawnPoint.position;
                HitPlayer.PositionFrozen = false;
                hasPlayer = false;
                
            }
            else if(Mathf.Abs(transform.position.y - HitPlayer.transform.position.y) < 0.1)
            {
                HitPlayer.PositionFrozen = true;
            }
        }
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(collision.name == "Center" && !hasPlayer)
            {
                HitPlayer = collision.gameObject.GetComponent<Player>();
                HitPlayer.PositionFrozen = true;
                hasPlayer = true;
                frozeTime = Time.fixedTime;
            }else if (collision.name == "Center")
            {
                HitPlayer.PositionFrozen = true;
            }
            else
            {
                HitPlayer = collision.gameObject.GetComponent<Player>();
                //HitPlayer.PositionFrozen = true;
                hasPlayer = true;
                frozeTime = Time.fixedTime;
            }
            if (GameObject.FindGameObjectWithTag("AudioSource"))
            {
                GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioController>().PlayPlayerFX(3);
            }

            //
        }
    }
}
