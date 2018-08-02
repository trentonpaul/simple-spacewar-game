using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_OnLeave : NetworkManager {

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        HashSet<NetworkInstanceId> hSet = conn.clientOwnedObjects;
        NetworkInstanceId instanceId = NetworkInstanceId.Invalid;
        foreach (NetworkInstanceId id in hSet)
        {
            instanceId = id;
        }
        foreach (GameObject ship in GameObject.FindGameObjectsWithTag("Player")) {
            if (ship.GetComponent<Player_ID>().playerNetID.ToString() == instanceId.ToString())
            {
                ship.GetComponent<ColorSquareManager>().TransmitChange(ship.GetComponent<Ship>().GetShipID(), true);
            }
        }
        base.OnClientDisconnect(conn);

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
