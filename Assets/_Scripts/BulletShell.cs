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
            transform.rotation = Quaternion.AngleAxis(_rotationForce, Vector3.forward) * transform.rotation;
        }
    }

    public void Eject()
    {
        _isEjected = false;
        _rb = ChronosTime.rigidbody2D;
        _rb.gravityScale = 0.5f;
        AddEjectionForce();
        ChronosTime.Plan(_duration, delegate { StopShell(); });
        _isEjected = true;
    }

    private void AddEjectionForce()
    {
        Vector2 direction = transform.position + transform.right;
        float randomAngle = Random.Range(-65, 65);
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
