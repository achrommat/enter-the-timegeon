﻿using Chronos;
using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine;

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
    public PlayerStats Stats;
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

    private bool _isReloading = false;

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

        if (!Stats.IsAlive())
        {
            Death();
            return;
        }
        FlipSprite();
        if ((!Weapon.HasAmmo() || (Input.GetKeyDown(KeyCode.R) && Weapon.CurrentAmmo < Weapon.MaxAmmo)) && !_isReloading)
        {
            StartCoroutine(Reload());
            return;
        }
        Shoot();              
    }

    private void FixedUpdate()
    {
        if (!Stats.IsAlive())
        {
            return;
        }

        if (_state != PlayerState.UNDER_CONTROL)
        {
            return;
        }

        Move();
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        Weapon.CurrentAmmo = 0;
        yield return new WaitForSeconds(2f);
        Weapon.CurrentAmmo = Weapon.MaxAmmo;
        _isReloading = false;
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
        float angle = -1 * Mathf.Atan2(_mouseVector.y, _mouseVector.x) * Mathf.Rad2Deg;
        if ((-90f < angle) && (angle < 90f))
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

    public void Knockback(Vector2 direction, float force)
    {
        ChronosTime.rigidbody2D.AddForce(direction * force, ForceMode2D.Force);
    }
}
