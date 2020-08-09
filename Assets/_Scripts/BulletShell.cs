using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShell : ChronosMonoBehaviour
{
    [SerializeField] private float _force, _rotationForce, _duration;
    private bool _isEjected = false;
    private RigidbodyTimeline2D _rb;

    private void Update()
    {
        if (_isEjected)
        {
            transform.rotation = Quaternion.AngleAxis(Random.Range(_rotationForce, _rotationForce * 2), Vector3.forward) * transform.rotation;
        }
    }

    public void Eject()
    {
        _isEjected = false;
        _rb = ChronosTime.rigidbody2D;
        _rb.gravityScale = 0.3f;
        AddEjectionForce();
        float randomDuration = Random.Range(_duration, _duration + 0.5f);
        ChronosTime.Plan(randomDuration, delegate { StopShell(); });
        _isEjected = true;
    }

    private void AddEjectionForce()
    {
        Vector2 direction = Vector2.right;
        float randomAngle = Random.Range(45, 180);
        direction = Quaternion.AngleAxis(randomAngle, Vector3.forward) * direction;
        _rb.AddForce(direction * _force, ForceMode2D.Force);
    }

    private void StopShell()
    {
        ChronosTime.rigidbody2D.velocity = new Vector2();
        _isEjected = false;
        _rb.gravityScale = 0f;
    }

    public void Despawn()
    {
        MF_AutoPool.Despawn(gameObject);
    }
}
