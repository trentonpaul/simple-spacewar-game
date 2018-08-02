using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorSquareManager : NetworkBehaviour {

    [SyncVar] public int syncSquareIdToChange;
    [SyncVar] public bool syncAvailabilityChange;

    public override void OnStartLocalPlayer()
    {
        CmdRequestChangesOnJoin();
    }

    void FixedUpdate()
    {
    }

    [Command]
    void CmdProvideChangeToServer(int ID, bool availability)
    {
        syncSquareIdToChange = ID;
        syncAvailabilityChange = availability;
        UpdateSquaresAccordingly(syncSquareIdToChange, syncAvailabilityChange);
    }

    [Command]
    void CmdRequestChangesOnJoin()
    {
        RpcRequestChangesOnJoin();
    }

    [ClientRpc]
    void RpcProvideChangeToClients(int ID, bool availability)
    {
        //Debug.Log("Received request with package [id= " + ID + ", avail= " + availability + "]");

        syncSquareIdToChange = ID;
        syncAvailabilityChange = availability;
        UpdateSquaresAccordingly(syncSquareIdToChange, syncAvailabilityChange);
    }

    [ClientRpc]
    void RpcRequestChangesOnJoin()
    {
        /*GameObject[] colorSquares = GameObject.FindGameObjectsWithTag("ColorSquare");
        foreach (GameObject colorSquare in colorSquares)
        {
            colorSquare.GetComponent<ColorSquare>().TransmitInformationToServer();
        }*/
        //Debug.Log("Responded to request with package [id= " + GameController.instance.currentSquareID + ", avail= False]");
        TransmitChange(GameController.instance.currentSquareID, false);
    }

    [ClientCallback]
    public void TransmitChange(int ID, bool availability)
    {
        //Debug.Log("Transmitting request with package [id= " + ID + ", avail= " + availability + "]");
        if (isServer) RpcProvideChangeToClients(ID, availability);
        else CmdProvideChangeToServer(ID, availability);
    }

    void UpdateSquaresAccordingly(int ID, bool availability)
    {
        GameObject[] colorSquares = GameObject.FindGameObjectsWithTag("ColorSquare");
        foreach (GameObject colorSquare in colorSquares)
        {
            if (colorSquare.GetComponent<ColorSquare>().GetID() == syncSquareIdToChange)
            {
                ColorSquare colorSquareScript = colorSquare.GetComponent<ColorSquare>();
                colorSquareScript.SetIsAvailable(syncAvailabilityChange);
                colorSquareScript.ChangeLook();
            }
        }
    }
}
