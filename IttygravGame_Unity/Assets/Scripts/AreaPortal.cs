using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPortal : MonoBehaviour {
    public string PortalId = "";
    public string SceneTarget;
    public string PortalTarget;
    public bool SpawnedFrom = false;
    public bool SpawnAtOnly = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !SpawnedFrom && !SpawnAtOnly)
        {
            FindObjectOfType<CampaignHandler>().LoadNewScene(SceneTarget, PortalTarget);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            SpawnedFrom = false;
        }
    }
}
