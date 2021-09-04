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

    float speed = 3f;
    public int _damageAmount = 2;

    int minionsSpawned = 0;
    int movingStarted = 0;
    int rammedPlayer = 0;
    bool reachDestination1 = false;
    bool reachDestination2 = false;

    private bool allowfire = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
        if (_currentHealth >2)
        {
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
        // if health is 2, move towards player and ram them
        else if (_currentHealth == 2)
        {
            if (movingStarted == 0)
            {
                Debug.Log("Boss is moving.");
                movingStarted = 1;
            }
            if (rammedPlayer == 0)
            {
                var targetPosition = new Vector3(_PlayerTransform.position.x, transform.position.y, _PlayerTransform.position.z);
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            }
        }
        //if health is 1, spawn minions
        else if (_currentHealth == 1 && allowfire == true)
        {
            FireAtPlayer();
            if (minionsSpawned == 0)
            {
                Debug.Log("Spawn minion!");
                _minionToSpawn = Instantiate(_minionToSpawn, transform.position + (transform.forward * 2), transform.rotation);
                _minionToSpawn = Instantiate(_minionToSpawn, transform.position + (transform.right * 2), transform.rotation);
                _minionToSpawn = Instantiate(_minionToSpawn, transform.position + (transform.forward * -2), transform.rotation);
                minionsSpawned = 1;
            }
        }
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

    protected override void PlayerImpact(Player player)
    {
        player.DecreaseHealth(_damageAmount);
        rammedPlayer = 1;
        if (rammedPlayer == 1)
        {
            Debug.Log("Pulling boss backwards.");
            StartCoroutine(PullBossBackwards());
        }
    }

    IEnumerator PullBossBackwards()
    {
        yield return new WaitForSeconds(4f);
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
            projectile.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
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
}
