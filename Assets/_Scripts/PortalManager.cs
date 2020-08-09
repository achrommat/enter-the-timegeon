using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : ChronosMonoBehaviour
{
    [SerializeField] private Portal[] _portals;


    public void HandlePortalActivation(bool activate)
    {
        if (!activate)
        {
            StartCoroutine(ClosePortals());
        }
        foreach (Portal portal in _portals)
        {
            if (!activate)
            {
                portal.Collider.enabled = false;
                portal.Animator.SetTrigger("Close");
                return;
            }
            portal.gameObject.SetActive(activate);
            portal.Collider.enabled = true;
        }
    }

    private IEnumerator ClosePortals()
    {
        yield return ChronosTime.WaitForSeconds(1f);
        foreach (Portal portal in _portals)
        {                       
            portal.gameObject.SetActive(false);
        }
        GameManager.Instance.Player.IsInPortal = false;
    }
}
