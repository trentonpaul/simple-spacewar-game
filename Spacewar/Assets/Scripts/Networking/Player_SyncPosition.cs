using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_SyncPosition : NetworkBehaviour {

    [SyncVar]
    private Vector2 syncPos;

    [SerializeField] private Transform myTransform;
    //[SerializeField] private float lerpRate = 5;

    private Vector2 lastPos;
    private float threshold = 0.02f;
    private float lerpProgress;
    private float currentDistance;
    private float fullDistance;
	
	// Update is called once per frame
	void FixedUpdate () {
        if (lerpProgress > 1)
        {
            lerpProgress = 1;
        }
        
        TransmitPosition();
        LerpPosition();
	}

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            currentDistance = Vector3.Distance(myTransform.position, syncPos);
            fullDistance = Vector3.Distance(lastPos, syncPos);
            if (fullDistance != 0)
            {
                lerpProgress = currentDistance / fullDistance;
            }
            /*print(syncPos + " " + (Vector2)myTransform.position);
            if (Mathf.Abs(myTransform.position.x - syncPos.x) > 5 && Mathf.Abs(myTransform.position.y - syncPos.y) > 5)
                myTransform.position = syncPos;
            else*/
            myTransform.position = Vector2.Lerp(myTransform.position, syncPos, lerpProgress);
        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector2 pos)
    {
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector2.Distance(myTransform.position, lastPos) > threshold)
        {
            CmdProvidePositionToServer(myTransform.position);
            lastPos = myTransform.position;
        }
    }
}
