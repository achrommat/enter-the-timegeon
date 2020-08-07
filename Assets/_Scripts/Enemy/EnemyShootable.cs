using Chronos;
using System.Collections;
using System.Globalization;
using UnityEngine;

public class EnemyShootable : ChronosMonoBehaviour
{
    [SerializeField] private EnemyController _enemy;
    [SerializeField] private Transform _shootPosition;
    private float _nextAttackTime;
    private Transform _player;

    private void Start()
    {
        _player = GameManager.Instance.Player.transform;
    }

    private Vector2 GetDirection()
    {
        Vector2 direction;
        float x = _player.position.x + Random.Range(-_enemy.Weapon.Recoil, _enemy.Weapon.Recoil);
        float y = _player.position.y + Random.Range(-_enemy.Weapon.Recoil, _enemy.Weapon.Recoil);
        direction = new Vector2(x, y) - (Vector2)transform.position;
        return direction;
    }

    public void Shoot()
    {
        if (ChronosTime.time >= _nextAttackTime)
        {
            Vector2 direction = GetDirection();
            PlanBullet(direction.normalized, _shootPosition.position);
            //CreateBullet((GameManager.Instance.Player.transform.position - transform.position).normalized);
            _nextAttackTime = ChronosTime.time + _enemy.Weapon.AttackDelay;
        }
    }

    private void PlanBullet(Vector2 direction, Vector2 position)
    {
        _enemy.Animator.SetTrigger("Attack");
        ChronosTime.Do
        (
            false,
            delegate ()
            {
                _enemy.ShotCount++;
                GameObject bullet = CreateBullet(direction, position);
                return bullet;
            },
            delegate (GameObject bullet)
            {
                _enemy.ShotCount--;
                MF_AutoPool.Despawn(bullet, 1f);
            }
        );
    }

    private GameObject CreateBullet(Vector2 direction, Vector2 position)
    {
        GameObject bullet = MF_AutoPool.Spawn(_enemy.Weapon.Projectile, position, Quaternion.identity);
        bullet.GetComponent<Projectile>().Initialize(direction, _enemy.Weapon);
        return bullet;
    }

    public void Burst()
    {
        if (ChronosTime.time >= _nextAttackTime)
        {
            StartCoroutine(BurstFire());
            _nextAttackTime = ChronosTime.time + _enemy.Weapon.AttackDelay;
        }
    }

    private IEnumerator BurstFire()
    {
        for (int i = 0; i < _enemy.Weapon.ProjectileCount; i++)
        {
            Vector2 direction = GetDirection();
            PlanBullet(direction.normalized, _shootPosition.position);
            yield return ChronosTime.WaitForSeconds(_enemy.Weapon.ProjectileFireRate);
        }        
    }

    public void ShotgunShoot()
    {
        if (ChronosTime.time >= _nextAttackTime)
        {
            float coneAngle = 60f;
            float angleStep = coneAngle / _enemy.Weapon.ProjectileCount;
            float angle = ShotgunGetAngle(angleStep);

            for (int i = 0; i < _enemy.Weapon.ProjectileCount; i++)
            {
                Vector3 projectileVector = new Vector3(transform.position.x + Mathf.Cos((angle * Mathf.PI) / 180) * 1f, transform.position.y + Mathf.Sin((angle * Mathf.PI) / 180) * 1f, 0);
                Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized;
                PlanBullet(projectileMoveDirection, _shootPosition.position);
                angle += angleStep;
            }
            _nextAttackTime = ChronosTime.time + _enemy.Weapon.AttackDelay;
        }
    }

    private float ShotgunGetAngle(float angleStep)
    {
        Vector2 directionToPlayer = _player.position - transform.position;
        float sign = (_player.position.y < transform.position.y) ? -1.0f : 1.0f;
        float angleToPlayer = Vector2.Angle(Vector3.right, directionToPlayer) * sign;        
        float angle = angleToPlayer - angleStep * _enemy.Weapon.ProjectileCount / 2;
        return angle;
    }

    public void RadialShoot()
    {
        if (ChronosTime.time >= _nextAttackTime)
        {
            float angleStep = 360f / _enemy.Weapon.ProjectileCount;
            float angle = 0f;

            for (int i = 0; i < _enemy.Weapon.ProjectileCount; i++)
            {
                // Direction calculations.
                float projectileDirXPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * 1f;
                float projectileDirYPosition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * 1f;

                // Create vectors.
                Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
                Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized;
                PlanBullet(projectileMoveDirection, _shootPosition.position);

                angle += angleStep;
            }
            _nextAttackTime = ChronosTime.time + _enemy.Weapon.AttackDelay;
        }
    }

    public void SinCosShoot()
    {
        if (ChronosTime.time >= _nextAttackTime)
        {            
            StartCoroutine(SinCosFire());
            _nextAttackTime = ChronosTime.time + _enemy.Weapon.AttackDelay;
        }
    }

    private IEnumerator SinCosFire()
    {
        float angleStep = 360f / _enemy.Weapon.ProjectileCount;
        float angle = 0f;

        for (int i = 0; i < _enemy.Weapon.ProjectileCount; i++)
        {
            // Direction calculations.
            float projectileDirXPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * 1f;
            float projectileDirYPosition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * 1f;

            // Create vectors.
            Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0f);
            Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized;
            PlanBullet(projectileMoveDirection, _shootPosition.position);
            angle += angleStep;
            yield return ChronosTime.WaitForSeconds(_enemy.Weapon.ProjectileFireRate);
        }
        
    }

    public void GroupShoot()
    {
        if (ChronosTime.time >= _nextAttackTime)
        {            
            ChronosTime.Do
            (
                false,
                delegate ()
                {
                    GameObject bullet = CreateGroupBullet();
                    return bullet;
                },
                delegate (GameObject bullet)
                {
                    MF_AutoPool.Despawn(bullet);
                }
            );
            
            _nextAttackTime = ChronosTime.time + _enemy.Weapon.AttackDelay;            
        }
    }

    private GameObject CreateGroupBullet()
    {
        Vector2 direction = GetDirection();
        GameObject groupProjectileObj = MF_AutoPool.Spawn(_enemy.GroupProjectile, _shootPosition.position, Quaternion.identity);
        groupProjectileObj.GetComponent<EnemyGroupProjectile>().OnSpawned(_shootPosition.position, direction.normalized, _enemy.Weapon);
        return groupProjectileObj;
    }    
}
