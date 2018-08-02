using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Player_SyncScene : NetworkBehaviour {

    [SyncVar (hook = "OnSceneChange")] public bool changeToGameScene = false;


    [ClientCallback]
    public void TransmitChange(bool changeScene)
    {
        if (isServer) RpcSendSceneChangeToClient();
        else CmdSendSceneChangeToServer();
    }

    [Command]
    void CmdSendSceneChangeToServer()
    {
        RpcSendSceneChangeToClient();
    }

    [ClientRpc]
    void RpcSendSceneChangeToClient()
    {
        OnSceneChange(true);
    }

    void OnSceneChange(bool changeScene)
    {
        if (changeScene)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
