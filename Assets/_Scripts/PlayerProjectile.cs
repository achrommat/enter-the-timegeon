using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] private GameObject _hitFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Stats stats;
        if ((stats = collision.gameObject.GetComponent(typeof(Stats)) as Stats) != null)
        {
            if (stats.IsAlive())
            {
                Vector3 collisionPoint = collision.bounds.ClosestPoint(transform.position);

                GameObject hit = MF_AutoPool.Spawn(_hitFX, collisionPoint, Quaternion.identity);
                hit.GetComponent<FX>().OnSpawned();
                MF_AutoPool.Despawn(gameObject);
                stats.Damage(Damage);
            }
        }
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("BossShield"))
        {
            Vector3 collisionPoint = collision.bounds.ClosestPoint(transform.position);

            GameObject hit = MF_AutoPool.Spawn(_hitFX, collisionPoint, Quaternion.identity);
            hit.GetComponent<FX>().OnSpawned();
            MF_AutoPool.Despawn(gameObject);
        }
    }
}
