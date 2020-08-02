using Chronos;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerState { UNDER_CONTROL, DASH, REWIND };

public class PlayerController : ChronosMonoBehaviour
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
    public PlayerDash Dash;
    public Stats Stats;
    public Animator Animator;
    public SpriteRenderer Sprite;
    public Collider2D Collider;
    public HealthBar HealthBar;

    public float HorizontalMove
    {
        get
        {
            return _horizontalMove;
        }
        set
        {
            _horizontalMove = value;
        }
    }
    public float VerticalMove
    {
        get
        {
            return _verticalMove;
        }
        set
        {
            _verticalMove = value;
        }
    }
    private float _horizontalMove, _verticalMove = 0f;
    [HideInInspector] public bool IsDead = false;
    [HideInInspector] public Vector2 StartPosition;
    public Vector3 MousePos
    {
        get
        {
            return _mousePos;
        }
        set
        {
            _mousePos = value;
        }
    }
    public Vector3 MouseVector
    {
        get
        {
            return _mouseVector;
        }
        set
        {
            _mouseVector = value;
        }
    }
    private Vector3 _mousePos, _mouseVector;

    private void Start()
    {
        GetMouseInput();
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

    private void Update()
    {
        GetInput();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        if (!Stats.IsAlive())
        {
            Death();
            return;
        }

        if (_state != PlayerState.UNDER_CONTROL)
        {
            return;
        }

        //HealthBar.Bar.localScale = new Vector3(Stats.CurrentHealth / Stats.MaxHealth, HealthBar.Bar.localScale.y, HealthBar.Bar.localScale.z);

        Move();
        Shoot();
        InitializeDash();
    }

    private void InitializeDash()
    {
        if (Input.GetKey(KeyCode.Space) && (ChronosTime.rigidbody2D.velocity.magnitude > 0f))
        {
            Dash.Initialize();
        }
    }

    private void GetInput()
    {
        _horizontalMove = Input.GetAxis("Horizontal");
        _verticalMove = Input.GetAxis("Vertical");
        GetMouseInput();
    }

    private void Move()
    {
        float horizontalSpeed = _horizontalMove * Stats.Speed;
        float verticalSpeed = _verticalMove * Stats.Speed;
        ChronosTime.rigidbody2D.velocity = new Vector2(horizontalSpeed, verticalSpeed);
    }

    private void FlipSprite()
    {
        float gunAngle = -1 * Mathf.Atan2(_mouseVector.y, _mouseVector.x) * Mathf.Rad2Deg;
        if ((-90f < gunAngle) && (gunAngle < 90f))
        {
            Sprite.flipX = false;
        }
        else
        {
            Sprite.flipX = true;
        }
    }

    private void GetMouseInput()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = transform.position.z;
        _mouseVector = (_mousePos - transform.position).normalized;
    }

    private void Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shootable.Shoot();
        }
    }
}
