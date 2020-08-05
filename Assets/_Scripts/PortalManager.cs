using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private Transform[] _portals;

    public void HandlePortalActivation(bool activate)
    {
        foreach (Transform portal in _portals)
        {
            portal.gameObject.SetActive(activate);
        }
    }
}
