using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : GravObject {
    public bool Pickup = true;
    public bool Active = false;
    public float Reach = 0.5f;
    public float HeightOffset = 0.5f;
    public float RotationRate = 10f;
    public float GunRange = 5;
    public float GunForce = 10f;
    public float GunRangeChangeRate = 10f;
    public Transform GunMesh;
    public Transform GunPivot;

    public Transform gravObject;

    public LayerMask PlatformMask;

    public Vector2 gravObjectDisplacement;
    private float gravObjectDistance;
    private Vector2 gravObjectInternalDisplacement;
    private CampaignHandler campaignHandler;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!Pickup)
        {
            GunMesh.localPosition = Vector3.MoveTowards(GunMesh.localPosition, new Vector3(Reach, 0, 0), 100*Time.deltaTime);
            float parentRotation = transform.localEulerAngles.z;
            transform.localEulerAngles = new Vector3(0, 0, -parentRotation);

            float rotate = Input.GetAxis("Vertical");
            GunPivot.localEulerAngles = GunPivot.localEulerAngles + new Vector3(0, 0, rotate * RotationRate);
        }

        if (Active)
        {
            transform.localEulerAngles = new Vector3(0, 0, -transform.parent.eulerAngles.z);
            if (Input.GetMouseButtonDown(1) && !gravObject)
            {
                //Physics2D.Raycast()
                var raycastHit = Physics2D.Raycast(GunMesh.transform.position, GunMesh.transform.right, GunRange, PlatformMask);
                if (raycastHit)
                {
                    
                    //gravObjectDisplacement = new Vector2(raycastHit.transform.position.x - GunMesh.position.x, raycastHit.transform.position.y - GunMesh.position.y);
                    gravObjectDistance = Vector2.Distance(raycastHit.transform.position, GunMesh.position);
                    if(gravObjectDistance <= GunRange)
                    {
                        gravObject = raycastHit.transform;
                        gravObjectDisplacement = new Vector2(raycastHit.point.x - GunMesh.position.x, raycastHit.point.y - GunMesh.position.y);
                        gravObjectInternalDisplacement = new Vector2(gravObject.position.x - raycastHit.point.x, gravObject.position.y - raycastHit.point.y);
                    }
                    

                }
            }else if(Input.GetMouseButton(1) && gravObject)
            {
                var raycastHit = Physics2D.Raycast(GunMesh.transform.position, GunMesh.transform.right, GunRange, PlatformMask);
                if (raycastHit && raycastHit.transform == gravObject)
                {
                    Vector2 gravObjectTargetPos = new Vector2(gravObjectDistance * Mathf.Cos(GunPivot.eulerAngles.z * Mathf.PI / 180f), gravObjectDistance * Mathf.Sin(GunPivot.eulerAngles.z * Mathf.PI / 180f)) + (Vector2)GunMesh.position;

                    Parameters2D gravObjectParam = gravObject.GetComponent<CrateController2D>().Parameters;


                    //displaceForce = displaceForce /(-Mathf.Clamp(Vector3.Magnitude(displaceForce), -GunForce, GunForce) / GunForce) ;

                    Vector2 displaceForce = getDisplacementForce(gravObject, gravObjectTargetPos, gravObjectParam.Mass);
                    displaceForce = displaceForce / (Vector3.Magnitude(displaceForce) / GunForce);

                    gravObject.GetComponent<Rigidbody2D>().AddForce(displaceForce);


                    //Debug.Log(displaceForce + " GunForce Magnitude: " + Vector3.Magnitude(displaceForce));
                    setGunforceText(Vector3.Magnitude(displaceForce));
                    setGunForceArrow(displaceForce);

                }
                else
                {
                    gravObject = null;
                }

            }
            else
            {
                gravObject = null;
                setGunforceText(0);
                setGunForceArrow(Vector2.zero);
            }

            if ( gravObject)
            {
                gravObjectDistance += Input.mouseScrollDelta.y * GunRangeChangeRate;
            }
        }  
	}
    private void setGunForceArrow(Vector2 displacement)
    {
        if (!campaignHandler) campaignHandler = FindObjectOfType<CampaignHandler>();
        else
        {
            if (displacement == Vector2.zero)
            {
                if (campaignHandler.GunForceArrow.activeSelf) campaignHandler.GunForceArrow.SetActive(false);
            }
            else
            {
                if (!campaignHandler.GunForceArrow.activeSelf) campaignHandler.GunForceArrow.SetActive(true);
                campaignHandler.GunForceArrow.transform.up = displacement;
            }

            
        }
    }

    private void setGunforceText(float gunForce)
    {
        if (!campaignHandler) campaignHandler = FindObjectOfType<CampaignHandler>();
        else
        {
            campaignHandler.GunForceText.text = ((int)gunForce).ToString();

        }
    }

    private Vector2 calcPosition(float Angle, Vector2 Displacement)
    {
        Angle = Angle * Mathf.PI / 180f;
        float theta = Mathf.Atan2(Displacement.y,Displacement.x);
        
        float R = Mathf.Abs(Displacement.y / Mathf.Sin(theta));

        Vector2 returnDisplacement;
        Angle = Angle - theta;
        Debug.Log(theta + " Angle: " + Angle);
        

        returnDisplacement = new Vector2(R * Mathf.Cos(Angle), R * Mathf.Sin(Angle));

        return returnDisplacement;

    }

    public void EquipGun(Transform player)
    {
        transform.position = player.transform.position + new Vector3(0, HeightOffset, 0);
        transform.parent = player.transform;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Pickup)
        {
            CampaignHandler ch = FindObjectOfType<CampaignHandler>();
            if (ch.HasGravGun) Destroy(gameObject);
 
            EquipGun(collision.transform);
            Pickup = false;
            Active = true;

            ch.HasGravGun = true;
        }
    }
}
