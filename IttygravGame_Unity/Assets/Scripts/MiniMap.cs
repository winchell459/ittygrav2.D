using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour {
    public RawImage Map;
    public RawImage Pawn;
    public GameObject Player;

    private Vector2 PlayerOrigin;
    private Vector2 PawnOrigin;
    public Vector2 MapScale = new Vector2(1, 1);
    public Vector2 PawnOffset = new Vector2(100, 100);

	// Use this for initialization
	void Start () {
        PlayerOrigin = (Vector2)Player.transform.position;
        PawnOrigin = Pawn.rectTransform.rect.position;
	}
	
	// Update is called once per frame
	void Update () {
        float PawnY = Player.transform.position.y  * MapScale.y + PawnOrigin.y + PawnOffset.y;
        float PawnX = Player.transform.position.x  * MapScale.x + PawnOrigin.x + PawnOffset.x;

        Pawn.rectTransform.localPosition = new Vector2(PawnX, PawnY);
    }
}
