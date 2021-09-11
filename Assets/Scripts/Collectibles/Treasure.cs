using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : CollectibleBase
{
    [SerializeField] int _treasureAmount = 1;

    protected override void Collect(Player player)
    {
        //increase Treasure Count for Player
        player.TreasureCount(_treasureAmount);
    }

    protected override void Movement(Rigidbody rb)
    {
        //calculate rotation
        Quaternion turnOffset = Quaternion.Euler
            (MovementSpeed, MovementSpeed, 0);
        rb.MoveRotation(rb.rotation * turnOffset);
    }
}
