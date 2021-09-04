﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float thrust = 500f;
    public Rigidbody rb;
    float timeLeft = 2f;

    public Vector3 target;

    public int weaponDamage = 1;

    public ParticleSystem impactParticles;
    public AudioClip impactSound;

    int addforce = 0;

    private float originY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        DestroyObject();
    }

    //projectile destroys upon collision
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
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
        if (collision.collider.tag != "Player")
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

    //destroys object after a certain amount of time
    void DestroyObject()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyOnImpact()
    {
        Debug.Log("Enumerator called.");
        yield return new WaitForSeconds(.25f);
        Debug.Log("Destroyed object.");
        Destroy(gameObject);
    }

    private Vector3 GetPosition(Vector2 pos)
    {
        Vector3 tempPos = transform.position;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            tempPos = hit.point;
        }
        return new Vector3(tempPos.x, originY, tempPos.z);
    }
}
