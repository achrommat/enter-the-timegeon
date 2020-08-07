using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : ChronosMonoBehaviour
{
    [SerializeField] private float _activeTime;

    public void OnSpawned()
    {
        ChronosTime.Plan(_activeTime, delegate { Despawn(); });
    }

    private void Despawn()
    {
        MF_AutoPool.Despawn(gameObject);
    }
}
