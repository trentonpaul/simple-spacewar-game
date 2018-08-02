using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    private float lastColorChangeTime;
    private SpriteRenderer spriteRenderer;

    public float fadeDuration;
    public Color startColor;
    public Color endColor;
    public GameObject sun;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!GameController.instance.secondSunSpawned && GameController.instance.twoSuns)
        {
            Instantiate(sun, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z), transform.rotation);
            transform.position = new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z);
            GameController.instance.secondSunSpawned = true;
        }

        if (GameController.instance.heavyGravity && !GameController.instance.twoSuns)
        {
            transform.GetChild(0).GetComponent<PointEffector2D>().forceMagnitude -= 1;
            transform.GetChild(1).GetComponent<PointEffector2D>().forceMagnitude -= 1;
            transform.GetChild(2).GetComponent<PointEffector2D>().forceMagnitude -= 1;
        } else if (GameController.instance.heavyGravity && !GameController.instance.twoSuns)
        {
            transform.GetChild(0).GetComponent<PointEffector2D>().forceMagnitude -= .5f;
            transform.GetChild(1).GetComponent<PointEffector2D>().forceMagnitude -= .5f;
            transform.GetChild(2).GetComponent<PointEffector2D>().forceMagnitude -= .5f;
        }
	}

    void Update()
    {
        if (GameController.instance.gameState == 3)
        {
            gameObject.SetActive(false);
        }
        float ratio = (Time.time - lastColorChangeTime) / fadeDuration;
        ratio = Mathf.Clamp01(ratio);
        spriteRenderer.color = Color.Lerp(startColor, endColor, ratio);

        if (ratio == 1f)
        {
            lastColorChangeTime = Time.time;

            // Switch colors
            Color temp = startColor;
            startColor = endColor;
            endColor = temp;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        transform.Rotate(Vector3.back * 3);
    }
}
