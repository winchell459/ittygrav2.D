using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramBlock : MonoBehaviour {
    public PBOperation BlockOperation;
    public enum PBOperation
    {
        Activate,
        Deactivate,
        MoveForward,
        MoveBackwards,
        Pause
    }
    private List<ProgramBlock> touchingBlocks = new List<ProgramBlock>();

    private ProgramBlock nextBlock;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ProgramBlock"))
        {
            if (!touchingBlocks.Contains(collision.gameObject.GetComponent<ProgramBlock>()))
            {
                touchingBlocks.Add(collision.gameObject.GetComponent<ProgramBlock>());
            }
            
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ProgramBlock"))
        {
            if (touchingBlocks.Contains(collision.gameObject.GetComponent<ProgramBlock>()))
            {
                touchingBlocks.Remove(collision.gameObject.GetComponent<ProgramBlock>());
                if (nextBlock == collision.gameObject.GetComponent<ProgramBlock>()) nextBlock = null;
            }
            
        }
    }

    public ProgramBlock GetNextBlock(Transform programPad)
    {
        Vector2 relativeUpDirection = transform.position - programPad.position;
        relativeUpDirection /= Vector3.Magnitude(relativeUpDirection);

        float blockDistance = Vector3.Distance(transform.position, programPad.position);
        
        if(touchingBlocks.Count > 0)
        {
            float nextBlockDistance = Mathf.Infinity;
            for (int i = 0; i < touchingBlocks.Count; i += 1)
            {
                float newBlockDistance = Vector3.Distance(programPad.position, touchingBlocks[i].transform.position);
                if (newBlockDistance > blockDistance && newBlockDistance <= nextBlockDistance) nextBlock = touchingBlocks[i];
            }
        }
        

        return nextBlock;
    }
}
