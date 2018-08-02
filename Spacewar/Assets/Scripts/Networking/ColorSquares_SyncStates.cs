using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorSquares_SyncStates : NetworkBehaviour {

    [SyncVar (hook = "UpdateAvailability")] bool syncIsAvaiable = true;

    private ColorSquare colorSquare;
    private bool prevAvailability;
    // Use this for initialization
    void Start () {
        colorSquare = GetComponent<ColorSquare>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        TransmitAvailability();
	}

    [ClientCallback]
    void TransmitAvailability()
    {
        if (isLocalPlayer && colorSquare.GetIsAvailable() != prevAvailability)
        {
            CmdProvideAvailabilityToServer(colorSquare.GetIsAvailable());
            prevAvailability = colorSquare.GetIsAvailable();
        }
    }

    [Command]
    void CmdProvideAvailabilityToServer(bool availability)
    {
         syncIsAvaiable = availability;
    }

    void UpdateAvailability(bool availability)
    {
        Debug.Log("Updating");
        colorSquare.SetIsAvailable(availability);
        colorSquare.ChangeLook();
    }
}
