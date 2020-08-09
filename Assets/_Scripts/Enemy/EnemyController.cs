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
    public EnemyDropLoot DropLoot;
    public Animator Animator;

    [Header("Behaviour")]
    [SerializeField] private float _attackDistance = 10f;
    private PlayerController _player;
    protected bool _isDead = false;
    public GameObject GroupProjectile;
    public int ShotCount = 0;
    public bool IsSpawned = false;
    [SerializeField] protected GameObject _explosion;
    [SerializeField] private float _explosionChance = 0.2f;

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
        if (Path)
        {
            Path.DestinationSetter.target = _player.transform;
            Path.canMove = false;
        }
        ChronosTime.rewindable = true;
        ChronosTime.Plan(0.5f, delegate () { AfterSpawned(); });
    }

    private void AfterSpawned()
    {
        IsSpawned = true;
        if (Path)
        {
            Path.canMove = true;
        }
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
        Animator.SetFloat("Speed", Path.velocity.magnitude);

        if (!Stats.IsAlive())
        {
            DeathHandler();
            return;
        }
        FlipSprite();
        Shoot();
    }

    private void FlipSprite()
    {
        if (!Path)
        {
            return;
        }

        float speed = Path.velocity.x;
        if (speed < 0)
        {
            Sprite.flipX = true;
        }
        else if (speed > 0)
        {
            Sprite.flipX = false;
        }
    }

    protected virtual void DeathHandler()
    {
        if (!_isDead)
        {
            Explode();


            if (Path)
            {
                Path.canMove = false;
            }
            ChronosTime.rewindable = false;
            GameObject x = MF_AutoPool.Spawn(GameManager.Instance.DeathX, Stats.DamageFeedback.transform.position, Quaternion.identity);
            x.GetComponent<FX>().OnSpawned();
            Animator.SetTrigger("Death");
            IsSpawned = false;
            DropLoot.Drop();

            ChronosTime.Plan(2f, delegate () { 
                MF_AutoPool.Despawn(gameObject);
            });
            //MF_AutoPool.Despawn(gameObject);
            _isDead = true;
        }
    }

    protected virtual void Explode()
    {
        float random = Random.Range(0, 1f);
        if (random <= _explosionChance)
        {
            ChronosTime.Plan(0.1f, delegate {
                GameObject exp = MF_AutoPool.Spawn(_explosion, transform.position, Quaternion.identity);
                exp.GetComponent<Explosion>().OnSpawned();
                GameManager.Instance.HitStop.Stop(0.1f);
            });            
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}
