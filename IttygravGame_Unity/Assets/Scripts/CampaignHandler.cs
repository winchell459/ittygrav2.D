using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignHandler : MonoBehaviour {
    public GameObject PlayerPrefab;
    private string portalName = "";
    public bool HasGravGun = false;
    public GameObject GravGunPrefab;

    // called first
    void OnEnable()
    {
        //Debug.Log("OnEnable called");
        
        if (FindObjectsOfType<CampaignHandler>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        AreaPortal[] portals = FindObjectsOfType<AreaPortal>();
        bool playerSpawned = false;
        foreach(AreaPortal ap in portals)
        {
            if (portalName.Equals(ap.PortalId))
            {
                ap.SpawnedFrom = true;
                GameObject player = Instantiate(PlayerPrefab, ap.transform.position, Quaternion.identity);
                CameraController cc = FindObjectOfType<CameraController>();
                cc.Player = player;
                cc.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, cc.transform.position.z);

                playerSpawned = true;
            }
        }

        if (!FindObjectOfType<Player>() && !playerSpawned)
        {
            CameraController cc = FindObjectOfType<CameraController>();
            Vector3 playerPos = new Vector3(cc.transform.position.x, cc.transform.position.y, 0);
            GameObject player = Instantiate(PlayerPrefab, playerPos, Quaternion.identity);
            cc.Player = player;
        }

        if(HasGravGun) SetGravGunActive(HasGravGun);
    }

    public void LoadNewScene(string SceneName, string PortalName)
    {
        portalName = PortalName;
        SceneManager.LoadScene(SceneName);
    }

    public void SetGravGunActive(bool state)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            Transform gun = player.transform.Find("GravGun");
            if (gun) gun.gameObject.SetActive(state);
            else
            {
                Debug.Log("Gun is missing!");
                gun = Instantiate(GravGunPrefab, player.transform.position, Quaternion.identity).transform;
                gun.GetComponent<GravityGun>().EquipGun(player.transform);
                gun.GetComponent<GravityGun>().Pickup = false;
                gun.GetComponent<GravityGun>().Active = state;
            }
        }
        else Debug.Log("Player is missing!");
    }
}
