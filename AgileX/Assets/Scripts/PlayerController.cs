using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private bool isHole = false;
    private bool isFalling = false;
    private bool isFire = false;
    private bool isCollision = false;

    [SerializeField] private GameObject sceneObject;
    private SceneController scene;

    public float energyDecreaseCoeficient = 1;
    public int maxEnergy = 100;
    public int maxDamage = 100;
    public int minDamage = 0;

    private Text pointsText;
    private int points;

    private Slider damageSlider;
    private float damage;

    private Slider energySlider;
    private float energy;
    private float stoppedTime;

    Animator anim;
    Rigidbody2D rb;
    Vector2 move;
    Transform belt;
    Vector2 direction;
    Vector3 holePos;

    [SerializeField] GameObject projectile;
    [SerializeField] float speed = 5;
    [SerializeField] float force = 20;
    [SerializeField] public int amunition = 5;

    public List<GameObject> places;
    public GameObject heart;
    public GameObject energies;
    public GameObject projectiles;

    public Text amunitionText;

    bool invulnerable = false;

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
                scene.GameOver(this);
                points = 0;
            }else
            {
                points = value;
                if(points >= scene.pointsToWin)
                {
                    scene.GameOver(this);
                }
            }
            
            pointsText.text = string.Format("{0} points", points);
        }
    }

    public float Energy
    {
        get
        {
            return energy;
        }

        set
        {
            if (value <= 0)
            {
                energy = 0;
            }
            else
            {
                energy = value > maxEnergy ? maxEnergy : value;
            }

            energySlider.value = energy;
        }
    }

    public float Damage
    {
        get
        {
            return damage;
        }

        set
        {
            if (value >= 100)
            {
                scene.GameOver(this);
                damage = 100;
            }
            else
            {
                damage = value < minDamage ? minDamage : value;
            }

            damageSlider.value = damage;
        }
    }

    void Awake()
    {
        scene = sceneObject.GetComponent<SceneController>();
        stoppedTime = 0;
    }

    // Use this for initialization
    void Start () {
        pointsText = GameObject.Find("PointsText").GetComponent<Text>();
        energySlider = GameObject.Find("EnergySlider").GetComponent<Slider>();
        energySlider.maxValue = maxEnergy;
        energySlider.minValue = 0;
        damageSlider = GameObject.Find("DamageSlider").GetComponent<Slider>();
        damageSlider.maxValue = maxDamage;
        damageSlider.minValue = 0;

        Points = scene.initialPoints;
        Energy = maxEnergy;
        Damage =  minDamage;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        belt = this.gameObject.transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {

        move = Energy == 0 ? Vector2.zero : new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        var random = UnityEngine.Random.Range(0, 500);

        if(random == 4)
        {
            var randomPlace = UnityEngine.Random.Range(0, places.Count - 1);
            Instantiate(heart, places[randomPlace].transform.position + new Vector3(1,0,0), Quaternion.identity);
        }

        if (random == 7)
        {
            var randomPlace = UnityEngine.Random.Range(0, places.Count - 1);
            Instantiate(energies, places[randomPlace].transform.position - new Vector3(1, 0, 0), Quaternion.identity);
        }

        if (random == 11)
        {
            var randomPlace = UnityEngine.Random.Range(0, places.Count - 1);
            Instantiate(projectiles, places[randomPlace].transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }

        if (isHole)
        {
            anim.SetTrigger("isHole");
            isHole = false;
            isFalling = true;
            StartCoroutine(waitHole(2));
        }

        if (isFalling)
        {
            transform.position = holePos;
            move = Vector2.zero;
        }

        if (isFire)
        {
            anim.SetTrigger("isFire");
            isFire = false;
        }

        if (isCollision)
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
            stoppedTime = 0;
        }else
        {
            anim.SetBool("isMoving", false);
            stoppedTime += Time.deltaTime;
            if (stoppedTime > 3)
            {
                Energy += 10;
                stoppedTime %= 3;
            }
        }

        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X))
        {
            if (amunition > 0)
            {
                GameObject projectileClone = (GameObject)Instantiate(projectile, belt.position, Quaternion.identity);
                projectileClone.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
                amunition = amunition - 1;
                amunitionText.text = ""+amunition;
            }
        }

        if (invulnerable)
        {
            StartCoroutine(waitInvulnerable(1));
        }
	}

    void FixedUpdate()
    {
        Vector2 moveVector = move * speed * Time.deltaTime;
        Energy -= energyDecreaseCoeficient * moveVector.magnitude;
        rb.MovePosition(rb.position + moveVector);
    }

    private IEnumerator waitHole(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isFalling = false;
        this.transform.position = new Vector3(0, 0, 0);
    }

    private IEnumerator waitInvulnerable(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        invulnerable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var coinController = collision.gameObject.GetComponent<CoinController>();
        if (coinController != null)
        {
            coinController.Disable(this);
        }

        string collisionTag = collision.gameObject.tag;
        switch(collisionTag)
        {
            case "Hole":
                if (!invulnerable)
                {
                    Points -= 10;
                    isHole = true;
                    holePos = collision.gameObject.transform.position + new Vector3(-1.5f,-0.5f,0);
                }
                invulnerable = true;
                break;
            case "Fire":
                if (!invulnerable)
                {
                    Points -= 10;
                    isFire = true;
                }
                invulnerable = true;
                break;
            case "Collision":
                if (!invulnerable)
                {
                    isCollision = true;
                }
                break;
            case "Enemy":
                if (!invulnerable)
                {
                    Damage += 10;
                    isFire = true;
                }
                invulnerable = true;
                break;
            case "Heart":
                Damage -= 50;
                Destroy(collision.gameObject);
                break;
            case "Energy":
                energy += 50;
                Destroy(collision.gameObject);
                break;
            case "Proj":
                amunition += 5;
                amunitionText.text = "" + amunition;
                Destroy(collision.gameObject);
                break;
        }
    }
}
