using UnityEngine;
using UnityEngine.Events;

public class BossRoomEnterTrigger : ChronosMonoBehaviour
{
    [SerializeField] private UnityEvent _onPlayerEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.PortalManager.HandlePortalActivation(false);
            GameManager.Instance.ActivateBossDialog();
            //_onPlayerEntered.Invoke();            
        }
    }
}
