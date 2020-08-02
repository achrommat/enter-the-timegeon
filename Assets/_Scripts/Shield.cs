using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ChronosMonoBehaviour
{
    [SerializeField] private float _activeTime = 3f;
    [SerializeField] private List<EnemyProjectile> _enemyBullets;

    public void OnSpawned()
    {
        _enemyBullets = new List<EnemyProjectile>();
        ChronosTime.Plan(_activeTime, delegate { Despawn(); });
    }

    private void Despawn()
    {
        foreach (EnemyProjectile projectile in _enemyBullets)
        {
            Vector2 direction = projectile.transform.position - transform.position;
            projectile.Deflect(direction);
        }

        MF_AutoPool.Despawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            EnemyProjectile projectile = collision.GetComponent<EnemyProjectile>();
            projectile.ChronosTime.rigidbody2D.velocity = new Vector2();
            projectile.IsFrozen = true;
            _enemyBullets.Add(projectile);
        }
    }
}
