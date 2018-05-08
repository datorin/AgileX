using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bat_controller : MonoBehaviour {

    [SerializeField] float speed = 5f;

    public GameObject target;
    Vector3 targetPos;
    Vector3 move;

    Rigidbody2D rb;
    Animator anim;

	// Use this for initialization
	void Awake () {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if(target != null) {
            targetPos = target.transform.position;

            move = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            transform.position = move;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("isAttacking");
        }
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            Destroy(this.gameObject);
        }
    }
}
