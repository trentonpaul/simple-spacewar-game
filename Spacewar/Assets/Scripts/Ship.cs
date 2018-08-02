using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Ship : NetworkBehaviour {

    //private Countdown countdownScript;
    private Rigidbody2D rb2d;
    private Countdown countdownScript;
    private Animator animator;
    private bool spawnedInPos = false;
    private GameObject invulnerableShield;
    private float loseLifeDelay = 0f;

    [SerializeField] int lives = 10;
    [SerializeField] bool invulnerable = false;

    public string action = "Neutral";
    public int shipID;
    public string playerName;
    public bool isAlive = true;
    public float speed, maxSpeed;
    public float fireRate, nextFire = 0.0f;
    public GameObject[] bullets;
    public Color shipColor;
    public Vector3[] spawnPositions;
    public Quaternion[] spawnRotations;

    // Use this for initialization
    void Start ()
    {

        //countdownScript = GetComponent<Countdown>();
        DontDestroyOnLoad(gameObject);
        countdownScript = GetComponent<Countdown>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        transform.GetChild(2).GetComponent<SpriteRenderer>().color = shipColor;
        transform.position = new Vector3(100, 0, 0);
        shipID = GameController.instance.highestShipID + 1;
        GameController.instance.highestShipID++;
        invulnerableShield = transform.GetChild(3).gameObject;
        invulnerableShield.SetActive(false);
    }

    // Update is called once per frame  
    void FixedUpdate ()
    {

        invulnerableShield.SetActive(invulnerable); //show that ship is invulnerable for other players, too

        if (GameController.instance.gameState == 3)
        {
            transform.rotation = Quaternion.identity;
            rb2d.velocity = Vector3.zero;
            rb2d.angularVelocity = 0f;
            return;
        }

        if (!isLocalPlayer)
        {
            return;
        }

        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            return;
        }

        if (lives <= 0)
        {
            return;
        }

        if (!spawnedInPos)
        {
            Spawn();
            spawnedInPos = true;
        }

        if (invulnerable && !countdownScript.ContainsCountdown("Invulnerable"))
        {
            invulnerable = false;
            GetComponent<Player_SyncLives>().TransmitInvulnerability(false);
        } else if (countdownScript.ContainsCountdown("Invulnerable") && !invulnerable)
        {
            invulnerable = true;
            GetComponent<Player_SyncLives>().TransmitInvulnerability(true);
        }

        if (loseLifeDelay > 0)
        {
            loseLifeDelay -= Time.deltaTime;
        }
        
        //check speed
        if (rb2d.velocity.magnitude > maxSpeed)
        {
            rb2d.AddForce(-rb2d.velocity * (rb2d.velocity.magnitude - maxSpeed));
        }
        //check speed end

        action = "Neutral";
        if (Input.GetKey(KeyCode.W))
        {
            action = "GoForward";
            if (Mathf.Abs(rb2d.velocity.magnitude) < maxSpeed)
            {
                rb2d.AddForce(transform.up * speed);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            action = "GoBackward";
            if (Mathf.Abs(rb2d.velocity.magnitude) < maxSpeed)
            {
                rb2d.AddForce(-transform.up * speed);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * 3);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * 3);
        }
        UpdateAnimator(action);

        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > nextFire && !countdownScript.ContainsCountdown("Invulnerable"))
            {
                CmdFireBullet();
                nextFire = Time.time + fireRate;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject)
        rb2d.freezeRotation = true;

        if (GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            if (collision.gameObject.tag == "Sun")
            {
                LoseLife(true);
            }
        }
    }

    [Command]
    void CmdFireBullet()
    {
        GameObject bullet = Instantiate(bullets[shipID-1], transform.GetChild(0).position, transform.rotation) as GameObject;
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetID(shipID);
        bullet.GetComponent<SpriteRenderer>().color = shipColor;
        NetworkServer.Spawn(bullet);
        //countdownScript.AddCountdown("fireDelay", .3f);
    }

    void UpdateAnimator(string action)
    {
        if (action.Equals("GoForward"))
        {
            animator.SetInteger("MoveDirection", 1);
        }
        else if (action.Equals("GoBackward"))
        {
            animator.SetInteger("MoveDirection", -1);
        }
        else if (action.Equals("Neutral"))
        {
            animator.SetInteger("MoveDirection", 0);
        }
    }

    void Spawn()
    {
        transform.position = spawnPositions[shipID - 1];
        transform.rotation = spawnRotations[shipID - 1];
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0f;
    }

    public void LoseLife(bool ignoreInvulnerable)
    {
        if (loseLifeDelay <= 0)
        {
            if (!countdownScript.ContainsCountdown("Invulnerable") || ignoreInvulnerable)
            {
                countdownScript.AddCountdown("Invulnerable", 2f);
                lives--;
                Spawn();
                GetComponent<Player_SyncLives>().TransmitLives(lives);
            }
            loseLifeDelay = 1f;
            if (lives <= 0)
            {
                transform.position = new Vector3(1000, 1000, 1000);
            }
        }
    }

    public void UpdateShipColor(Color color)
    {
        shipColor = color;
        transform.GetChild(2).GetComponent<SpriteRenderer>().color = color;
    }

    public void UpdatePlayerName(string name)
    {
        playerName = name;
        GetComponent<Player_ID>().TransmitName(name);
    }

    public void TransmitInfo()
    {
        GetComponent<Player_ID>().TransmitColor(shipColor);
        GetComponent<Player_ID>().TransmitName(playerName);
    }

    public int GetShipID() { return shipID; }

    public int GetLives()
    {
        return lives;
    }

    public void SetLives(int _lives)
    {
        lives = _lives;
    }

    public void SetInvulnerable(bool _invulnerable)
    {
        invulnerable = _invulnerable;
    }
    
}
