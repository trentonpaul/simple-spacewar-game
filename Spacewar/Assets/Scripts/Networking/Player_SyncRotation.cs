using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_SyncRotation : NetworkBehaviour {

    [SyncVar]
    private Quaternion syncPlayerRotation;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private float lerpRate = 15;

    private Quaternion lastPlayerRotation;
    private float theshold = 5;
	
	void FixedUpdate () {
        TransmitRotations();
        LerpRotations();
	}

    void LerpRotations()
    {
        if (!isLocalPlayer) playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
    }

    [Command]
    void CmdProvideRotationsToServer(Quaternion playerRotation)
    {
        syncPlayerRotation = playerRotation;
    }

    [ClientCallback]
    void TransmitRotations()
    {
        if (isLocalPlayer && Quaternion.Angle(playerTransform.rotation, lastPlayerRotation) > theshold)
        {
            CmdProvideRotationsToServer(playerTransform.rotation);
            lastPlayerRotation = playerTransform.rotation;
        }
    }
}
