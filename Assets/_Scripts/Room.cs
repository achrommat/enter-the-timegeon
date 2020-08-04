using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Door[] _doors;

    private void Start()
    {
        foreach (Door door in _doors)
        {           
            door.ResetState();
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
