using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = .25f;
    [SerializeField] float _turnSpeed = 2f;

    [SerializeField] GameObject _Projectile;
    [SerializeField] ParticleSystem _MuzzleFlare;
    [SerializeField] AudioClip _FireProjectile;
    [SerializeField] Transform _EmitLocation;

    Rigidbody _rb = null;
    private bool allowfire = true;

    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();
    }

    public void MoveTank()
    {
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * MoveSpeed;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
    }

    public void TurnTank()
    {
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * _turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
    }

    public void Update()
    {
        FireProjectile();
    }

    private void FireProjectile()
    {
        //fire projectile
        if (Input.GetKeyDown(KeyCode.Space) && allowfire == true)
        {
            allowfire = false;
            _MuzzleFlare.Play();
            if (_FireProjectile != null)
            {
                AudioHelper.PlayClip2D(_FireProjectile, 1f);
            }
            GameObject projectile = Instantiate(_Projectile, _EmitLocation.position, _EmitLocation.rotation);
            
            StartCoroutine(FireCooldown());
        }
    }

    IEnumerator FireCooldown()
    {
        Debug.Log("Cooldown activated.");
        yield return new WaitForSeconds(1f);
        allowfire = true;
    }
}
