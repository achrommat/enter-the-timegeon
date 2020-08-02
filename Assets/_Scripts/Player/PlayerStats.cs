using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    [SerializeField] private PlayerController _player;

    public override void Damage(float amount)
    {
        if (!IsAlive() || _player.State == PlayerState.DASHING)
        {
            return;
        }

        CurrentHealth -= amount;

        if (DamageFeedback != null)
        {
            DamageFeedback.PlayFeedbacks();
        }
    }
}
