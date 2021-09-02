using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TankController))]
public class Player : MonoBehaviour
{
    [SerializeField] int _maxHealth = 3;
    int _currentHealth;
    [SerializeField] int _treasureCount = 0;
    int _currentTreasure;
    [SerializeField] ParticleSystem _deathParticles;
    [SerializeField] AudioClip _deathSound;

    public bool invincible = false;

    public Text _TreasureCountText;
    public Text _HealthCountText;

    TankController _tankController;

    private void Awake()
    {
        _tankController = GetComponent<TankController>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        SetHealthCountText();
        SetTreasureCountText();
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth = _currentHealth + amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        Debug.Log("Player's health: " + _currentHealth);
        SetHealthCountText();
    }

    public void DecreaseHealth(int amount)
    {
        if (invincible == false)
        {
            _currentHealth -= amount;
            Debug.Log("Player's health: " + _currentHealth);
            if (_currentHealth <= 0)
            {
                Kill();
            }
        }
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
        _HealthCountText.text = "Health: " + _currentHealth;
    }

    public void Kill()
    {
        if (invincible == false)
        {
            gameObject.SetActive(false);
            //play particles and sound
            Feedback();
        }
    }

    private void Feedback()
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

