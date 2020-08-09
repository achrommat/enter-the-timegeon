using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : ChronosMonoBehaviour
{
    [SerializeField] private Transform _to;
    public Animator Animator;
    public Collider2D Collider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.PortalManager.HandlePortalActivation(false);
            collision.transform.position = _to.position;
            PlayerController player = collision.GetComponent<PlayerController>();
            player.IsInPortal = true;
            player.ChronosTime.ResetRecording();
            player.ChronosTime.rigidbody2D.velocity = new Vector2();
        }
    }    
}
