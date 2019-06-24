using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController2D : MonoBehaviour {

    [SerializeField]
    private CrateParameters2D DefaultParameters;

    public CrateParameters2D Parameters { get { return _overrideParameters ?? DefaultParameters; } }

    private CrateParameters2D _overrideParameters;

    public float DeltaMoveCoefficient = 1.5f;

    private List<Transform> collidingObjects = new List<Transform>();

    private Vector2 playerMaxVelocity;

    // Use this for initialization
    void Start () {
        GetComponent<Rigidbody2D>().mass = Parameters.Mass;
	}
	
	// Update is called once per frame
	void Update () {
		if(playerMaxVelocity == null)
        {
            playerMaxVelocity = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>().Parameters.MaxVelocity;
        }
	}

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce(Parameters.GravityCoefficient* Parameters.Mass *Parameters.Gravity );
        //collidingObjects.Clear();
    }

    public Vector2 Move(Vector2 deltaMovement, float massOfImpact, Vector2 normalForce)
    {
        //Do momentum calculation and check for box or wall collisions
        //Return the deltaMovement to set displacement of all touching objects.
        //If touching a wall, return 0


        transform.position += (Vector3)deltaMovement;
        //Vector2 myDeltaMovement = deltaMovement * DeltaMoveCoefficient / Time.deltaTime;
        //Debug.Log(deltaMovement.x);

        //GetComponent<Rigidbody2D>().velocity = myDeltaMovement;
        GetComponent<Rigidbody2D>().AddForce(normalForce );

        if (GameObject.FindGameObjectWithTag("AudioSource"))
        {
            GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioController>().PlayPlayerFX(2);
        }

        return deltaMovement;
    }

    public Vector2 VerticalCollision(Vector2 deltaMovement, float massOfImpact, Vector2 normalForce)
    {

        return deltaMovement;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Box"))
        {
            collidingObjects.Add(collision.transform);
        }
    }

	private void OnCollisionExit2D(Collision2D collision)
	{
        if (collision.transform.CompareTag("Box"))
        {
            collidingObjects.Remove(collision.transform);
        }
	}

	public List<Transform> GetCollidingObject(){
        return collidingObjects;
    }
}
