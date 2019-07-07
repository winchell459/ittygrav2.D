using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector2 getDisplacementForce(Transform gravObject, Vector2 targetPos, float mass)
    {
        Vector2 gravTargetDisplacement = new Vector2(targetPos.x - gravObject.position.x, targetPos.y - gravObject.position.y);

        Rigidbody2D gravObjectRB2D = gravObject.GetComponent<Rigidbody2D>();
        

        float deltaTime = Time.deltaTime;
        //Debug.Log("Time.deltaTime: " + deltaTime);
        //Vector2 gravityNullify = -gravObjectParam.Gravity * gravObjectParam.Mass * gravObjectParam.GravityCoefficient;
        float displaceForceX = 2 * (-gravObjectRB2D.velocity.x * deltaTime + gravTargetDisplacement.x) / (deltaTime * deltaTime);
        float displaceForceY = 2 * (-gravObjectRB2D.velocity.y * deltaTime + gravTargetDisplacement.y) / (deltaTime * deltaTime);
        displaceForceX *= mass;
        displaceForceY *= mass;

        return new Vector2(displaceForceX, displaceForceY);
    }
}
