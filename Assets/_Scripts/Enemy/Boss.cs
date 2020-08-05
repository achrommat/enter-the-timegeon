using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyController
{
    [Header("Boss")]
    [SerializeField] private int _stage = 0;
    [SerializeField] private float _healthToChangeStage = 5;
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private int _shotCountToSwitch = 3;
    [SerializeField] private BossShield _shield;

    protected override void Update()
    {
        if (!Stats.IsAlive())
        {
            DeathHandler();
            return;
        }

        ChangeStage();
        Shoot();
    }

    private void ChangeStage()
    {
        if (_stage == 0 && Stats.CurrentHealth <= _healthToChangeStage)
        {
            _stage++;
            _shield.Activate();
            Weapon = _weapons[3];
        }        
    }

    protected override void Shoot()
    {
        SwitchWeapon();
        switch (Weapon.Type)
        {
            case Weapon.TypeEnum.SinCos:
                Shootable.SinCosShoot();
                break;
            default:
                Shootable.GroupShoot();
                break;
        }
    }

    private void SwitchWeapon()
    {
        if (ShotCount == _shotCountToSwitch)
        {
            if (_stage == 0)
            {
                if (Weapon == _weapons[0])
                {
                    Weapon = _weapons[1];
                }
                else if (Weapon == _weapons[1])
                {
                    Weapon = _weapons[0];
                }
            }
            else
            {
                if (Weapon == _weapons[2])
                {
                    Weapon = _weapons[3];
                }
                else if (Weapon == _weapons[3])
                {
                    Weapon = _weapons[2];
                }
            }            
            ShotCount = 0;
        }        
    }
}
