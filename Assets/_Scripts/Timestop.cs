using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timestop : ChronosMonoBehaviour
{
    [SerializeField] private float _activeTime = 3f;
    [SerializeField] private List<EnemyController> _enemies;
    [SerializeField] private List<PlayerProjectile> _playerBullets;

    public void OnSpawned()
    {
        _enemies = new List<EnemyController>();
        _playerBullets = new List<PlayerProjectile>();
        ChronosTime.Plan(_activeTime, delegate { Despawn(); });
    }

    private void Despawn()
    {
        foreach(EnemyController enemy in _enemies)
        {
            enemy.Stats.Damage(_playerBullets.Count);
        }

        foreach (PlayerProjectile projectile in _playerBullets)
        {
            projectile.Despawn();
        }

        MF_AutoPool.Despawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(123);
        if (collision.CompareTag("Enemy"))
        {
            _enemies.Add(collision.GetComponent<EnemyController>());
        }

        if (collision.CompareTag("PlayerProjectile"))
        {
            PlayerProjectile projectile = collision.GetComponent<PlayerProjectile>();
            projectile.ChronosTime.rigidbody2D.velocity = new Vector2();
            _playerBullets.Add(projectile);
        }
    }
}
