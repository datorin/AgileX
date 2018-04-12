using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Animator anim;
    Rigidbody2D rb;
    Vector2 move;
    Transform belt;
    Vector2 direction;

    [SerializeField] GameObject projectile;
    [SerializeField] float speed = 5;
    [SerializeField] float force = 20;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        belt = this.gameObject.transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
        move = new Vector2 (
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

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
}
