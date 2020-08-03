using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    [SerializeField] private PlayerController _player;

    public override void Damage(float amount)
    {
        if (_player.State == PlayerState.DASH || _player.State == PlayerState.REWIND)
        {
            return;
        }
        base.Damage(amount);
    }
}
