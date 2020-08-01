using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [HideInInspector] public float Speed;
    [HideInInspector] public float Lifetime;
    [HideInInspector] public float CurrentLifetime;
    [HideInInspector] public float Damage;

    public void Initialize(Vector3 direction, Weapon weapon)
    {
        SetParams(weapon);
        StartCoroutine(DespawnAfterTime());
        //_rb.velocity = direction * Speed;
        _rb.AddForce(direction * Speed, ForceMode2D.Impulse);
    }

    private void SetParams(Weapon weapon)
    {
        Speed = weapon.ProjectileSpeed;
        Lifetime = weapon.ProjectileLifetime;
        Damage = weapon.Damage;
    }

    IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(Lifetime);
        MF_AutoPool.Despawn(gameObject);
    }
}
