using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletCollisionManager : NetworkBehaviour {

    //[SyncVar] int shipIdHit = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [ClientCallback]
    public void TransmitShipHit(int id)
    {
        if (isServer) RpcProvideShipHitToClients(id);
        else CmdProvideShipHitToServer(id);
    }

    [Command]
    void CmdProvideShipHitToServer(int id)
    {
        RpcProvideShipHitToClients(id);
    }

    [ClientRpc]
    void RpcProvideShipHitToClients(int id)
    {
        OnShipHit(id);
    }

    void OnShipHit(int id)
    {
        if (id == GetLocalShip().GetComponent<Ship>().GetShipID())
        {
            GetLocalShip().GetComponent<Ship>().LoseLife(false);
        }
    }
    
    GameObject GetLocalShip()
    {
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ship in ships)
        {
            if (ship.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                return ship;
            }
        }
        return ships[0];
    }
}
