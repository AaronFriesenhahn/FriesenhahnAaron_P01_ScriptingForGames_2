using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : ProjectileBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
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
