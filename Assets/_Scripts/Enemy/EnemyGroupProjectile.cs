using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupProjectile : Projectile
{
    public void OnSpawned(Vector2 centerPos, Vector2 direction, Weapon weapon)
    {
        Lifetime = weapon.ProjectileLifetime;
        Vector2 center = centerPos;
        for (var pointNum = 0; pointNum < weapon.ProjectileCount; pointNum++)
        {
            float i = (float)(pointNum * 1.0 / weapon.ProjectileCount);
            float angle = i * Mathf.PI * 2;
            float x = Mathf.Sin(angle) * 1;
            float y = Mathf.Cos(angle) * 1;
            Vector2 pos = new Vector2(x, y) + center;

            GameObject bullet = MF_AutoPool.Spawn(weapon.Projectile, pos, Quaternion.identity);
            bullet.GetComponent<Projectile>().Initialize(direction, weapon);
        }
    }
}
