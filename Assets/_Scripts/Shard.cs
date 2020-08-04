using Chronos;
using Pathfinding.Util;
using UnityEngine;

public class Shard : ChronosMonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private float _flybackDistance = 5f;
    [SerializeField] private float _flybackSpeed = 5f;
    [SerializeField] private float _droppingForce, _droppingDuration;
    private RigidbodyTimeline2D _rb;

    private void Start()
    {
        _player = GameManager.Instance.Player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

    public void Drop()
    {
        _rb = ChronosTime.rigidbody2D;
        _rb.gravityScale = 0.5f;
        AddDroppingForce();
        ChronosTime.Plan(_droppingDuration, delegate { StopDropping(); });
    }

    private void AddDroppingForce()
    {
        Vector2 direction = Vector2.right;
        float randomAngle = Random.Range(45, 135);
        direction = Quaternion.AngleAxis(randomAngle, Vector3.forward) * direction;
        _rb.AddForce(direction * _droppingForce, ForceMode2D.Force);
    }

    private void StopDropping()
    {
        ChronosTime.rigidbody2D.velocity = new Vector2();
        _rb.gravityScale = 0f;
    }

    private void Update()
    {
        FlybackToPlayer();
    }

    private void FlybackToPlayer()
    {
        if (Vector2.Distance(transform.position, _player.transform.position) <= _flybackDistance && _player.Stats.CanTakeShards())
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _flybackSpeed * ChronosTime.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _flybackDistance);
    }
}
