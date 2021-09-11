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

    TankController _tankController;
    Health _healthSystem;

    private void Awake()
    {
        _tankController = GetComponent<TankController>();
        _healthSystem = GetComponent<Health>();
    }

    private void Start()
    {
        SetHealthCountText();
        SetTreasureCountText();
    }

    private void Update()
    {
        SetHealthCountText();
        if (_healthSystem.KillObject == true)
        {
            DeathFeedback();
        }
        _healthSystem.invincible = invincible;
    }

    public void IncreaseHealth(int amount)
    {
        _healthSystem._currentHealth = _healthSystem._currentHealth + amount;
        _healthSystem._currentHealth = Mathf.Clamp(_healthSystem._currentHealth, 0, _healthSystem._maxHealth);
        Debug.Log("Player's health: " + _healthSystem._currentHealth);
        SetHealthCountText();
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
    }

    public void HitFeedback()
    {
        //play noise
        AudioHelper.PlayClip2D(_hitSound, 1f);
        StartCoroutine(HitFlash());
    }

    IEnumerator PauseForTime()
    {
        yield return new WaitForSeconds(2f);
    }

    IEnumerator HitFlash()
    {
        GameObject TankBody = GameObject.Find("Tank/Art/Body");
        var TankBodyRenderer = TankBody.GetComponent<Renderer>();
        //flash colors?
        Debug.Log("Hit. Applying Visual Feedback.");
        TankBodyRenderer.material.SetColor("_Color", Color.white);
        yield return new WaitForSeconds(0.1f);
        TankBodyRenderer.material.SetColor("_Color", Color.red);
        Debug.Log("Red.");
        yield return new WaitForSeconds(0.1f);
        TankBodyRenderer.material.SetColor("_Color", Color.white);
        Debug.Log("White.");
        yield return new WaitForSeconds(0.1f);
        TankBodyRenderer.material = TankBodyMaterial;
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
        gameObject.SetActive(false);
    }
}

