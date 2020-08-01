using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _activeTime;
    [SerializeField] private float _shakeMagnitude = 1f;
    [SerializeField] private float _shakeRoughness = 1f;
    [SerializeField] private float _shakeFadeInTime = 0.2f;
    [SerializeField] private float _shakeFadeOutTime = 0.2f;

    [SerializeField] private AudioSource _audio;

    public void OnSpawned()
    {
        StartCoroutine(Despawn());
        CameraShaker.Instance.ShakeOnce(_shakeMagnitude, _shakeRoughness, _shakeFadeInTime, _shakeFadeOutTime);
        _audio.Play();
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(_activeTime);
        MF_AutoPool.Despawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Stats stats;
            if ((stats = other.gameObject.GetComponent(typeof(Stats)) as Stats) != null)
            {
                stats.Damage(2f);
            }
        }
    }
}
