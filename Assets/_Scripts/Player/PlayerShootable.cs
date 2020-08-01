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
        Quaternion rotation = Quaternion.Euler(0, 0, shootPosition.eulerAngles.z + Random.Range(-_player.Weapon.Recoil, _player.Weapon.Recoil));
        GameObject projectileObj = MF_AutoPool.Spawn(_player.Weapon.Projectile, shootPosition.position, rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        Vector3 velocity = projectileObj.transform.rotation * Vector3.right;
        //GameObject bullet = MF_AutoPool.Spawn(_player.Weapon.Projectile, shootPosition.position, shootPosition.rotation);
        projectile.Initialize(velocity, _player.Weapon);
    }
}
