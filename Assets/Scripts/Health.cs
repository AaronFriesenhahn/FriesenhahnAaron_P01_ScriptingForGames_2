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
        
    }

    public void TakeDamage(int damage)
    {
        if (invincible == false)
        {
            _currentHealth -= damage;
            //HitFeedback();
            Debug.Log(ObjectName + "'s Health: " + _currentHealth);
            HitFlash();


            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Kill();
            }
        }
    }

    private void Kill()
    {
        KillObject = true;
        var objectMesh = gameObject.GetComponent<MeshRenderer>();
        var objectCollider = gameObject.GetComponent<Collider>();

        objectMesh.enabled = false;
        objectCollider.enabled = false;
    }

    private void HitFlash()
    {
        Debug.Log("Hit. Applying Visual Feedback.");
        _hitParticles = Instantiate(_hitParticles, transform.position, Quaternion.identity);
    }
}
