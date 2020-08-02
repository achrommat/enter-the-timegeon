using Chronos;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Projectile : ChronosMonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [HideInInspector] public float Speed;
    [HideInInspector] public float Lifetime;
    [HideInInspector] public float CurrentLifetime;
    [HideInInspector] public float Damage;
    [HideInInspector] public bool IsFrozen = false;
    protected bool _isDeflected = false;

    public void Initialize(Vector3 direction, Weapon weapon)
    {
        SetParams(weapon);
        CurrentLifetime = Lifetime;
        IsFrozen = false;
        _isDeflected = false;
        ChronosTime.rigidbody2D.AddForce(direction * Speed, ForceMode2D.Impulse);
    }

    private void SetParams(Weapon weapon)
    {
        Speed = weapon.ProjectileSpeed;
        Lifetime = weapon.ProjectileLifetime;
        Damage = weapon.Damage;
    }

    private void Update()
    {
        DespawnAfterTime();
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
            MF_AutoPool.Despawn(gameObject, 2f);
            CurrentLifetime = Lifetime;
        }
    }

    public void Despawn()
    {
        MF_AutoPool.Despawn(gameObject, 2f);
    }
}
