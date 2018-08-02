using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorSquare : MonoBehaviour
{
    [SerializeField] bool isSelected = false;
    [SerializeField] bool isAvailable = true;
    [SerializeField] int squareID;
    private SpriteRenderer spriteRenderer;
    private GameObject[] children = { null, null };
    public Color squareColor;

    // Use this for initialization
    void Start()
    {
        children[0] = transform.GetChild(0).gameObject;
        children[1] = transform.GetChild(1).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = squareColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
        bool foundMismatch = true;
        foreach (GameObject ship in ships)
        {
            if (ship.GetComponent<Ship>().shipColor == squareColor)
            {
                foundMismatch = false;
            }
        }
        if (foundMismatch)
        {
            isAvailable = true;
            isSelected = false;
            ChangeLook();
        }*/
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnButtonPressed();
        }
    }

    public void OnButtonPressed()
    {
        if (isAvailable)
        {
            isAvailable = false;
            isSelected = true;

            GameObject playerShip = null;
            GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject ship in ships)
            {
                if (ship.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    playerShip = ship;
                }
            }
            //ship color
            playerShip.GetComponent<Ship>().UpdateShipColor(squareColor);
            playerShip.GetComponent<Player_ID>().TransmitColor(squareColor);

            playerShip.GetComponent<ColorSquareManager>().TransmitChange(squareID, isAvailable);
            
            if (GameController.instance.currentSquareID != -1)
            {
                GameObject[] colorSquares = GameObject.FindGameObjectsWithTag("ColorSquare");
                foreach (GameObject colorSquare in colorSquares)
                {
                    if (colorSquare.GetComponent<ColorSquare>().GetID() == GameController.instance.currentSquareID)
                    {
                        ColorSquare colorSquareScript = colorSquare.GetComponent<ColorSquare>();
                        colorSquareScript.SetIsAvailable(true);
                        colorSquareScript.SetIsSelected(false);
                        colorSquareScript.ChangeLook();
                        playerShip.GetComponent<ColorSquareManager>().TransmitChange(GameController.instance.currentSquareID, true);
                    }
                }
            }
            GameController.instance.currentSquareID = squareID;
            ChangeLook();
            
        }
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void SetSquareColor(Color _color)
    {
        squareColor = _color;
    }

    public void SetIsAvailable(bool _isAvaiable)
    {
        isAvailable = _isAvaiable;
        ChangeLook();
    }
    
    public bool GetIsAvailable()
    {
        return isAvailable;
    }

    public void SetIsSelected(bool _isSelected)
    {
        isSelected = _isSelected;
    }

    public int GetID()
    {
        return squareID;
    }

    public void SetID(int ID)
    {
        squareID = ID;
    }

    public void TransmitInformationToServer()
    {
        GameObject playerShip = null;
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ship in ships)
        {
            if (ship.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                playerShip = ship;
            }
        }

        if (playerShip != null)
        {
            playerShip.GetComponent<ColorSquareManager>().TransmitChange(squareID, isAvailable);
        }
    }

    public void ChangeLook()
    {
        if (isSelected)
        {
            children[0].SetActive(true);
        }
        else if (!isAvailable)
        {
            children[1].SetActive(true);
        }
        else
        {
            children[0].SetActive(false);
            children[1].SetActive(false);
        }
    }
}
