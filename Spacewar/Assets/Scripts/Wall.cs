using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    private BoxCollider2D boxCollider2D;

    public bool isHorizontal;
    public string objectDestination;

	// Use this for initialization
	void Start () {
        boxCollider2D = GetComponent<BoxCollider2D>();

        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        if (isHorizontal)
        {
            boxCollider2D.size = new Vector2(width, 1);
        }
        else
        {
            boxCollider2D.size = new Vector2(1, height);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("WallCollider"))
        {
            GameObject colliderParent = collider.transform.parent.gameObject;
            if (objectDestination.Equals("right"))
            {
                colliderParent.transform.position = new Vector2(7.9f, colliderParent.transform.position.y);
            }
            else if (objectDestination.Equals("left"))
            {
                colliderParent.transform.position = new Vector2(-7.9f, colliderParent.transform.position.y);
            }
            else if (objectDestination.Equals("top"))
            {
                colliderParent.transform.position = new Vector2(colliderParent.transform.position.x, 4.35f);
            }
            else if (objectDestination.Equals("bottom"))
            {
                colliderParent.transform.position = new Vector2(colliderParent.transform.position.x, -4.35f);
            }
            colliderParent.GetComponent<Rigidbody2D>().freezeRotation = true;
        }
    }
}
