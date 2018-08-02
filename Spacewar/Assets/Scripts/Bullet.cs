using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : MonoBehaviour {
    
    private Countdown countdownScript;
    private SpriteRenderer spriteRenderer;
    private bool isOwnBullet = false;

    public float speed;
    public Color bulletColor;
    [SerializeField] int bulletID;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject ship in ships)
        {
            if (ship.GetComponent<Ship>().GetShipID() == bulletID)
            {
                bulletColor = ship.GetComponent<Ship>().shipColor;
            }
        }
        spriteRenderer.color = bulletColor;

        countdownScript = GetComponent<Countdown>();
        countdownScript.AddCountdown("lifeSpan", 2f);

        isOwnBullet = GetLocalShip().GetComponent<Ship>().GetShipID() == bulletID;
	}

    // Update is called once per frame
    void Update ()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        if (!countdownScript.ContainsCountdown("lifeSpan") || GameController.instance.gameState == 3)
        {
            Destroy(gameObject);
        }
	}

    /*void FixedUpdate()
    {
        if (isOwnBullet)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player" && hit.distance < .15f)
                {
                    OnBulletHitShip(hit.collider.gameObject);
                }
            }
        }
    }*/

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Equals("Wall") && !collision.gameObject.tag.Equals("WallCollider") && !collision.gameObject.tag.Equals("Gravity") && !collision.gameObject.tag.Equals("Bullet"))
        {
            if (collision.gameObject.tag.Equals("Player")) // if it's a ship
            {
                Ship ship = collision.gameObject.GetComponent<Ship>();
                if (ship.GetShipID() == bulletID) // if it's your own ship
                {
                    return;
                } else
                {
                    OnBulletHitShip(ship.gameObject);
                }
            }
            Destroy(gameObject);
        }
    }

    void OnBulletHitShip(GameObject ship)
    {
        if (ship.GetComponent<Ship>().GetShipID() != bulletID)
        {
            GetLocalShip().GetComponent<BulletCollisionManager>().TransmitShipHit(ship.GetComponent<Ship>().GetShipID());
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

    public int GetID() { return bulletID; }

    public void SetID(int ID) { bulletID = ID; }

    public void SetRotation(Quaternion rotation) { transform.rotation = rotation; }
}
