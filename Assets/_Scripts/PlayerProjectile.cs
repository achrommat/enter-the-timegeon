using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] private GameObject _explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Stats stats;
        if ((stats = collision.gameObject.GetComponent(typeof(Stats)) as Stats) != null)
        {
            if (stats.IsAlive())
            {
                stats.Damage(Damage);
                //GameObject exp = MF_AutoPool.Spawn(_explosion, transform.position, Quaternion.identity);
                //exp.GetComponent<Explosion>().OnSpawned();
                MF_AutoPool.Despawn(gameObject);
            }
        }
        if (collision.CompareTag("Wall"))
        {
            //GameObject exp = MF_AutoPool.Spawn(_explosion, transform.position, Quaternion.identity);
            //exp.GetComponent<Explosion>().OnSpawned();
            MF_AutoPool.Despawn(gameObject);
        }
    }
}
