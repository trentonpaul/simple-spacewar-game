using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyUIController : MonoBehaviour {

    public GameObject livesInputField;
    public GameObject strongGravityToggle;
    public GameObject twoSunsToggle;

	// Use this for initialization
	void Start () {
        StartCoroutine(CheckIfServer());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnLivesUpdated(string lives)
    {
        if (GetLocalShip().GetComponent<NetworkIdentity>().isServer)
        {
            InputField inputField = livesInputField.GetComponent<InputField>();
            if (inputField.text != "")
            {
                int livesToTransmit;
                bool success = int.TryParse(inputField.text, out livesToTransmit);
                if (success) GetLocalShip().GetComponent<Lobby_SyncSettings>().TransmitTotalLives(Mathf.Abs(livesToTransmit));
            }
        }
    }

    public void OnStrongGravityUpdated(bool heavyGravity)
    {
        if (GetLocalShip().GetComponent<NetworkIdentity>().isServer)
        {
            GetLocalShip().GetComponent<Lobby_SyncSettings>().TransmitHeavyGravity(heavyGravity);
        }
    }

    public void OnTwoSunsUpdated(bool twoSuns)
    {
        if (GetLocalShip().GetComponent<NetworkIdentity>().isServer)
        {
            GetLocalShip().GetComponent<Lobby_SyncSettings>().TransmitTwoSuns(twoSuns);
        }
    }

    IEnumerator CheckIfServer()
    {
        yield return new WaitForSeconds(.3f);
        if (GetLocalShip().GetComponent<NetworkIdentity>().isServer)
        {
            livesInputField.GetComponent<InputField>().readOnly = false;
            strongGravityToggle.GetComponent<Toggle>().interactable = true;
            twoSunsToggle.GetComponent<Toggle>().interactable = true;
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
