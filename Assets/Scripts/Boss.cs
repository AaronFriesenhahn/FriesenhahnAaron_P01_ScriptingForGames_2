using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] GameObject EnemyProjectile;
    [SerializeField] Transform EmitLocation;
    [SerializeField] AudioClip _enemyFire;

    [SerializeField] ParticleSystem _deathParticles;
    [SerializeField] GameObject _minionToSpawn;

    [SerializeField] Transform _targetToFireAt;
    [SerializeField] GameObject _tankTopToLookAtPlayer;

    [SerializeField] Transform _BossDestination1;
    [SerializeField] Transform _BossDestination2;

    [SerializeField] AudioClip _hitSound;

    [SerializeField] ParticleSystem _chargeAtPlayerParticles;
    [SerializeField] TrailRenderer _ChargeTrail;

    int minionsSpawned = 0;
    int movingStarted = 0;
    int rammedPlayer = 0;
    bool reachDestination1 = false;
    bool reachDestination2 = false;

    private bool allowfire = true;

    Health _healthSystem;

    //testing particle activation
    bool _particleActivated = false;

    private void Awake()
    {
        _healthSystem = GetComponent<Health>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("Updating boss...");
        Move();
        Debug.Log(_healthSystem._currentHealth);
    }

    protected override void Move()
    {
        if (_targetToFireAt != null)
        {
            var targetPosition = _targetToFireAt.position;
            targetPosition.y = _tankTopToLookAtPlayer.transform.position.y;
            _tankTopToLookAtPlayer.transform.LookAt(targetPosition);
        }
        // if health is normal, perform a patrol and fire at player.
        if (_healthSystem._currentHealth == _healthSystem._maxHealth)
        {
            _ChargeTrail.enabled = false;
            FireAtPlayer();
            //move to Destination1
            float patrolSpeed = 2f;
            if (transform.position != _BossDestination1.position && reachDestination1 == false)
            {
                float step = patrolSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _BossDestination1.position, step);
            }
            //move to Destination2
            else if (transform.position != _BossDestination2.position && reachDestination2 == false)
            {
                float step = patrolSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _BossDestination2.position, step);
            }
            //if reached Destination1, swap to Destination2
            if (Vector3.Distance(transform.position, _BossDestination1.position) < 1f)
            {
                reachDestination1 = true;
                reachDestination2 = false;
                Debug.Log("reached destination 1.");
            }
            //if reached Destination2, swap to Destination1
            else if (Vector3.Distance(transform.position, _BossDestination2.position) < 1f)
            {
                reachDestination2 = true;
                reachDestination1 = false;
                Debug.Log("reached destination 2.");
            }
        }
        // if health is 4 or 3, move towards player and ram them
        else if (_healthSystem._currentHealth == 4 || _healthSystem._currentHealth == 3)
        {
            //activate trail effect?
            _ChargeTrail.enabled = true;
            StartCoroutine(PauseBeforeSpawning());
            if (_particleActivated == true)
            {
                if (movingStarted == 0)
                {
                    Debug.Log("Boss is moving.");
                    movingStarted = 1;
                }
                if (rammedPlayer == 0)
                {
                    float speed = 10f;
                    var targetPosition = new Vector3(_PlayerTransform.position.x, transform.position.y, _PlayerTransform.position.z);
                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                }
            }
        }
        //if health is 2 or 1, spawn minions
        else if (_healthSystem._currentHealth == 2 && allowfire == true || _healthSystem._currentHealth == 1 && allowfire == true )
        {
            _ChargeTrail.enabled = false;
            FireAtPlayer();
            if (minionsSpawned == 0)
            {
                //StartCoroutine(PauseBeforeSpawning());
                Debug.Log("Spawn minion!");
                _minionToSpawn = Instantiate(_minionToSpawn, transform.position + (transform.forward * 2), transform.rotation);
                _minionToSpawn = Instantiate(_minionToSpawn, transform.position + (transform.right * 2), transform.rotation);
                _minionToSpawn = Instantiate(_minionToSpawn, transform.position + (transform.forward * -2), transform.rotation);
                minionsSpawned = 1;
            }
        }
    }

    IEnumerator PauseBeforeSpawning()
    {
        yield return new WaitForSeconds(1f);
        if (_particleActivated == false)
        {
            _chargeAtPlayerParticles.Play();
        }
        yield return new WaitForSeconds(1f);
        _particleActivated = true;
    }

    protected override void DeathFeedback()
    {
        if (_deathParticles != null)
        {
            _deathParticles = Instantiate(_deathParticles,
                transform.position, Quaternion.identity);
        }
        if (_deathSound != null)
        {
            AudioHelper.PlayClip2D(_deathSound, 1f);
        }
        
        gameObject.SetActive(false);
    }

    protected override void OnCollisionEnter(Collision collision)
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
            PlayerImpact();
        }
        else if (collision.collider.tag == "Projectile")
        {
            HitFeedback();
        }
        else if (collision.collider.tag == "Projectile" && _healthSystem._currentHealth == 0)
        {
            DeathFeedback();
        }
    }

    private void PlayerImpact()
    {
        rammedPlayer = 1;
        if (rammedPlayer == 1)
        {
            Debug.Log("Pulling boss backwards.");
            StartCoroutine(PullBossBackwards());
        }
    }

    IEnumerator PullBossBackwards()
    {
        yield return new WaitForSeconds(3f);
        rammedPlayer = 0;
    }

    IEnumerator BossMoveBetween2Points()
    {
        
        yield return new WaitForSeconds(1f);
    }

    public void FireAtPlayer()
    {
        if (allowfire == true)
        {
            Debug.Log("Fire at Player!");
            //projectile ready to be fired so it is red
            AudioHelper.PlayClip2D(_enemyFire, 1f);
            //instantiate EnemyProjectile towards player
            GameObject projectile = Instantiate(EnemyProjectile, EmitLocation.position, EmitLocation.rotation);

            allowfire = false;
            StartCoroutine(PauseBetweenShots());
        }
    }

    IEnumerator PauseBetweenShots()
    {
        Debug.Log("Pause!");
        yield return new WaitForSeconds(2f);
        allowfire = true;
    }

    public void HitFeedback()
    {
        //play noise
        AudioHelper.PlayClip2D(_hitSound, 1f);
        //flash colors
        StartCoroutine(BossHitFlash());
        
    }

    IEnumerator BossHitFlash()
    {
        GameObject _BossBody;
        _BossBody = GameObject.Find("BossTank/Art/Body");
        Debug.Log("Boss has been hit. Coroutine started.");
        _BossBody.GetComponent<Renderer>().material.color = Color.white;
        yield return new WaitForSeconds(.2f);
        _BossBody.GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(.2f);
        _BossBody.GetComponent<Renderer>().material.color = Color.white;
        yield return new WaitForSeconds(.2f);
        _BossBody.GetComponent<Renderer>().material.color = Color.red;
    }
}
