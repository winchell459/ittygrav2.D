using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    public Transform[] Waypoints;
    private int index = 0;
    public float Speed = 10f;
    public float WaitTime = 2f;
    private float freezeTime = float.NegativeInfinity;
    public bool moving = false;

    private List<Transform> collidingObjects = new List<Transform>();
    private List<Transform> handledCollidingObjects = new List<Transform>();

    public bool IgnoreStacking = false;
    public bool Pushing = false;
    public MovingPlatform PairedPlatform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(moving){
            Vector3 displacement = Vector3.MoveTowards(transform.position, Waypoints[index].position, Speed * Time.deltaTime) - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[index].position, Speed * Time.deltaTime);
            if (!IgnoreStacking) checkCollidingObjects(collidingObjects, displacement);
            //for (int i = 0; i < collidingObjects.Count; i += 1){
            //    collidingObjects[i].position += displacement;
            //}
            if(Vector3.Distance(transform.position, Waypoints[index].position) < 0.1){
                moving = false;
                index = (index + 1) % Waypoints.Length;
                freezeTime = Time.fixedTime;
                
                
            }
        }else if(freezeTime + WaitTime < Time.fixedTime && !PairedPlatform)
        {
            moving = true;
        }else if(freezeTime + WaitTime < Time.fixedTime && !PairedPlatform.moving)
        {
            moving = true;
        }

        collidingObjects.Clear();
        handledCollidingObjects.Clear();
	}

    private void checkCollidingObjects(List<Transform> collided, Vector3 displacement){
        for (int i = 0; i < collided.Count; i+= 1){
            if(!handledCollidingObjects.Contains(collided[i])){
                if(displacement.x < 0)
                {
                    if(collided[i].position.x < transform.position.x + 2.06f)
                    {
                        collided[i].position += displacement;
                        handledCollidingObjects.Add(collided[i]);
                        checkCollidingObjects(collided[i].GetComponent<CrateController2D>().GetCollidingObject(), displacement);
                    }
                }else if(displacement.x > 0)
                {
                    if (collided[i].position.x > transform.position.x - 2.06f)
                    {
                        collided[i].position += displacement;
                        handledCollidingObjects.Add(collided[i]);
                        checkCollidingObjects(collided[i].GetComponent<CrateController2D>().GetCollidingObject(), displacement);
                    }
                }
                
            }
        }
        collided.Clear();
    }

	private void OnCollisionStay2D(Collision2D collision)
	{
        if(collision.transform.CompareTag("Box")){
            collidingObjects.Add(collision.transform);
        }
	}
}
