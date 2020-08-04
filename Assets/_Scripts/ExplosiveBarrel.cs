using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : ChronosMonoBehaviour
{
    public Stats Stats;

    [SerializeField] private GameObject _explosionPrefab;
    //private bool _isDead = false;

    private void Update()
    {
        if (!Stats.IsAlive())
        {
            DeathHandler();
        }
    }

    private void DeathHandler()
    {
        GameObject explosionObj = MF_AutoPool.Spawn(_explosionPrefab, transform.position, Quaternion.identity);
        explosionObj.GetComponent<Explosion>().OnSpawned();
        MF_AutoPool.Despawn(gameObject);
    }
}
