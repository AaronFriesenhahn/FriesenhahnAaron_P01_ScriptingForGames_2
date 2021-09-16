using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    public int damage = 1;
    public ParticleSystem _impactParticles;
    [SerializeField] AudioClip _impactSound;
    public Transform _PlayerTransform;

    public AudioClip _deathSound;

    Health _healthSystem;

    Rigidbody _rb;

    float speed = 2.5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _healthSystem = GetComponent<Health>();
    }

    public void Update()
    {
        if (_healthSystem.KillObject == true)
        {
            DeathFeedback();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            IDamageable hit = (IDamageable)collision.gameObject.GetComponent(typeof(IDamageable));
            if (hit != null)
            {
                hit.TakeDamage(damage);
            }
            ImpactFeedback();
        }
        else if (collision.collider.tag == "Projectile")
        {
            DeathFeedback();
        }
    }

    public void ImpactFeedback()
    {
        //particles
        if (_impactParticles != null)
        {
            _impactParticles = Instantiate(_impactParticles, 
                transform.position, Quaternion.identity);
        }
        //audio. TODO - consider Object Pooling for performance
        if (_impactSound != null)
        {
            AudioHelper.PlayClip2D(_impactSound, 1f);
        }
    }

    private void FixedUpdate()
    {
        GameObject PlayerFound = GameObject.FindGameObjectWithTag("Player");
        if (PlayerFound != null)
        {
            Move();
        } 
    }

    protected virtual void Move()
    {
        var targetPosition = new Vector3 (_PlayerTransform.position.x, transform.position.y, _PlayerTransform.position.z);
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    protected virtual void DeathFeedback()
    {
        if (_impactParticles != null)
        {
            _impactParticles = Instantiate(_impactParticles,
                transform.position, Quaternion.identity);
        }
        if (_deathSound != null)
        {
            AudioHelper.PlayClip2D(_deathSound, 1f);
        }
        gameObject.SetActive(false);
    }


}
