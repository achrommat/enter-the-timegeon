using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : ChronosMonoBehaviour
{
    [Header("Links")]
    public Stats Stats;
    public Weapon Weapon;
    public EnemyShootable Shootable;
    public Rigidbody2D Rigidbody;
    public SpriteRenderer Sprite;
    public ChronosAIPath Path;

    [Header("Behaviour")]
    [SerializeField] private float _attackDistance = 10f;
    private PlayerController _player;
    protected bool _isDead = false;

    [Header("Patrol")]
    [SerializeField] private float _xBoundary = 28;
    private Vector2 _patrolPointA, _patrolPointB;
    private Vector2[] _patrolPoints;
    private int _randomPoint;
    [SerializeField] private float _waitTime = 1f;
    private float _originWaitTime;
    private Vector2 _changePos;

    public bool IsChangingPos { get; private set; }

    private void Start()
    {
        OnSpawned();
    }

    public virtual void OnSpawned()
    {
        _isDead = false;
        _originWaitTime = _waitTime;
        _player = GameManager.Instance.Player;
        Path.DestinationSetter.target = _player.transform;
        Path.canMove = true;        
    }

    private void GetPatrolPoints()
    {
        _patrolPointA = new Vector2(_xBoundary, transform.position.y);
        _patrolPointB = new Vector2(-_xBoundary, transform.position.y);
        _patrolPoints = new Vector2[] { _patrolPointA, _patrolPointB };
        _randomPoint = Random.Range(0, _patrolPoints.Length);
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
            DeathHandler();
            return;
        }

        //Move();
        Shoot();
        //MoveAway();
    }

    protected virtual void DeathHandler()
    {
        if (!_isDead)
        {
            Path.canMove = false;
            MF_AutoPool.Despawn(gameObject);
            _isDead = true;
        }
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
                    _changePos = new Vector2(Random.Range(transform.position.x - 2, transform.position.x + 2), Random.Range(transform.position.y - 2, transform.position.y + 2));
                    IsChangingPos = true;
                }
            }
        }
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
        if (Vector2.Distance(_player.transform.position, transform.position) > _attackDistance)
        {
            return;
        }

        switch (Weapon.Type)
        {
            case Weapon.TypeEnum.Burst:
                Shootable.Burst();
                break;
            case Weapon.TypeEnum.Shotgun:
                Shootable.ShotgunShoot();
                break;
            case Weapon.TypeEnum.Radial:
                Shootable.RadialShoot();
                break;
            default:
                Shootable.Shoot();
                break;
        }
    }
}
