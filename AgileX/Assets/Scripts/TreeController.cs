using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : EnemyController
{
    public GameObject projectile;

    private int force = 10;

    float angle;
    Vector3 rotation;

    bool isShooting = false;
    private double random = 4;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            rotation = target.transform.position - transform.position;
            angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            if(angle < 0)
            {
                angle += 360;
            }else if(angle > 360)
            {
                angle -= 360;
            }
            animator.SetFloat("Angle", angle);

            if (isShooting)
            {
                GameObject projectileClone = (GameObject)Instantiate(projectile, transform.position, Quaternion.identity);
                projectileClone.GetComponent<Rigidbody2D>().AddForce(rotation.normalized * force, ForceMode2D.Impulse);
                isShooting = false;
            }

            if(random == Random.Range(0, 100))
            {
                isShooting = true;
            }
        }
    }
}
