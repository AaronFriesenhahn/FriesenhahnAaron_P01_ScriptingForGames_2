using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : Enemy
{
    float speed = 1f;

    protected override void PlayerImpact(Player player)
    {
        //base.PlayerImpact(player);
        player.Kill();
    }

    protected override void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _PlayerTransform.position, step);
    }

    protected override void DeathFeedback()
    {
        //none, the Killer is invincible
    }
}
