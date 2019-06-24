using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprites : MonoBehaviour {
    public GameObject RightFoot;
    public GameObject LeftFoot;
    public GameObject pivotFoot;
    public GameObject floatingFoot;

    public Player Player;
    private CharacterController2D CC2D;


    [SerializeField]private float LegsLength = 1.0f;
    [SerializeField] private float NormalizeHeight = 0.5f;
    [SerializeField] private float MinimumHeight = 0.1f;
    private float defaultBodyHeight;

    private Transform Camera;

    private float rfDefaultY;
    private float lfDefaultY;
	// Use this for initialization
	void Start () {
        rfDefaultY = RightFoot.transform.localPosition.y;
        lfDefaultY = LeftFoot.transform.localPosition.y;

        pivotFoot = RightFoot;
        floatingFoot = LeftFoot;
        defaultBodyHeight = transform.localPosition.y;
        defaultBodyHeight = LegsLength / 2 - (Mathf.Abs(RightFoot.transform.localPosition.x) + Mathf.Abs(LeftFoot.transform.localPosition.x)) + RightFoot.transform.localPosition.y - rfDefaultY + LeftFoot.transform.localPosition.y - lfDefaultY;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!Camera) Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        transform.eulerAngles = Camera.transform.eulerAngles;

        if (!CC2D) CC2D = Player.gameObject.GetComponent<CharacterController2D>();

        //Set Foot placement
        handleFootPlacement(floatingFoot);

        //Set Body Possition
        handleBodyPlacement();
	}
    void handleBodyPlacement(){
        float x1 = RightFoot.transform.localPosition.x;
        float x2 = LeftFoot.transform.localPosition.x;
        float y1 = RightFoot.transform.localPosition.y - rfDefaultY;
        float y2 = LeftFoot.transform.localPosition.y - lfDefaultY;
        float y = LegsLength / 2 - (Mathf.Abs(x1) + Mathf.Abs(x2)) + y1 + y2;

        float deltaY = y / defaultBodyHeight;
        transform.localPosition = new Vector3(0, deltaY * NormalizeHeight + defaultBodyHeight, 0);
    }
    void handleFootPlacement(GameObject foot){
        float deltaX = Player.GetHorzontalDisplacement();


        if(transform.localPosition.y < MinimumHeight && Mathf.Sign(deltaX) == Mathf.Sign(foot.transform.localPosition.x)){
            transform.localPosition = new Vector3(transform.localPosition.x, 0.2f, 0);
            floatingFoot = pivotFoot;
            pivotFoot = foot;

            //foot.transform.localPosition = new Vector3(foot.transform.localPosition.x + deltaX, foot.transform.localPosition.y, 0);
            //pivotFoot.transform.localPosition = new Vector3(pivotFoot.transform.localPosition.x - deltaX, pivotFoot.transform.localPosition.y, 0);
        }else{
            foot.transform.localPosition = new Vector3(foot.transform.localPosition.x + deltaX, foot.transform.localPosition.y, 0);
            pivotFoot.transform.localPosition = new Vector3(pivotFoot.transform.localPosition.x - deltaX, pivotFoot.transform.localPosition.y, 0);
        }
    }
}
