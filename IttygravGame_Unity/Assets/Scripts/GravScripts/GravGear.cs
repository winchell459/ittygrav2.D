using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GravGear : GravObject {
    public float RotationSpeed = 25f;
    public float StepAngle = 45;
    private float startAngle;

    public bool rotating = false;
    private float targetAngle;
    private bool clockwiseRotation = true;
    public bool Rotating { get { return rotating; } }



    public bool Ready = true;

    public List<Transform> Slots;
    private Transform[] slotObjects;
    public float SlotsRange = 5;
    public LayerMask PlatformMask;

    public bool Attracting = false;
    public float AttractionSpeed = 10f;
    public float AttractionOffset = 1f;


    // Use this for initialization
    void Start () {
        startAngle = transform.eulerAngles.z;
        targetAngle = startAngle;
        slotObjects = new Transform[Slots.Count];
	}

    float rotationBuffer = 0.1f;
	// Update is called once per frame
	void Update () {

        handleRotation();
        handleAttraction();
        
	}
    private void handleAttraction()
    {
        if (Attracting)
        {
            //foreach(Transform so in slotObjects)
            //{
            //    if (so)
            //    {
            //        Vector2 targetPos = transform.up * AttractionOffset;
            //        so.gameObject.GetComponent<Rigidbody2D>().velocity = -.up * AttractionSpeed;

            //    }
            //}
            for(int i = 0; i < Slots.Count; i += 1)
            {
                if (slotObjects[i])
                {
                    Vector2 targetPos = Slots[i].up * AttractionOffset;
                    slotObjects[i].gameObject.GetComponent<Rigidbody2D>().velocity = -Slots[i].up * AttractionSpeed;
                }
            }
        }
    }
    private void handleRotation()
    {
        if (rotating)
        {
            float rotation = 0;
            float deltaRotation = Mathf.Abs(targetAngle - transform.eulerAngles.z);
            float fullRotation = RotationSpeed * Time.deltaTime;
            if (!clockwiseRotation)
            {
                
                //Debug.Log(deltaRotation + " Rotation: " + RotationSpeed * Time.deltaTime + " TargetAngle: " + targetAngle);
                if (deltaRotation > fullRotation)
                {
                    rotation = 1;
                }
                else
                {
                    rotation = (targetAngle - transform.eulerAngles.z) / RotationSpeed * Time.deltaTime;
                    rotating = false;
                }
            }
            else
            {
                if (deltaRotation > fullRotation)
                {
                    rotation = -1;
                }
                else
                {
                    rotation = (targetAngle - transform.eulerAngles.z) / RotationSpeed * Time.deltaTime;
                    rotating = false;
                }
            }

            if (rotation != 0) transform.Rotate(new Vector3(0, 0, rotation * RotationSpeed * Time.deltaTime));
            //Debug.Log(rotation);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);
            Ready = true;
        }
    }

    public void Rotate(bool clockwise)
    {
        
        if (rotating || !Ready) return;
        //Debug.Log("Rotation Time" + " rotating: " + rotating);
        if (clockwise) targetAngle = targetAngle - StepAngle;
        else targetAngle = targetAngle + StepAngle;
        //Debug.Log(targetAngle);
        rotating = true;

        if (targetAngle > 360) targetAngle -= 360;
        if (targetAngle < 0) targetAngle += 360;
        clockwiseRotation = clockwise;

        Ready = false;
    }

    public void CaptureBlock()
    {
        //for (int i = 0; i < 1; i += 1)
        for (int i = 0; i < Slots.Count; i+=1)
        {
            var raycastHit = Physics2D.Raycast(Slots[i].position, Slots[i].up, SlotsRange, PlatformMask);
            Debug.DrawRay(Slots[i].position, Slots[i].up * SlotsRange, Color.red);

            if (raycastHit)
            {
                if (raycastHit.transform.CompareTag("Box"))
                {
                    slotObjects[i] = raycastHit.transform;
                    Debug.Log(raycastHit.transform.name + " captured");
                }                 

            }
            else
            {
                slotObjects[i] = null;
            }
        }

        Attracting = true;
        
    }
}
