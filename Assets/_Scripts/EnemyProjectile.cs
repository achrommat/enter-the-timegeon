using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    [SerializeField] private GameObject _explosion;
    public Vector2 DeflectVelocity;
    [SerializeField] private GameObject _hitFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isDeflected)
        {
            Stats stats;
            if ((stats = collision.GetComponent(typeof(Stats)) as Stats) != null)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player.State == PlayerState.DASH || player.State == PlayerState.REWIND || player.State == PlayerState.SHIELD || player.Stats.IsDamaged)
                {
                    return;
                }

                if (stats.IsAlive())
                {
                    Vector3 collisionPoint = collision.bounds.ClosestPoint(transform.position);
                    GameObject hit = MF_AutoPool.Spawn(_hitFX, collisionPoint, Quaternion.identity);
                    hit.GetComponent<FX>().OnSpawned();
                    
                    player.Knockback(ChronosTime.rigidbody2D.velocity, 1000f);
                    stats.Damage(Damage);
                    MF_AutoPool.Despawn(gameObject);
                }
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 collisionPoint = collision.bounds.ClosestPoint(transform.position);
            GameObject hit = MF_AutoPool.Spawn(_hitFX, collisionPoint, Quaternion.identity);
            hit.GetComponent<FX>().OnSpawned();
            MF_AutoPool.Despawn(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy") && _isDeflected)
        {
            Stats stats;
            if ((stats = collision.gameObject.GetComponent(typeof(Stats)) as Stats) != null)
            {
                if (stats.IsAlive())
                {
                    Vector3 collisionPoint = collision.bounds.ClosestPoint(transform.position);
                    GameObject hit = MF_AutoPool.Spawn(_hitFX, collisionPoint, Quaternion.identity);
                    hit.GetComponent<FX>().OnSpawned();
                    stats.Damage(Damage);
                    MF_AutoPool.Despawn(gameObject);
                }
            }
        }
        /*if (collision.gameObject.CompareTag("ExplosiveBarrel"))
        {
            Stats stats;
            if ((stats = collision.gameObject.GetComponent(typeof(Stats)) as Stats) != null)
            {
                if (stats.IsAlive())
                {
                    collision.GetContacts(contacts);

                    Vector3 normal = contacts[0].normal;
                    Vector2 point = contacts[0].point;

                    GameObject hit = MF_AutoPool.Spawn(_hitFX, point, Quaternion.identity);
                    hit.GetComponent<ProjectileHitFX>().OnSpawned();
                    stats.Damage(Damage);
                    MF_AutoPool.Despawn(gameObject);
                }
            }
        }*/
    }

    public void Deflect()
    {
        _isDeflected = true;
        ChronosTime.rigidbody2D.velocity = DeflectVelocity * 2;
        //ChronosTime.rigidbody2D.AddForce(direction * Speed * 2, ForceMode2D.Impulse);
    }
}
