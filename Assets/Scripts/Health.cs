using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public int _maxHealth = 3;
    public int _currentHealth;
    public bool KillObject = false;
    string ObjectName;

    public event Action<int> Damaged = delegate { };
    public event Action<int> Healed = delegate { };
    public event Action HealthAt50Percent = delegate { };
    //public event Action Killed = delegate { };

    [SerializeField] ParticleSystem _hitParticles;

    public bool invincible = false;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
        ObjectName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentHealth <= (_maxHealth / 2))
        {
            At50PercentHealth();
        }
    }

    public void TakeDamage(int damage)
    {
        if (invincible == false)
        {
            _currentHealth -= damage;
            Damaged.Invoke(damage);
            HitFlash();
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Kill();
            }
        }
    }

    public void IncreaseHealth(int value)
    {
        _currentHealth += value;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        Healed.Invoke(value);
    }

    public void At50PercentHealth()
    {
        HealthAt50Percent.Invoke();
    }

    private void Kill()
    {
        gameObject.SetActive(false);
    }

    private void HitFlash()
    {
        _hitParticles = Instantiate(_hitParticles, transform.position, Quaternion.identity);
    }
}
