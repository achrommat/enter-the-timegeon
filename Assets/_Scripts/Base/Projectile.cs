using Chronos;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Projectile : ChronosMonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    public float Speed;
    [HideInInspector] public float Lifetime;
    public float CurrentLifetime;
    [HideInInspector] public float Damage;
    [HideInInspector] public bool IsFrozen = false;
    protected bool _isDeflected = false;
    private Vector3 _direction;
    private bool _isInitialized;

    public void Initialize(Vector3 direction, Weapon weapon)
    {
        _direction = direction;
        SetParams(weapon);        
        IsFrozen = false;
        _isDeflected = false;

        ChronosTime.Plan(0.05f, delegate { _isInitialized = true; });
        //_rb.AddForce(direction * Speed, ForceMode2D.Impulse);
        //ChronosTime.rigidbody2D.AddForce(direction * Speed, ForceMode2D.Force);
    }

    private void SetParams(Weapon weapon)
    {
        Speed = weapon.ProjectileSpeed;
        Lifetime = weapon.ProjectileLifetime;
        CurrentLifetime = Lifetime;
        Damage = weapon.Damage;
    }

    private void Update()
    {
        DespawnAfterTime();
        //transform.position = Vector2.MoveTowards(transform.position, transform.position + _direction, Speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (_isInitialized)
        {
            _rb.AddForce(_direction * Speed, ForceMode2D.Impulse);
            _isInitialized = false;
        }
    }

    public void DespawnAfterTime()
    {
        if (IsFrozen)
        {
            return;
        }

        CurrentLifetime -= ChronosTime.deltaTime;
        if (CurrentLifetime <= 0f)
        {
            MF_AutoPool.Despawn(gameObject);
            CurrentLifetime = Lifetime;
        }
    }

    public void Despawn()
    {
        MF_AutoPool.Despawn(gameObject);
    }
}
