using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomEnterTrigger : ChronosMonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private UnityEvent _onChangeDoorState;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_spawner.CanSpawn && !_spawner.IsOver)
        {
            _onChangeDoorState.Invoke();
            _spawner.CanSpawn = true;
        }
    }
}
