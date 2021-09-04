using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] int _damageAmount = 1;
    public ParticleSystem _impactParticles;
    [SerializeField] AudioClip _impactSound;
    public Transform _PlayerTransform;

    public AudioClip _deathSound;

    Rigidbody _rb;
    public int _MaxHealth = 1;
    public int _currentHealth;

    float speed = 2f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _currentHealth = _MaxHealth;
    }

    private void OnCollisionEnter(Collision other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            PlayerImpact(player);
            ImpactFeedback();
        }
        else if (other.collider.tag == "Projectile")
        {
            Health(_damageAmount);
            Debug.Log("Decrease enemy health.");
        }

    }

    protected virtual void PlayerImpact(Player player)
    {
        player.DecreaseHealth(_damageAmount);
    }

    private void ImpactFeedback()
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

    public void Health(int amount)
    {
        _currentHealth -= amount;
        Debug.Log("Enemy's health: " + _currentHealth);
        if (_currentHealth <= 0)
        {
            DeathFeedback();
        }
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
