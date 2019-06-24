using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour {
    public GameObject BlockPrefab;
    public Transform BlockSpawnPoint;
    private List<GameObject> overlappingObjects = new List<GameObject>();

    public float DelaySpawn = 1;
    private float delaySpawnStart;
    private bool spawnStarted = false;
	
	// Update is called once per frame
	void Update () {
		if(overlappingObjects.Count <= 0)
        {
            if (!spawnStarted)
            {
                spawnStarted = true;
                delaySpawnStart = Time.fixedTime;
            }
            else if(delaySpawnStart + DelaySpawn < Time.fixedTime)
            {
                Instantiate(BlockPrefab, BlockSpawnPoint.position, Quaternion.identity);
                spawnStarted = false;
            }
            
        }
        else
        {
            delaySpawnStart = Time.fixedTime;
            spawnStarted = false;
        }

        //overlappingObjects.Clear();
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if(collision.CompareTag("Player") || collision.CompareTag("Box"))
        {
            Debug.Log("SpawnBlock Triggered");
            if(!overlappingObjects.Contains(collision.gameObject)) overlappingObjects.Add(collision.gameObject);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Box"))
        {
            
            if (overlappingObjects.Contains(collision.gameObject)) overlappingObjects.Remove(collision.gameObject);

        }
    }
}
