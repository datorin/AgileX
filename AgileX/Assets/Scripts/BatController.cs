using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : EnemyController {

    [SerializeField] float speed = 5f;
	
	// Update is called once per frame
	void Update () {
        if(target != null) {
            var targetPos = target.transform.position;

            var move = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            transform.position = move;
        }
	}

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("isAttacking");
        }
        base.OnTriggerEnter2D(collision);
    }
}
