using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TankController))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    [SerializeField] int _treasureCount = 0;
    int _currentTreasure;
    [SerializeField] AudioClip _hitSound;
    [SerializeField] ParticleSystem _deathParticles;
    [SerializeField] AudioClip _deathSound;

    [SerializeField] Material TankBodyMaterial;

    public bool invincible = false;
    public bool SpeedActivated = false;

    public Text _TreasureCountText;
    public Text _HealthCountText;
    public GameObject DamageImage1;
    public GameObject DamageImage2;
    public GameObject _camera;

    TankController _tankController;
    Health _healthSystem;

    private void Awake()
    {
        _tankController = GetComponent<TankController>();
        _healthSystem = GetComponent<Health>();
    }

    private void Start()
    {
        //SetHealthCountText();
        SetTreasureCountText();
    }

    private void Update()
    {
        //SetHealthCountText();
        _healthSystem.invincible = invincible;
    }

    public void IncreaseHealth(int amount)
    {
        _healthSystem.IncreaseHealth(amount);
    }

    public void TreasureCount(int amount)
    {
        _currentTreasure = _currentTreasure + amount;
        Debug.Log("Player's treasure: " + _currentTreasure);
        SetTreasureCountText();
    }

    void SetTreasureCountText()
    {
        _TreasureCountText.text = "Treasure: " + _currentTreasure;
    }

    void SetHealthCountText()
    {
        _HealthCountText.text = "Health: " + _healthSystem._currentHealth;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "EnemyProjectile")
        {
            HitFeedback();
        }
        else if (_healthSystem._currentHealth == 0)
        {
            DeathFeedback();
        }
        else if (collision.collider.tag == "Enemy")
        {
            HitFeedback();
        }
    }

    public void HitFeedback()
    {
        //play noise
        AudioHelper.PlayClip2D(_hitSound, 1f);
        //camera shake
        StartCoroutine(ShakeCamera());
        //flashing screen
        StartCoroutine(HitFlash());
    }

    IEnumerator PauseForTime()
    {
        yield return new WaitForSeconds(2f);
    }

    IEnumerator HitFlash()
    {
        DamageImage1.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        DamageImage1.SetActive(false);
        DamageImage2.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        DamageImage2.SetActive(false);
    }

    IEnumerator ShakeCamera()
    {
        _camera.transform.Rotate(0, 0, 15);
        yield return new WaitForSeconds(0.1f);
        _camera.transform.Rotate(0, 0, -15);
        yield return new WaitForSeconds(0.1f);
        _camera.transform.Rotate(0, 0, 15);
        yield return new WaitForSeconds(0.1f);
        _camera.transform.Rotate(0, 0, -15);
        yield return new WaitForSeconds(0.1f);
        _camera.transform.Rotate(0, 0, 0);
    }

    private void DeathFeedback()
    {
        //particles
        if (_deathParticles != null)
        {
            _deathParticles = Instantiate(_deathParticles,
                transform.position, Quaternion.identity);
        }
        //audio
        if (_deathSound != null)
        {
            AudioHelper.PlayClip2D(_deathSound, 1f);
        }
    }
}

