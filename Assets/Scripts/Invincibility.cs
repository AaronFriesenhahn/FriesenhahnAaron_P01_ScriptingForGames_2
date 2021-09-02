using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : PowerUpBase
{
    [SerializeField] GameObject _playerObject;
    [SerializeField] Material _playerMaterial;
    [SerializeField] Material _InvincibleMaterial;

    protected override void PowerUp(Player player)
    {
        //make player invincible
        player.invincible = true;
        //change player color to Cyan
        _playerObject.GetComponent<MeshRenderer>().material = _InvincibleMaterial;
    }

    protected override void PowerDown(Player player)
    {
        //return player to normal
        player.invincible = false;
        //change player color to normal
        _playerObject.GetComponent<MeshRenderer>().material = _playerMaterial;
        gameObject.SetActive(false);
    }
}
