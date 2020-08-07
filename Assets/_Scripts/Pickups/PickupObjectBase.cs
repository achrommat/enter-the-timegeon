using Chronos;
using UnityEngine;

public class PickupObjectBase : ChronosMonoBehaviour
{
    protected PlayerController _player;
    [SerializeField] protected float _flybackDistance = 5f;
    [SerializeField] protected float _flybackSpeed = 5f;
    [SerializeField] protected float _droppingForce, _droppingDuration;
    public float DropChance = 0.5f;
    protected RigidbodyTimeline2D _rb;

    private void Start()
    {
        _player = GameManager.Instance.Player;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) { }

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

    protected virtual void FlybackToPlayer() { }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _flybackDistance);
    }
}
