using System.Collections;
using UnityEngine;

public class EnemyShootable : MonoBehaviour
{
    [SerializeField] private EnemyController _enemy;
    [SerializeField] private Transform _shootPosition;
    private float _nextAttackTime;

    public void Shoot()
    {
        if (Time.time >= _nextAttackTime)
        {
            CreateBullet((GameManager.Instance.Player.transform.position - transform.position).normalized);
            _nextAttackTime = Time.time + _enemy.Weapon.AttackDelay;
        }
    }

    private void CreateBullet(Vector2 direction)
    {
        GameObject bullet = MF_AutoPool.Spawn(_enemy.Weapon.Projectile, transform.position, transform.rotation);
        bullet.GetComponent<Projectile>().Speed = _enemy.Weapon.ProjectileSpeed;
        bullet.GetComponent<Projectile>().Lifetime = _enemy.Weapon.ProjectileLifetime;
        bullet.GetComponent<Projectile>().Damage = _enemy.Weapon.Damage;
        bullet.GetComponent<Projectile>().Initialize(direction);
    }

    public void Burst()
    {
        if (Time.time >= _nextAttackTime)
        {
            StartCoroutine(BurstFire());
            _nextAttackTime = Time.time + _enemy.Weapon.AttackDelay;
        }
    }

    private IEnumerator BurstFire()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector2 direction = new Vector2(GameManager.Instance.Player.transform.position.x + Random.Range(-2, 2), GameManager.Instance.Player.transform.position.y + Random.Range(-2, 2)) - (Vector2)transform.position;
            CreateBullet(direction.normalized);
            yield return new WaitForSeconds(0.5f);
        }        
    }

    public void RadialShoot()
    {
        if (Time.time >= _nextAttackTime)
        {
            float angleStep = 360f / 6;
            float angle = 0f;

            for (int i = 0; i < 6; i++)
            {
                // Direction calculations.
                float projectileDirXPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * 1f;
                float projectileDirYPosition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * 1f;

                // Create vectors.
                Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
                Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized;
                CreateBullet(projectileMoveDirection);

                angle += angleStep;
            }
            _nextAttackTime = Time.time + _enemy.Weapon.AttackDelay;
        }
    }
}
