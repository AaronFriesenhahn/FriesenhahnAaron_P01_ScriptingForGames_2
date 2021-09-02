using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncrease : CollectibleBase
{
    [SerializeField] float _speedAmount = 5;
    [SerializeField] float _PowerUpDuration = 2f;
    [SerializeField] AudioClip _powerDownSound;

    protected override void Collect(Player player)
    {
        //pull motor controller from the player
        TankController controller = player.GetComponent<TankController>();
        float _originalMoveSpeed = controller.MoveSpeed;
        float _originalPowerUpDuration = _PowerUpDuration;
        if (controller != null)
        {
            controller.MoveSpeed += _speedAmount;

            //call PowerDown funtion after PowerUp duration is over and disable game object
            StartCoroutine(PowerUpCountdown());

            //timer that restores the player to their normal speed
            IEnumerator PowerUpCountdown()
            {
                while (_PowerUpDuration > 0)
                {
                    Debug.Log("Powerup Countdown Starting.");
                    yield return new WaitForSeconds(_PowerUpDuration);

                    _PowerUpDuration--;
                }

                if (_PowerUpDuration <= 0)
                {
                    if (_powerDownSound != null)
                    {
                        AudioHelper.PlayClip2D(_powerDownSound, 1f);
                    }
                    Debug.Log("Powerup Countdown has ended. Return time and speed to normal.");
                    controller.MoveSpeed = _originalMoveSpeed;
                    _PowerUpDuration = _originalPowerUpDuration;
                }
            }
        }
    }

    //protected override void Movement(Rigidbody rb)
    //{
    //    //calculate rotation
    //    Quaternion turnOffset = Quaternion.Euler
    //        (MovementSpeed, MovementSpeed, MovementSpeed);
    //    rb.MoveRotation(rb.rotation * turnOffset);
    //}
}
