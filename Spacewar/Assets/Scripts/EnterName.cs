using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EnterName : MonoBehaviour {

    public InputField inputField;
    public string input;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnNameEntered()
    {
        input = inputField.text.Trim();
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject ship in ships)
        {
            if (ship.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                ship.GetComponent<Ship>().UpdatePlayerName(input);
            }
        }
    }
}
