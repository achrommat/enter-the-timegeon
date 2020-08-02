using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : ChronosMonoBehaviour
{
    public bool Hit = false;
    public Stats Stats;
    public Weapon Weapon;
    public EnemyShootable Shootable;
    public Rigidbody2D Rigidbody;
    public SpriteRenderer Sprite;

    [SerializeField] private float _attackDistance;
    private PlayerController _player;

    public bool IsChangingPos = false;
    private Vector2 _changePos;
    [SerializeField] private float _minX, _maxX, _minY, _maxY;
    private Vector3 _previousPosition;

    protected bool _isDead = false;



    [SerializeField] private float _xBoundary = 28;
    private Vector2 _movePoint, _patrolPointA, _patrolPointB;
    private bool _isAtPoint = false;
    private bool _hasPatrolPoints = false;
    private Vector2[] _patrolPoints;
    private int _randomPoint;
    [SerializeField] private float _waitTime = 1f;
    private float _originWaitTime;

    public virtual void OnSpawned()
    {
        _isDead = false;
    }

    private void Start()
    {
        _originWaitTime = _waitTime;
        _player = GameManager.Instance.Player;
        //Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    public void ResetHit()
    {
        StartCoroutine(Reset());
    } 

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(3f);
        Hit = false;
    }

    private void GetPatrolPoints()
    {
        _patrolPointA = new Vector2(_xBoundary, transform.position.y);
        _patrolPointB = new Vector2(-_xBoundary, transform.position.y);
        _patrolPoints = new Vector2[] { _patrolPointA, _patrolPointB };
        _randomPoint = Random.Range(0, _patrolPoints.Length);
        _hasPatrolPoints = true;
    }

    private void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, _patrolPoints[_randomPoint], Stats.Speed * ChronosTime.deltaTime);

        if (Vector2.Distance(_patrolPoints[_randomPoint], transform.position) < 0.2f)
        {
            _waitTime -= ChronosTime.deltaTime;
            if (_waitTime <= 0f)
            {
                _randomPoint = Random.Range(0, _patrolPoints.Length);
                _waitTime = _originWaitTime;
            }
        }
    }

    protected virtual void Update()
    {
        if (!Stats.IsAlive())
        {
            //DeathHandler();
            return;
        }

        if (!_hasPatrolPoints)
        {
            GetPatrolPoints();
        }
        else
        {
            Patrol();
        }

        /*_previousPosition = transform.position;


        if (GetDirection() < 0)
        {
            Sprite.flipX = true;
        }
        if (GetDirection() > 0)
        {
            Sprite.flipX = false;
        }

        if (Vector2.Distance(transform.position, _player.transform.position) >= _attackDistance)
        {
            Move();
        }
        else
        {
            Shoot();
        }
        MoveAway();*/
    }

    protected virtual void DeathHandler()
    {
        if (!_isDead)
        {
            MF_AutoPool.Despawn(gameObject);

            _isDead = true;
        }
    }

    private float GetDirection()
    {
        Vector3 direction = new Vector3();
        if (_previousPosition != transform.position)
        {
            direction = (_previousPosition - transform.position).normalized;
        }
        return -direction.x;
    }

    protected virtual void Move()
    {
        if (IsChangingPos)
        {
            return;
        }

        GameObject[] gos;
        var hash = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        hash.Remove(gameObject);
        gos = hash.ToArray();
        foreach (GameObject enemy in gos)
        {
            if (enemy != null)
            {
                float currentDistance = Vector2.Distance(transform.position, enemy.transform.position);

                if (currentDistance < 1f && !IsChangingPos && !enemy.GetComponent<EnemyController>().IsChangingPos)
                {
                    _changePos = new Vector2(Random.Range(transform.position.x - _minX, transform.position.x + _maxX), Random.Range(transform.position.y - _minY, transform.position.y + _maxY));
                    IsChangingPos = true;
                }
            }
        }

        Vector2 predictedPoint = new Vector2(_player.transform.position.x, _player.transform.position.y) + _player.Rigidbody.velocity * Time.fixedDeltaTime;
        Vector2 moveVector = (_player.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, predictedPoint, Stats.Speed * Time.deltaTime);   
    }

    private void MoveAway()
    {
        if (!IsChangingPos)
        {
            return;
        }

        StartCoroutine(ResetFlocked());
        if (Vector2.Distance(_changePos, transform.position) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, _changePos, Stats.Speed * Time.deltaTime);
        }
    }

    private IEnumerator ResetFlocked()
    {
        yield return new WaitForSeconds(3f);
        IsChangingPos = false;
    }

    protected virtual void Shoot()
    {
        if (Weapon.IsBurst)
        {
            Shootable.Burst();
            return;
        }
        if (Weapon.IsRadial)
        {
            Shootable.RadialShoot();
            return;
        }
        Shootable.Shoot();
    }
}
