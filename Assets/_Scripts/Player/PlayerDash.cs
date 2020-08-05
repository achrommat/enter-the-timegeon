using Chronos;
using MoreMountains.Tools;
using UnityEngine;

public class PlayerDash : ChronosMonoBehaviour
{
    [Header("Links")]
    [SerializeField] private PlayerController _player;
    [SerializeField] private AbilityUI _abilityUI;

    [Header("Dash settings")]
    public int MaxDashCount = 3;
    public int CurrentDashCount;
    [SerializeField] private float _dashForce = 5;
    private float _nextDashTime;
    [SerializeField] private float _dashCooldown = 1;
    [SerializeField] private float _dashDuration = 0.1f;
    private PlayerState _previousPlayerState;
    [SerializeField] private float _timeToRestoreDash = 3f;
    private float _dashRestorationTimer;
    private bool _canRestoreDash = false;

    [Header("SlowTime")]
    private float _timeToHold;
    private float _holdTimer;
    [SerializeField] private float _slowTimeScale = 0.5f;
    [SerializeField] private float _playerSlowTimeScale = 0.6f;
    [SerializeField] private float _slowTimeDuration = 0.5f;
    [SerializeField] private float _slowTimeCooldown = 5f;
    private float _slowtimeCooldownTimer;
    private bool _canSlowTime = false;

    [Header("After Image")]
    private Vector2 _lastImagePos;
    [SerializeField] private float _distanceBetweenImages;
    [SerializeField] private GameObject _afterImage;    

    private void Start()
    {
        _timeToHold = _dashDuration;
        CurrentDashCount = MaxDashCount;
    }

    private void Update()
    {
        if (!_player.Stats.IsAlive())
        {
            return;
        }

        RestoreDashCount();

        if (_player.State == PlayerState.DASH)
        {
            if (Vector2.Distance(transform.position, _lastImagePos) > _distanceBetweenImages)
            {                
                SpawnAfterImage();
            }
        }

        if (GetCanDash())
        {
            Initialize();
            SlowTime();
        }

        if (_slowtimeCooldownTimer > 0)
        {
            _slowtimeCooldownTimer -= ChronosTime.deltaTime;
            _abilityUI.CooldownTimer.text = Mathf.Floor(_slowtimeCooldownTimer).ToString();
        }
        else
        {
            _slowtimeCooldownTimer = 0;
            _abilityUI.Cooldown.gameObject.SetActive(false);
        }

    }

    private void SlowTime()
    {
        if (!_player.Stats.HasShards())
        {
            return;
        }

        if (ChronosTime.unscaledTime >= _holdTimer && _canSlowTime && _slowtimeCooldownTimer == 0)
        {
            _player.Stats.TakeShard();

            GlobalClock globalClock = Timekeeper.instance.Clock("Root");
            globalClock.LerpTimeScale(_slowTimeScale, 0.5f);
            GlobalClock playerClock = Timekeeper.instance.Clock("Player");
            playerClock.LerpTimeScale(_playerSlowTimeScale, 0.5f);
            _canSlowTime = false;
            ChronosTime.Plan(_slowTimeDuration, delegate { ResetTimeScale(); });
            _holdTimer = ChronosTime.unscaledTime + _timeToHold;
            _slowtimeCooldownTimer = _slowTimeCooldown;

            _abilityUI.Cooldown.gameObject.SetActive(true);
        }
    }

    private bool GetCanDash()
    {
        bool canDash = _player.State == PlayerState.UNDER_CONTROL && Input.GetKey(KeyCode.Space) &&
            (ChronosTime.rigidbody2D.velocity.magnitude > 0f);
        return canDash;
    }

    public void Initialize()
    {
        if (ChronosTime.time >= _nextDashTime && CurrentDashCount > 0)
        {
            StartDash();
            _nextDashTime = ChronosTime.time + _dashCooldown;
            ChronosTime.Plan(_dashDuration, delegate { StopDash(); });
        }
    }

    private void StartDash()
    {
        CurrentDashCount--;
        GameManager.Instance.Crosshair.UpdateSprite(CurrentDashCount);

        _canSlowTime = true;
        _holdTimer = ChronosTime.unscaledTime + _timeToHold;
        _previousPlayerState = _player.State;
        _player.State = PlayerState.DASH;
        SpawnAfterImage();
        _player.ChronosTime.rigidbody2D.velocity = new Vector2(_player.HorizontalMove, _player.VerticalMove).normalized * _dashForce;
        //_player.Animation.Animator.SetTrigger("Dash");
    }

    private void StopDash()
    {
        if (_player.State == PlayerState.DASH)
        {
            _player.State = _previousPlayerState;           
            ChronosTime.Plan(_dashDuration, delegate { ResetCanSlowTime(); });
        }
    }

    private void ResetCanSlowTime()
    {
        if (_canSlowTime)
        {
            _canSlowTime = false;
        }
    }

    private void ResetTimeScale()
    {
        GlobalClock globalClock = Timekeeper.instance.Clock("Root");
        globalClock.LerpTimeScale(1f, 0.5f);
        GlobalClock playerClock = Timekeeper.instance.Clock("Player");
        playerClock.LerpTimeScale(1f, 0.5f);
    }

    private void SpawnAfterImage()
    {
        _lastImagePos = transform.position;
        GameObject afterImage = MF_AutoPool.Spawn(_afterImage, _lastImagePos, Quaternion.identity);
        afterImage.GetComponent<PlayerAfterImageEffect>().OnSpawned();
    }

    private void RestoreDashCount()
    {
        if (CurrentDashCount < MaxDashCount && !_canRestoreDash)
        {
            _dashRestorationTimer = ChronosTime.time + _timeToRestoreDash;
            _canRestoreDash = true;
        }
        if (ChronosTime.time > _dashRestorationTimer && _canRestoreDash)
        {
            CurrentDashCount++;
            GameManager.Instance.Crosshair.UpdateSprite(CurrentDashCount);
            _canRestoreDash = false;
        }
    }
}
