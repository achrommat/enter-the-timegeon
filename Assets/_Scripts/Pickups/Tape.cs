using UnityEngine;

public class Tape : PickupObjectBase
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats stats;
            if ((stats = collision.GetComponent(typeof(PlayerStats)) as PlayerStats) != null)
            {
                if (stats.IsAlive() && stats.CanTakeShards())
                {
                    stats.AddShard();
                    MF_AutoPool.Despawn(gameObject);
                }
            }
        }
    }

    protected override void FlybackToPlayer()
    {
        if (Vector2.Distance(transform.position, _player.transform.position) <= _flybackDistance && _player.Stats.CanTakeShards())
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _flybackSpeed * ChronosTime.deltaTime);
        }
    }
}
