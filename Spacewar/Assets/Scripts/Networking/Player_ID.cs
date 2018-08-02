using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_ID : NetworkBehaviour {

    [SyncVar] public string playerUniqueName;
    [SyncVar(hook="OnNameChange")] public string playerInGameName;
    [SyncVar(hook = "OnColorChange")] public Color shipColor;// = Color.cyan;
    public NetworkInstanceId playerNetID;
    private Transform myTransform;

    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
        SetIdentity();
        Invoke("PickRandomColor", .2f);
    }

    // Use this for initialization
    void Awake () {
        myTransform = transform;
        Invoke("UpdateValues", .1f);
    }
	
	// Update is called once per frame
	void Update () {
		if (myTransform.name == "" || myTransform.name == "Ship(Clone)")
        {
            SetIdentity();
        }
	}

    void FixedUpdate()
    {
        //OnNameChange(playerInGameName);
        //OnColorChange(shipColor);
    }

    void SetIdentity()
    {
        if (!isLocalPlayer)
        {
            myTransform.name = playerUniqueName;
        } else
        {
            myTransform.name = MakeUniqueIdentity();
        }
    }

    [ClientCallback]
    void GetNetIdentity()
    {
        playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyIdentity(MakeUniqueIdentity());
    }

    [Command]
    void CmdTellServerMyIdentity(string name)
    {
        playerUniqueName = name;
    }

    //name
    public void TransmitName(string name)
    {
        if (isServer)
        {
            playerInGameName = name;
        }
        else
        {
            CmdSendServerMyName(name);
        }
    }

    [Command]
    void CmdSendServerMyName(string name)
    {
        TransmitName(name);
    }

    //color
    public void TransmitColor(Color color)
    {
        if (isServer)
        {
            shipColor = color;
        } else
        {
            CmdSendServerMyColor(color);
        }
    }

    [Command]
    void CmdSendServerMyColor(Color color)
    {
        TransmitColor(color);
    }

    /*Command]
    void CmdRequestChangesOnJoin()
    {
        RpcRequestChangesOnJoin();
    }

    [ClientRpc]
    void RpcRequestChangesOnJoin()
    {
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ship in ships)
        {
            ship.GetComponent<Ship>().TransmitInfo();
        }
    }*/

    string MakeUniqueIdentity()
    {
        string uniqueName = "Player " + playerNetID.ToString();
        return uniqueName;
    }

    void OnNameChange(string name)
    {
        if (!isLocalPlayer)
        {
            myTransform.gameObject.GetComponent<Ship>().playerName = name;
        }
    }

    void OnColorChange(Color color)
    {
        if (!isLocalPlayer)
        {
            myTransform.gameObject.GetComponent<Ship>().UpdateShipColor(color);
        }
    }

    void PickRandomColor()
    {
        GameObject[] colorSquares = GameObject.FindGameObjectsWithTag("ColorSquare");
        List<GameObject> availableColors = new List<GameObject>();
        foreach (GameObject colorSquare in colorSquares)
        {
            if (colorSquare.GetComponent<ColorSquare>().GetIsAvailable() == true)
            {
                availableColors.Add(colorSquare);
            }
        }
        ColorSquare randomColorSquare = availableColors[Random.Range(0, availableColors.Count)].GetComponent<ColorSquare>();
        randomColorSquare.SetIsSelected(true);
        randomColorSquare.SetIsAvailable(false);
        myTransform.gameObject.GetComponent<Ship>().UpdateShipColor(randomColorSquare.squareColor);
        myTransform.gameObject.GetComponent<Player_ID>().TransmitColor(randomColorSquare.squareColor);
        myTransform.gameObject.GetComponent<ColorSquareManager>().TransmitChange(randomColorSquare.GetID(), false);
        GameController.instance.currentSquareID = randomColorSquare.GetID();
        randomColorSquare.ChangeLook();
    }

    void UpdateValues()
    {
        if (!isLocalPlayer)
            myTransform.gameObject.GetComponent<Ship>().playerName = playerInGameName;
        if (!isLocalPlayer)
            myTransform.gameObject.GetComponent<Ship>().UpdateShipColor(shipColor);
    }
}
