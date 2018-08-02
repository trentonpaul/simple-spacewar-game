using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (gameObject.activeInHierarchy)
        {
            GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject ship in ships)
            {
                if (ship.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    if (!ship.GetComponent<NetworkIdentity>().isServer)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void OnStartGamePressed()
    {
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ship in ships)
        {
            if (ship.GetComponent<NetworkIdentity>().isLocalPlayer)
                ships[0].GetComponent<Player_SyncScene>().TransmitChange(true);
        }
    }
}
