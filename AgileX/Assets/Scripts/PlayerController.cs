using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public bool isHole = false;
    public bool isFire = false;
    public bool isCollision = false;
    public Vector3 collDirection;
    public int initialPoints = 0;
    public uint pointsToWin = 70;
    public int totalSeconds = 60;
    public int coinsEachRound = 2;
    public float secondsBetweenRounds = 5.0f;

    private Text countDownText;
    private GameObject endTextObject;
    private TimeSpan countdown;

    private Text pointsText;
    private int points = 0;

    private GameObject[] coinObjects;

    Animator anim;
    Rigidbody2D rb;
    Vector2 move;
    Transform belt;
    Vector2 direction;

    [SerializeField] GameObject projectile;
    [SerializeField] float speed = 5;
    [SerializeField] float force = 20;

    bool invulnerable = false;

    public double Countdown
    {
        get
        {
            return countdown.TotalSeconds;
        }

        set
        {
            countdown = TimeSpan.FromSeconds(value);
            countDownText.text = string.Format("{0:D2}:{1:D2}", countdown.Minutes, countdown.Seconds);
        }
    }

    public int Points
    {
        get
        {
            return points;
        }

        set
        {
            if(value <= 0)
            {
                points = 0;
            }else
            {
                points = value;
            }
            
            pointsText.text = string.Format("{0} points", points);
        }
    }

    // Use this for initialization
    void Start () {
        pointsText = GameObject.Find("PointsText").GetComponent<Text>();
        countDownText = GameObject.Find("CountDownText").GetComponent<Text>();
        endTextObject = GameObject.Find("EndText");

        Points = initialPoints;

        coinObjects = GameObject.FindGameObjectsWithTag("Coin");
        if (coinsEachRound > coinObjects.Length)
        {
            coinsEachRound = coinObjects.Length;
        }
        foreach(var coin in coinObjects)
        {
            coin.SetActive(false);
        }
        InvokeRepeating("ActivateCoins", 2.0f, secondsBetweenRounds);

        endTextObject.SetActive(false);
        endTextObject.GetComponent<Text>().enabled = false;
        countdown = TimeSpan.FromSeconds(totalSeconds);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        belt = this.gameObject.transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
        if (Countdown > 0)
        {
            Countdown -= Time.deltaTime;
        }
        else if (!endTextObject.activeSelf)
        {
            GameOver();
        }
        if(!isHole && !isCollision && !isFire)
        {
            move = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );
        }else if (isHole)
        {
            anim.SetTrigger("isHole");
            StartCoroutine(waitHole(2, collDirection));
        }else if (isFire)
        {
            anim.SetTrigger("isCollision");
            isFire = false;

        } else if (isCollision)
        {
            anim.SetTrigger("isCollision");
            isCollision = false;
        }

        if (move != Vector2.zero)
        {
            direction = move.normalized;
            anim.SetFloat("MoveX", move.x);
            anim.SetFloat("MoveY", move.y);
            anim.SetBool("isMoving", true);
        }else
        {
            anim.SetBool("isMoving", false);
        }

        if(Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X))
        {
            GameObject projectileClone = (GameObject)Instantiate(projectile, belt.position, Quaternion.identity);
            projectileClone.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        }

        if (invulnerable)
        {
            StartCoroutine(waitInvulnerable(1));
        }
	}

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.deltaTime);
    }

    void ActivateCoins()
    {
        for(int i = 0; i < coinsEachRound; i++)
        {
            int index = UnityEngine.Random.Range(0, coinObjects.Length - 1);
            int checks = 0;
            while (coinObjects[index].activeSelf)
            {
                index++;
                checks++;
                if (checks >= coinObjects.Length)
                {
                    return;
                }
            }
            int type = UnityEngine.Random.Range(0, 3);
            coinObjects[index].SetActive(true);
            switch(type) {
                case 0:
                    coinObjects[index].tag = "CoinBronze";
                    coinObjects[index].GetComponent<Animator>().SetBool("isBronce",true);
                    coinObjects[index].GetComponent<Animator>().SetBool("isSilver",false);
                    coinObjects[index].GetComponent<Animator>().SetBool("isGold",false);
                    break;
                case 1:
                    coinObjects[index].tag = "CoinSilver";
                    coinObjects[index].GetComponent<Animator>().SetBool("isBronce",false);
                    coinObjects[index].GetComponent<Animator>().SetBool("isSilver",true);
                    coinObjects[index].GetComponent<Animator>().SetBool("isGold",false);
                    break;
                case 2:
                    coinObjects[index].tag = "CoinGold";
                    coinObjects[index].GetComponent<Animator>().SetBool("isBronce",false);
                    coinObjects[index].GetComponent<Animator>().SetBool("isSilver",false);
                    coinObjects[index].GetComponent<Animator>().SetBool("isGold",true);
                    break;
            }
        }
    }

    private void GameOver()
    {
        endTextObject.SetActive(true);
        var endText = endTextObject.GetComponent<Text>();
        endText.text = Points < pointsToWin ? "Game Over :(" : "You Win! :D";
        endText.enabled = true;
        Destroy(gameObject);
    }

    private IEnumerator waitHole(float waitTime, Vector3 collDirection)
    {
        yield return new WaitForSeconds(waitTime);
        this.transform.position = new Vector3(0, 0, 0);
        isHole = false;
    }

    private IEnumerator waitInvulnerable(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        invulnerable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.gameObject.tag;
        switch(collisionTag)
        {
            case "CoinBronze":
                Points += 5;
                collision.gameObject.SetActive(false);
                break;
            case "CoinSilver":
                Points += 10;
                collision.gameObject.SetActive(false);
                break;
            case "CoinGold":
                Points += 15;
                collision.gameObject.SetActive(false);
                break;
            case "Hole":
                if (!invulnerable)
                {
                    Points -= 10;
                }
                invulnerable = true;
                break;
            case "Fire":
                if (!invulnerable)
                {
                    Points -= 10;
                }
                invulnerable = true;
                break;
        }
        Debug.Log("Coins: " + Points);
    }
}
