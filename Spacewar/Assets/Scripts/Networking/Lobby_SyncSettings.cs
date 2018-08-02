using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lobby_SyncSettings : NetworkBehaviour {

    //[SyncVar(hook = "OnTotalLivesChanged")] int syncTotalLives;
    [SyncVar] [SerializeField] bool heavyGravity = false;
    [SyncVar] [SerializeField] bool twoSuns = false;

    public override void OnStartLocalPlayer()
    {
        CmdRequestTotalLivesOnJoin();
        CmdRequestHeavyGravityOnJoin();
        CmdRequestTwoSunsOnJoin();
    }

    //lives
    public void TransmitTotalLives(int lives)
    {
        if (isServer)
        {
            RpcTransmitTotalLivesToClients(lives);
        }
    }

    [ClientRpc]
    void RpcTransmitTotalLivesToClients(int lives)
    {
        OnTotalLivesChanged(lives);
    }

    void OnTotalLivesChanged(int lives)
    {
        GetComponent<Ship>().SetLives(lives);
        if (isLocalPlayer)
        {
            GetComponent<Player_SyncLives>().TransmitLives(lives);
        } else
        {
            GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject ship in ships)
            {
                if (ship.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    ship.GetComponent<Ship>().SetLives(lives);
                    ship.GetComponent<Player_SyncLives>().TransmitLives(lives);
                }
            }
        }
        GameObject.Find("LivesInputField").GetComponent<InputField>().text = lives.ToString();
    }

    [Command]
    void CmdRequestTotalLivesOnJoin()
    {
        if (isServer)
        {
            InputField livesInputField = GameObject.Find("LivesInputField").GetComponent<InputField>();
            int sendData = int.Parse(livesInputField.text);
            if (livesInputField.text != "") TransmitTotalLives(sendData);
        }
    }

    //heavyGravity
    public void TransmitHeavyGravity(bool heavyGravity)
    {
        if (isServer)
        {
            RpcTransmitHeavyGravityToClients(heavyGravity);
            OnHeavyGravityChanged(heavyGravity);
        }
    }

    [ClientRpc]
    void RpcTransmitHeavyGravityToClients(bool heavyGravity)
    {
        OnHeavyGravityChanged(heavyGravity);
    }

    void OnHeavyGravityChanged(bool heavyGravity)
    {
        if (!isServer)
        {
            GameController.instance.heavyGravity = heavyGravity;
            UpdateToggleCorrectly(GameObject.Find("ToggleGravity").GetComponent<Toggle>(), heavyGravity);
        } else
        {
            GameController.instance.heavyGravity = heavyGravity;
        }
    }

    [Command]
    void CmdRequestHeavyGravityOnJoin()
    {
        if (isServer)
        {
            TransmitHeavyGravity(GameObject.Find("ToggleGravity").GetComponent<Toggle>().isOn);
        }
    }

    //two suns
    public void TransmitTwoSuns(bool twoSuns)
    {
        if (isServer)
        {
            RpcTransmitTwoSunsToClients(twoSuns);
            OnTwoSunsChanged(twoSuns);
        }
    }

    [ClientRpc]
    void RpcTransmitTwoSunsToClients(bool twoSuns)
    {
        OnTwoSunsChanged(twoSuns);
    }

    void OnTwoSunsChanged(bool twoSuns)
    {
        if (!isServer)
        {
            GameController.instance.twoSuns = twoSuns;
            UpdateToggleCorrectly(GameObject.Find("ToggleSuns").GetComponent<Toggle>(), twoSuns);
        }
            
        else
        {
            GameController.instance.twoSuns = twoSuns;
        }
    }

    [Command]
    void CmdRequestTwoSunsOnJoin()
    {
        if (isServer)
        {
            TransmitTwoSuns(GameObject.Find("ToggleSuns").GetComponent<Toggle>().isOn);
        }
    }

    void UpdateToggleCorrectly(Toggle toggle, bool state)
    {
        if (toggle.isOn != state)
        {
            toggle.isOn = !toggle.isOn;
        }
    }
}
