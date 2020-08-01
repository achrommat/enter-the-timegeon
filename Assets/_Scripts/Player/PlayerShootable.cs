using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootable : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    public Transform ShootPosition;
    private float _nextAttackTime;

    public void Shoot()
    {
        if (Time.time >= _nextAttackTime)
        {
            CreateBullet(ShootPosition);
            _nextAttackTime = Time.time + _player.Weapon.AttackDelay;
        }
    }

    private void CreateBullet(Transform shootPosition)
    {
        GameObject bullet = MF_AutoPool.Spawn(_player.Weapon.Projectile, shootPosition.position, shootPosition.rotation);
        bullet.GetComponent<Projectile>().Speed = _player.Weapon.ProjectileSpeed;
        bullet.GetComponent<Projectile>().Lifetime = _player.Weapon.ProjectileLifetime;
        bullet.GetComponent<Projectile>().Damage = _player.Weapon.Damage;
        bullet.GetComponent<Projectile>().Initialize(shootPosition.right);
    }
}
