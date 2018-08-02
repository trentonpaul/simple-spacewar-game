using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

    private bool created = false;
    private bool checkForEndGame = false;
    private bool gameEnd = false;
    private GameObject winner;

    public static GameController instance;
    public bool heavyGravity = false;
    public bool twoSuns = false;
    public bool secondSunSpawned = false;
    public Dictionary<Color, int> availableColors = new Dictionary<Color, int>();
    public List<string> playerNames = new List<string>();
    public int gameState; // 0 = not connected, 1 = in lobby, 2 = in game, 3 = in game but game over
    public float scrollSpeed = 20f;
    public int highestShipID = 0;
    public int currentSquareID = -1;

    // Use this for initialization
    void Awake ()
    {
        CreateColorDictionary();
        Application.targetFrameRate = 60;
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update () {
        if (checkForEndGame)
        {
            int shipsAlive = 0;
            GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject ship in ships)
            {
                if (ship.GetComponent<Ship>().GetLives() > 0)
                {
                    shipsAlive++;
                }
            }
            if (shipsAlive <= 1 && highestShipID != 1)
            {
                gameState = 3;
            } else if (shipsAlive == 0)
            {
                gameState = 3;
            }

            GameObject disconnectButton = GameObject.Find("ButtonDisconnect");

            if (shipsAlive == 1 && highestShipID == 1 && disconnectButton.GetComponent<RectTransform>().sizeDelta != new Vector2(106f, 22.5f))
            {
                disconnectButton.GetComponent<RectTransform>().sizeDelta = new Vector2(106f, 22.5f);
            }
        }
        if (gameState == 3 && !gameEnd)
        {
            gameEnd = true;
            GameObject ship = GetLocalShip();
            if (ship.GetComponent<Ship>().GetLives() > 0)
            {
                ship.transform.position = Vector3.zero;
                ship.transform.rotation = Quaternion.identity;
                ship.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                ship.GetComponent<Rigidbody2D>().angularVelocity = 0f;
            }

            //set winner text
            winner = GetWinner();
            winner.transform.localScale = new Vector3(3, 3, 3);
            Text winnerText = GameObject.Find("PlayerWinText").GetComponent<Text>();
            winnerText.text = ((winner.GetComponent<Ship>().playerName == "") ? "[Unnamed Player]" : winner.GetComponent<Ship>().playerName) + "\n\n\n\n\nWins";
            winnerText.color = winner.GetComponent<Ship>().shipColor;
            winnerText.gameObject.SetActive(true);

            //show disconnect text
            GameObject disconnectButton = GameObject.Find("ButtonDisconnect");
            disconnectButton.GetComponent<RectTransform>().sizeDelta = new Vector2(106f, 22.5f);
        }
	}

    void CreateColorDictionary()
    {
        availableColors.Add(new Color(239 / 255.0f, 91 / 255.0f, 91 / 255.0f), 0);
        availableColors.Add(new Color(239 / 255.0f, 160 / 255.0f, 91 / 255.0f), 0);
        availableColors.Add(new Color(239 / 255.0f, 214 / 255.0f, 91 / 255.0f), 0);
        availableColors.Add(new Color(189 / 255.0f, 239 / 255.0f, 91 / 255.0f), 0);
        availableColors.Add(new Color(118 / 255.0f, 239 / 255.0f, 91 / 255.0f), 0);
        availableColors.Add(new Color(91 / 255.0f, 239 / 255.0f, 140 / 255.0f), 0);
        availableColors.Add(new Color(91 / 255.0f, 239 / 255.0f, 206 / 255.0f), 0);
        availableColors.Add(new Color(91 / 255.0f, 216 / 255.0f, 239 / 255.0f), 0);
        availableColors.Add(new Color(91 / 255.0f, 145 / 255.0f, 239 / 255.0f), 0);
        availableColors.Add(new Color(108 / 255.0f, 91 / 255.0f, 239 / 255.0f), 0);
        availableColors.Add(new Color(150 / 255.0f, 91 / 255.0f, 239 / 255.0f), 0);
        availableColors.Add(new Color(211 / 255.0f, 91 / 255.0f, 239 / 255.0f), 0);
    }

    IEnumerator EnableCheckForEndGame()
    {
        yield return new WaitForSeconds(1);
        checkForEndGame = true;
    }

    IEnumerator ResendLives() // just in case they're out of sync
    {
        yield return new WaitForSeconds(.2f);
        GameObject localShip = GetLocalShip();
        localShip.GetComponent<Player_SyncLives>().TransmitLives(localShip.GetComponent<Ship>().GetLives());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                Destroy(gameObject);
            }
            if (SceneManager.GetActiveScene().name == "GameScene" && !checkForEndGame)
            {
                StartCoroutine(ResendLives());
                StartCoroutine(EnableCheckForEndGame());
            }
        } catch (Exception e)
        {
            Debug.Log(e.Message);
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

    GameObject GetWinner()
    {
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ship in ships)
        {
            if (ship.GetComponent<Ship>().GetLives() > 0)
            {
                return ship;
            }
        }
        return null;
    }
}
