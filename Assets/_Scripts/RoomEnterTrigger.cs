using UnityEngine;

public class RoomEnterTrigger : ChronosMonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_spawner.CanSpawn && !_spawner.IsOver)
        {
            GameManager.Instance.PortalManager.HandlePortalActivation(false);
            GameManager.Instance.BattleStartMusic();
            _spawner.CanSpawn = true;
        }
    }
}
