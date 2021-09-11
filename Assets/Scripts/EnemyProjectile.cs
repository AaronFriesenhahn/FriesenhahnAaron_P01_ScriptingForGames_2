using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : ProjectileBehavior
{
    protected override void Move()
    {
        //fire straight forward
        rb.GetComponent<Rigidbody>().AddForce(transform.forward * thrust);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            //does damage to hit object
            IDamageable hit = (IDamageable)collision.gameObject.GetComponent(typeof(IDamageable));
            if (hit != null)
            {
                hit.TakeDamage(damage);
            }
            Debug.Log("Hit an object.");
            if (impactParticles != null)
            {
                impactParticles = Instantiate(impactParticles,
                    transform.position, Quaternion.identity);
            }
            //AudioHelper.PlayClip2D(impactSound, 1f);
            gameObject.SetActive(false);
            //StartCoroutine(DestroyOnImpact());
        }
        else if (collision.collider.tag != "Player")
        {
            Debug.Log("Hit an object.");
            if (impactParticles != null)
            {
                impactParticles = Instantiate(impactParticles,
                    transform.position, Quaternion.identity);
            }
            AudioHelper.PlayClip2D(impactSound, 1f);
            gameObject.SetActive(false);
            //StartCoroutine(DestroyOnImpact());
        }
    }
}
