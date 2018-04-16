using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Hole")
        {
            transform.parent.GetComponent<PlayerController>().isHole = true;
            transform.parent.GetComponent<PlayerController>().collDirection = collision.transform.position - this.transform.position;
        }
        if (collision.tag == "Fire")
        {
            transform.parent.GetComponent<PlayerController>().isFire = true;
            transform.parent.GetComponent<PlayerController>().collDirection = collision.transform.position - this.transform.position;
        }
        if (collision.tag == "Collision")
        {
            transform.parent.GetComponent<PlayerController>().isCollision = true;
            transform.parent.GetComponent<PlayerController>().collDirection = collision.transform.position - this.transform.position;
        }
    }
}
