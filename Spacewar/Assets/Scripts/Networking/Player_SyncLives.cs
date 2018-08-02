using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_SyncLives : NetworkBehaviour {

    [SyncVar (hook = "OnLivesChanged")] [SerializeField] int syncLives;
    [SyncVar(hook = "OnInvulnerableChanged")] bool syncInvulnerable;

    //lives
    public void TransmitLives(int lives)
    {
        if (isServer)
        {
            syncLives = lives;
            OnLivesChanged(lives);
        }
        else
        {
            CmdSendServerMyLives(lives);
        }
    }

    [Command]
    void CmdSendServerMyLives(int lives)
    {
        TransmitLives(lives);
        OnLivesChanged(lives);
    }

    void OnLivesChanged(int lives)
    {
        GetComponent<Ship>().SetLives(lives);
    }

    //invulnerability
    public void TransmitInvulnerability(bool invulnerable)
    {
        if (isServer)
        {
            syncInvulnerable = invulnerable;
        }
        else
        {
            CmdSendServerMyInvulnerability(invulnerable);
        }
    }

    [Command]
    void CmdSendServerMyInvulnerability(bool invulnerable)
    {
        TransmitInvulnerability(invulnerable);
    }

    void OnInvulnerableChanged(bool invulnerable)
    {
        GetComponent<Ship>().SetInvulnerable(invulnerable);
    }

}
