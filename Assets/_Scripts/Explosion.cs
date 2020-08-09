using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using Chronos;

public class Explosion : ChronosMonoBehaviour
{
    [SerializeField] private float _knockbackForce;
    [SerializeField] private float _activeTime;
    [SerializeField] private float _shakeMagnitude = 1f;
    [SerializeField] private float _shakeRoughness = 1f;
    [SerializeField] private float _shakeFadeInTime = 0.2f;
    [SerializeField] private float _shakeFadeOutTime = 0.2f;

    [SerializeField] private AudioSource _audio;

    [SerializeField] private float _freezeDuration = 0.1f;

    public void OnSpawned()
    {        
        ChronosTime.Plan(_activeTime, delegate { Despawn(); });
        CameraShaker.Instance.ShakeOnce(_shakeMagnitude, _shakeRoughness, _shakeFadeInTime, _shakeFadeOutTime);

        

        //_audio.Play();
    }

    private void Despawn()
    {
        MF_AutoPool.Despawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Stats stats;
        if ((stats = other.gameObject.GetComponent(typeof(Stats)) as Stats) != null)
        {
            Vector2 direction = other.transform.position - transform.position;
            other.GetComponent<Timeline>().rigidbody2D.AddForce(direction * _knockbackForce, ForceMode2D.Force);

            if (other.CompareTag("Player") || other.CompareTag("Boss"))
            {
                stats.Damage(2f);
                return;
            }
            stats.Damage(stats.MaxHealth);
        }
    }
}
