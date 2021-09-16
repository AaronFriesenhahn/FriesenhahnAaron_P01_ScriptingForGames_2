using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : Enemy
{
    [SerializeField] float _speedAmount = 1;
    [SerializeField] public float _PowerUpDuration = 2;
    [SerializeField] AudioClip _powerUpSound;
    [SerializeField] AudioClip _powerDownSound;

    float _originalMoveSpeed;
    float _originalPowerUpDuration;

    int hitPlayer = 0;

    private Health _healthSystem;

    float speed = 4f;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        TankController controller = player.GetComponent<TankController>();
        _originalMoveSpeed = controller.MoveSpeed;

    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            IDamageable hit = (IDamageable)collision.gameObject.GetComponent(typeof(IDamageable));
            if (hit != null)
            {
                //hit.TakeDamage(damage);
            }
            PlayerImpact(player);
        }
        else if(collision.collider.tag == "Projectile")
        {
            DeathFeedback();
        }
    }

    private void PlayerImpact(Player player)
    {
        //pull motor controller from the player
        TankController controller = player.GetComponent<TankController>();
        if (hitPlayer == 0)
        {
            _originalMoveSpeed = controller.MoveSpeed;
        }
        
        hitPlayer = 1;
        
        if (controller != null)
        {
            if (controller.MoveSpeed > 0)
            {
                controller.MoveSpeed -= _speedAmount;
            }
            if (controller.MoveSpeed < 0)
            {
                controller.MoveSpeed = 0.02f;
            }

            AudioHelper.PlayClip2D(_powerDownSound, 1f);

            //call PowerDown funtion after PowerUp duration is over and disable game object
            StartCoroutine(PowerUpCountdown());

            //timer that restores the player to their normal speed
            IEnumerator PowerUpCountdown()
            {
                Debug.Log("Slower Powerup Countdown Starting.");
                yield return new WaitForSeconds(_PowerUpDuration);
                if (_powerUpSound != null)
                {
                    AudioHelper.PlayClip2D(_powerUpSound, 1f);
                }
                Debug.Log("Slower Powerup Countdown has ended.");
                controller.MoveSpeed = _originalMoveSpeed;
                hitPlayer = 0;
            }
        }
    }

    protected override void Move()
    {
        if (hitPlayer == 0)
        {
            var targetPosition = new Vector3(_PlayerTransform.position.x, transform.position.y, _PlayerTransform.position.z);
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        } 
    }

    protected override void DeathFeedback()
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
        //upon death return player to normal speed
        if (GameObject.FindGameObjectsWithTag("Player") != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            TankController controller = player.GetComponent<TankController>();
            Debug.Log("Upon death, return player speed to normal.");
            controller.MoveSpeed = _originalMoveSpeed;
            AudioHelper.PlayClip2D(_powerUpSound, 1f);
        }
    }
}
