using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {
    
    private BoxCollider2D boxCollider2D;
    public float horizontalLength;

	// Use this for initialization
	void Start () {
        boxCollider2D = GetComponent<BoxCollider2D>();
        horizontalLength = boxCollider2D.size.x;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x <= -horizontalLength)
        {
            RepositionBackground();
        }
	}

    private void RepositionBackground()
    {
        Vector2 groundOffset = new Vector2(horizontalLength * 2f, 0f);
        transform.position = (Vector2)transform.position + groundOffset;
    }
}
