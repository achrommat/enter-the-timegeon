using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    [SerializeField] private GameObject _explosion;    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isDeflected)
        {
            Stats stats;
            if ((stats = collision.GetComponent(typeof(Stats)) as Stats) != null)
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                if (player.State == PlayerState.DASH || player.State == PlayerState.REWIND)
                {
                    return;
                }

                if (stats.IsAlive())
                {
                    stats.Damage(Damage);
                    //GameManager.Instance.Player.Animator.SetTrigger("Hurt");
                    MF_AutoPool.Despawn(gameObject);
                }
            }
        }
        if (collision.CompareTag("Wall"))
        {
            //GameObject exp = MF_AutoPool.Spawn(_explosion, transform.position, Quaternion.identity);
            //exp.GetComponent<Explosion>().OnSpawned();
            MF_AutoPool.Despawn(gameObject);
        }

        if (collision.CompareTag("Enemy") && _isDeflected)
        {
            Stats stats;
            if ((stats = collision.GetComponent(typeof(Stats)) as Stats) != null)
            {
                if (stats.IsAlive())
                {
                    stats.Damage(Damage);
                    MF_AutoPool.Despawn(gameObject);
                }
            }
        }
    }

    public void Deflect(Vector2 direction)
    {
        _isDeflected = true;
        ChronosTime.rigidbody2D.AddForce(direction * Speed * 2, ForceMode2D.Impulse);
    }
}
