using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float thrust = 500f;
    public Rigidbody rb;
    float timeLeft = 2f;

    public Vector3 target;

    int weaponDamage = 10;

    [SerializeField] ParticleSystem impactParticles;

    int addforce = 0;

    private float originY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        DestroyObject();
    }

    //projectile destroys upon collision
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit an object.");
        Destroy(gameObject);
    }

    void DestroyObject()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 GetPosition(Vector2 pos)
    {
        Vector3 tempPos = transform.position;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            tempPos = hit.point;
        }
        return new Vector3(tempPos.x, originY, tempPos.z);
    }
}
