using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramPad : MonoBehaviour {

    public GravGear MyGravGear;
    public List<GameObject> programBlocks = new List<GameObject>();
    private ProgramBlock currentBlock;
    public bool Clockwise = true;
    public float SequenceTime = 2.0f;
    private float lastSequence = Mathf.NegativeInfinity;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (currentBlock && MyGravGear.Ready)
        {
            if(currentBlock.BlockOperation == ProgramBlock.PBOperation.MoveForward)
            {
                MyGravGear.Rotate(true);
            }else if (currentBlock.BlockOperation == ProgramBlock.PBOperation.MoveBackwards)
            {
                MyGravGear.Rotate(false);
            }else if (currentBlock.BlockOperation == ProgramBlock.PBOperation.Activate)
            {
                MyGravGear.CaptureBlock();
            }
            else if (currentBlock.BlockOperation == ProgramBlock.PBOperation.Deactivate)
            {
                MyGravGear.Attracting = false;
            }
            else
            {

            }
            currentBlock = currentBlock.GetNextBlock(transform);
            if (!currentBlock) currentBlock = programBlocks[0].GetComponent<ProgramBlock>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("ProgramBlock") && !programBlocks.Contains(collision.gameObject)) 
        {
            programBlocks.Add(collision.gameObject);

            currentBlock = programBlocks[0].GetComponent<ProgramBlock>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        programBlocks.Remove(collision.gameObject);

        if (programBlocks.Count > 0) currentBlock = programBlocks[0].GetComponent<ProgramBlock>();
        else currentBlock = null;
    }
}
