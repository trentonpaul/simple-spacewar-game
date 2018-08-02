using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameDisplayPlayer : MonoBehaviour {

    public Text playerName, playerLives;
    public Sprite skullSprite;
    public int playerID;

    private bool keepRefreshing = true;

	// Use this for initialization
	void Start () {
        if (GameController.instance.highestShipID < playerID)
        {
            transform.GetChild(2).gameObject.SetActive(false);
        }
        playerName = transform.GetChild(0).GetComponent<Text>();
        playerLives = transform.GetChild(1).GetComponent<Text>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (GameController.instance.gameState == 3)
        {
            gameObject.SetActive(false);
        }
        if (keepRefreshing)
        {
            GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject ship in ships)
            {
                if (ship.GetComponent<Ship>().GetShipID() == playerID)
                {
                    Ship shipScript = ship.GetComponent<Ship>();
                    if (playerName.text == "")
                    {
                        playerName.color = shipScript.shipColor;
                        playerName.text = (shipScript.playerName == "") ? "[Unnamed Player]" : shipScript.playerName;
                    }
                    playerLives.color = shipScript.shipColor;
                    playerLives.text = shipScript.GetLives().ToString();
                    if (shipScript.GetLives() <= 0)
                    {
                        Transform lifeSprite = transform.GetChild(2);
                        lifeSprite.gameObject.GetComponent<SpriteRenderer>().sprite = skullSprite;
                        lifeSprite.position = new Vector2(lifeSprite.position.x, (playerID <= 2 ? lifeSprite.position.y + .05f : lifeSprite.position.y - .05f));
                        keepRefreshing = false;
                    }
                    break;
                }
            }
        }
    }
}
