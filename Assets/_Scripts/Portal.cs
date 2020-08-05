using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform _to;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.PortalManager.HandlePortalActivation(false);
            collision.transform.position = _to.position;
            collision.GetComponent<PlayerController>().ChronosTime.ResetRecording();
        }
    }
}
