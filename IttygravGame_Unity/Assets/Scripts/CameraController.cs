using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject Player;
    public float RotationSmoothing = 10;
    public float FollowSmoothing = 10;
    public Vector2 FollowOffset;
    public bool FollowPlayer = true;
    public PointOfInterest POI;

    public GameObject AttentionArrow;
    public GameObject Lever;

    public LayerMask CameraMask;
    public bool DisplayArrow = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (FollowPlayer)
        {
            DisplayArrow = true;
            followPlayer();
            if (POI) if(POI.ZoomIn) zoomIn();
        }
        else
        {
            panOut();
            DisplayArrow = false;
        }

        //
        if (DisplayArrow)
        {
            handleAttensionArrow();
            //AttensionArrow.SetActive(true);
        }
        else
        {
            if(AttentionArrow) AttentionArrow.SetActive(false);
        }
        
        
        
        
	}
    private void panOut()
    {


        //Debug.Log(GetComponent<Camera>().orthographicSize);

        float dollyRate = Vector3.Distance(transform.position, POI.PanTarget.transform.position) * POI.ZoomRate / (POI.CameraSize - GetComponent<Camera>().orthographicSize);

        if (GetComponent<Camera>().orthographicSize < POI.CameraSize)
        {
            GetComponent<Camera>().orthographicSize = GetComponent<Camera>().orthographicSize + POI.ZoomRate * Time.deltaTime;
        }
        else
        {
            GetComponent<Camera>().orthographicSize = POI.CameraSize;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, POI.PanTarget.transform.rotation, RotationSmoothing * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, POI.PanTarget.transform.position, POI.PanRate * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, POI.PanTarget.transform.position, dollyRate * Time.deltaTime);
    }

    private void zoomIn()
    {
        if(GetComponent<Camera>().orthographicSize > POI.ZoomedCameraSize)
        {
            GetComponent<Camera>().orthographicSize = GetComponent<Camera>().orthographicSize - POI.ZoomRate * Time.deltaTime;
        }
        else
        {
            GetComponent<Camera>().orthographicSize = POI.ZoomedCameraSize;
            POI = null;
        }
        
    }

    private void followPlayer()
    {
        //move towards player
        float cameraAngle = transform.eulerAngles.z * Mathf.PI / 180;
        float xOffset = Mathf.Cos(cameraAngle) * FollowOffset.x + Mathf.Cos(cameraAngle + Mathf.PI / 2) * FollowOffset.y;
        float yOffset = Mathf.Sin(cameraAngle) * FollowOffset.x + Mathf.Sin(cameraAngle + Mathf.PI / 2) * FollowOffset.y;
        Vector3 newCameraPosition = new Vector3(Player.transform.position.x + xOffset, Player.transform.position.y + yOffset, transform.position.z);
        transform.position = Vector3.Lerp(newCameraPosition, transform.position, FollowSmoothing * Time.deltaTime);

        //rotate camera
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Player.transform.rotation, RotationSmoothing * Time.deltaTime);

    }
    float radius = 2;
    void handleAttensionArrow()
    {
        if (AttentionArrow)
        {
            float y = Lever.transform.position.y;
            float x = Lever.transform.position.x;

            y = y - Player.transform.position.y;
            x = x - Player.transform.position.x;

            float zRot = Mathf.Atan(y / x) * 180 / Mathf.PI;

            //AttensionArrow.transform.eulerAngles = new Vector3(0, 0, zRot);
            AttentionArrow.transform.right = new Vector2(x, y);

            //calculate distance
            var rayVector = (Vector2)Player.transform.position;
            var rayDirection = new Vector2(x, y);

            Debug.DrawRay(rayVector, rayDirection);

            //LayerMask cameraScreen = LayerMask.NameToLayer("Camera");

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, 100, CameraMask);
            if (raycastHit)
            {
                float edgeY = raycastHit.point.y;
                float edgeX = raycastHit.point.x;
                radius = Mathf.Sin(zRot * Mathf.PI / 180) / edgeY;
                //Debug.Log(raycastHit.transform.name + "(" + edgeX + ", " + edgeY + ")");

                AttentionArrow.transform.position = new Vector3(edgeX, edgeY, 0);

                if(Vector3.Distance(Player.transform.position, new Vector3(edgeX, edgeY, 0)) > Vector3.Distance(Player.transform.position, Lever.transform.position))
                {
                    AttentionArrow.gameObject.SetActive(false);
                }
                else
                {
                    AttentionArrow.gameObject.SetActive(true);
                }

            }
            //offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - raycastHit.point.x));


            //float arrowY = Player.transform.position.y + radius * Mathf.Sin(zRot * Mathf.PI / 180);
            //float arrowX = Player.transform.position.x + radius * Mathf.Cos(zRot * Mathf.PI / 180);

            //AttensionArrow.transform.position = new Vector3(arrowX, arrowY, 0);
        }
    }
}
