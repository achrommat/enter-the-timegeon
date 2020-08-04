using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Door[] _doors;
    [SerializeField] private Transform[] _explosiveBareelPoint;
    [SerializeField] private GameObject _explosiveBarrelPrefab;

    private void Start()
    {
        foreach (Door door in _doors)
        {           
            door.ResetState();
        }

        foreach (Transform point in _explosiveBareelPoint)
        {
            MF_AutoPool.Spawn(_explosiveBarrelPrefab, point.position, Quaternion.identity);
        }
    }

    public void ChangeDoorStates()
    {
        foreach (Door door in _doors)
        {
            door.ChangeState();
        }
    }
}
