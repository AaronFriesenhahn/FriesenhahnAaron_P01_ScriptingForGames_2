using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    [SerializeField] public float _PowerUpDuration = 2;
    [SerializeField] ParticleSystem _collectParticles;
    [SerializeField] AudioClip _collectSound;
    [SerializeField] AudioClip _powerDownSound;
    protected abstract void PowerUp(Player player);
    protected abstract void PowerDown(Player player);
    [SerializeField] float _movementSpeed = -1;

    float _originalPowerUpDuration;
    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement(_rb);
    }

    protected virtual void Movement(Rigidbody rb)
    {
        //calculate rotation
        Quaternion turnOffset = Quaternion.Euler(0, _movementSpeed, 0);
        rb.MoveRotation(_rb.rotation * turnOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Feedback();
            PowerUp(player);
            //disable collider and visuals
            var powerupMesh = gameObject.GetComponent<MeshRenderer>();
            var powerupCollider = gameObject.GetComponent<Collider>();
            powerupMesh.enabled = false;
            powerupCollider.enabled = false;

            _originalPowerUpDuration = _PowerUpDuration;

            IEnumerator PowerUpCountdown()
            {
                while (_PowerUpDuration > 0)
                {
                    yield return new WaitForSeconds(1f);

                    _PowerUpDuration--;
                }

                if (_PowerUpDuration <= 0)
                {
                    if (_powerDownSound != null)
                    {
                        AudioHelper.PlayClip2D(_powerDownSound, 1f);
                    }
                    _PowerUpDuration = 0;
                    PowerDown(player);
                }
            }
            //call PowerDown funtion after PowerUp duration is over and disable game object
            StartCoroutine(PowerUpCountdown());
        }
        _PowerUpDuration = _originalPowerUpDuration;
    }

    private void Feedback()
    {
        //particles
        if (_collectParticles != null)
        {
            _collectParticles = Instantiate(_collectParticles,
                transform.position, Quaternion.identity);
        }
        //audio
        if (_collectSound != null)
        {
            AudioHelper.PlayClip2D(_collectSound, 1f);
        }
        
    }

}
