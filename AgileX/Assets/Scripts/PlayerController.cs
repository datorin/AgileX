using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static int coins;

    public bool isHole = false;
    public bool isFire = false;
    public bool isCollision = false;
    public Vector3 collDirection;

    Animator anim;
    Rigidbody2D rb;
    Vector2 move;
    Transform belt;
    Vector2 direction;

    [SerializeField] GameObject projectile;
    [SerializeField] float speed = 5;
    [SerializeField] float force = 20;

    private void Awake()
    {
        coins = 0;
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        belt = this.gameObject.transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
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
            StartCoroutine(waitFire(.25f, collDirection));
            isFire = false;

        } else if (isCollision)
        {
            anim.SetTrigger("isCollision");
            StartCoroutine(waitCollision(.25f, collDirection));
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
	}

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.deltaTime);
    }

    private IEnumerator waitHole(float waitTime, Vector3 collDirection)
    {
        yield return new WaitForSeconds(waitTime);
        this.transform.position = new Vector3(0, 0, 0);
        isHole = false;
    }

    private IEnumerator waitCollision(float waitTime, Vector3 collDirection)
    {
        yield return new WaitForSeconds(waitTime);
        this.transform.position = this.transform.position + collDirection.normalized;
        isCollision = false;
    }

    private IEnumerator waitFire(float waitTime, Vector3 collDirection)
    {
        yield return new WaitForSeconds(waitTime);
        this.transform.position = this.transform.position + collDirection.normalized;
        isFire = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Coin")
        {
            coins++;
            Destroy(collision.gameObject);
        }
        Debug.Log("Coins: " + coins);
    }
}
