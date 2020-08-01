using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [HideInInspector] public float Speed;
    [HideInInspector] public float Lifetime;
    [HideInInspector] public float CurrentLifetime;
    [HideInInspector] public float Damage;

    public void Initialize(Vector3 direction)
    {
        StartCoroutine(DespawnAfterTime());
        //_rb.velocity = direction * Speed;
        _rb.AddForce(direction * Speed, ForceMode2D.Impulse);
    }

    IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(Lifetime);
        MF_AutoPool.Despawn(gameObject);
    }
}
