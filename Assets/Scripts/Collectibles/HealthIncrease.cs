using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncrease : CollectibleBase
{
    [SerializeField] int _healthAdded = 1;
    Health _healthSystem;

    protected override void Collect(Player player)
    {
        player.IncreaseHealth(_healthAdded);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Collect(player);
            //get mesh of Collectable
            var powerupMesh = gameObject.GetComponent<MeshRenderer>();
            var powerupCollider = gameObject.GetComponent<Collider>();
            //spawn particles & sfx because we need to disable object
            Feedback();

            //gameObject.SetActive(false);
            powerupMesh.enabled = false;
            powerupCollider.enabled = false;
        }
    }
}
