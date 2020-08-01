using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerState { UNDER_CONTROL };

public class PlayerController : MonoBehaviour
{
    private PlayerState _state = PlayerState.UNDER_CONTROL;
    public PlayerState State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
        }
    }

    [Header("Links")]
    public Rigidbody2D Rigidbody;
    public Weapon Weapon;
    public PlayerShootable Shootable;
    public Stats Stats;
    public Animator Animator;
    public SpriteRenderer Sprite;
    public Collider2D Collider;
    public HealthBar HealthBar;

    [HideInInspector] public bool IsDead = false;
    [HideInInspector] public Vector2 StartPosition;

    private void Start()
    {
        StartPosition = transform.position;
    }

    private void Death()
    {
        if (!IsDead)
        {
            IsDead = true;
            GameManager.Instance.PlayerDeath();
        }
    }

    private void FixedUpdate()
    {
        
        if (!Stats.IsAlive())
        {
            Death();
            return;
        }

        //HealthBar.Bar.localScale = new Vector3(Stats.CurrentHealth / Stats.MaxHealth, HealthBar.Bar.localScale.y, HealthBar.Bar.localScale.z);

        Move();
        Shoot();
    }

    private void Move()
    {
        float horizontalMove = GetHorizontalInput();
        float verticalMove = GetVerticalInput();
        float horizontalSpeed = horizontalMove * Stats.Speed;
        float verticalSpeed = verticalMove * Stats.Speed;

        Rigidbody.velocity = new Vector2(horizontalSpeed, verticalSpeed);

        if (horizontalMove < 0)
        {
            Sprite.flipX = true;
        }
        if (horizontalMove > 0)
        {
            Sprite.flipX = false;
        }
    }

    private float GetHorizontalInput()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        return horizontalMove;
    }

    private float GetVerticalInput()
    {
        float verticalMove = Input.GetAxis("Vertical");
        return verticalMove;
    }

    private void Shoot()
    {
        if (Input.GetKey(KeyCode.RightControl))
        {
            Shootable.Shoot();
        }
    }
}
