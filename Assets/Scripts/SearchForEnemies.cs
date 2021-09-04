using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForEnemies : MonoBehaviour
{
    [SerializeField] GameObject _searchingObject;

    GameObject _enemyFound;
    bool _foundEnemy = false;

    //detects an enemy and returns enemy's position
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            _foundEnemy = true;
            _enemyFound = collision.collider.gameObject;
        }
        else
        {
            _foundEnemy = false;
        }
    }

    //the speed to move at
    public float speed = 50f;
    void Update()
    {
        GameObject[] EnemyArray;
        EnemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in EnemyArray)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        //if the array is not empty check for enemy
        if (EnemyArray.Length != 0)
        {
            //Check if the enemy is within 5 meters?
            if (Vector3.Distance(transform.position, closest.transform.position) < 5f)
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                //reset momentum
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                //add some momentum so it doesn't turn so drastically
                rb.AddForce(transform.forward * 100);
                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, closest.transform.position, step);
            }
            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, closest.transform.position) < 0.001f)
            {
                Debug.Log("destroy object.");
                //Destroy(gameObject);
            }
        }
    }
}
