using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DisplayPlayer : MonoBehaviour {

    public Text text;

    void Start()
    {
        text.supportRichText = true;
    }

    void FixedUpdate()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        string finalText = "Players:\n";
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ship in ships)
        {
            finalText += "<color=#" + ColorUtility.ToHtmlStringRGBA(ship.GetComponent<Ship>().shipColor).ToLower() + ">" + ((ship.GetComponent<Ship>().playerName == "") ? "[Unnamed Player]" : ship.GetComponent<Ship>().playerName) + "</color>\n";
        }
        text.text = finalText;
    }
}
