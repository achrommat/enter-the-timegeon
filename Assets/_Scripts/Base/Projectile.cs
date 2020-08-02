using Chronos;
using System.Collections;
using UnityEngine;

public class Projectile : ChronosMonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Timeline _timeline;
    [HideInInspector] public float Speed;
    [HideInInspector] public float Lifetime;
    [HideInInspector] public float CurrentLifetime;
    [HideInInspector] public float Damage;
    [HideInInspector] public bool IsFrozen = false;

    public void Initialize(Vector3 direction, Weapon weapon)
    {
        IsFrozen = false;
        SetParams(weapon);
        _timeline.Plan(Lifetime, delegate { Despawn(); });
        _rb.AddForce(direction * Speed, ForceMode2D.Impulse);
    }

    private void SetParams(Weapon weapon)
    {
        Speed = weapon.ProjectileSpeed;
        Lifetime = weapon.ProjectileLifetime;
        Damage = weapon.Damage;
    }

    public void Despawn()
    {
        if (IsFrozen)
        {
            return;
        }
        MF_AutoPool.Despawn(gameObject);
    }
}
