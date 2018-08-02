using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateColorSquares : MonoBehaviour {

    [SerializeField] GameObject colorSquarePrefab;
    [SerializeField] Vector2 startingPosition;
    [SerializeField] float gap;


	// Use this for initialization
	void Start () {
        List<Color> availableColors = new List<Color>(GameController.instance.availableColors.Keys);
        for (int i = 0; i < availableColors.Count; i++)
        {
            GameObject colorSquare = Instantiate(colorSquarePrefab, new Vector2(startingPosition.x + gap * i, startingPosition.y), transform.rotation) as GameObject;
            ColorSquare colorSquareScript = colorSquare.GetComponent<ColorSquare>();
            colorSquareScript.SetSquareColor(availableColors[i]);
            colorSquareScript.SetID(i);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
