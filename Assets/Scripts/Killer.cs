using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : Enemy
{
    float speed = 2.5f;

    public void Awake()
    {
        GameObject PlayerFound = GameObject.FindGameObjectWithTag("Player");
        _PlayerTransform = PlayerFound.transform;
    }

    protected override void Move()
    {
        var targetPosition = new Vector3(_PlayerTransform.position.x, transform.position.y, _PlayerTransform.position.z);
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

}
