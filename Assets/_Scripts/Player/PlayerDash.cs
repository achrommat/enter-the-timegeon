using Chronos;
using UnityEngine;

public class PlayerDash : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;

    [SerializeField] private float _dashForce = 5;
    private float _nextDashTime;
    [SerializeField] private float _dashCooldown = 1;
    [SerializeField] private float _dashDuration = 0.1f;
    [SerializeField] private float _slowTimeWindow = 0.5f;
    private PlayerState _previousPlayerState;

    // After Image
    private Vector2 _lastImagePos;
    [SerializeField] private float _distanceBetweenImages;
    [SerializeField] private GameObject _afterImage;

    [Header("SlowTime")]
    [SerializeField] private float _slowTimeScale = 0.5f;
    [SerializeField] private float _slowTimeDuration = 0.5f;
    private bool _canSlowTime = false;

    private void Update()
    {
        if (_player.State == PlayerState.DASH)
        {
            if (Vector2.Distance(transform.position, _lastImagePos) > _distanceBetweenImages)
            {
                _lastImagePos = transform.position;
                SpawnAfterImage();
            }
        }

        if (Input.GetKey(KeyCode.Space) && _canSlowTime)
        {
            GlobalClock globalClock = Timekeeper.instance.Clock("Root");
            globalClock.LerpTimeScale(_slowTimeScale, 0.5f);

            ChronosTime.Plan(_slowTimeDuration, delegate { ResetTimeScale(); });
        }
    }

    public void Initialize()
    {
        if (_player.State == PlayerState.UNDER_CONTROL && ChronosTime.unscaledTime >= _nextDashTime)
        {
            StartDash();
            _nextDashTime = ChronosTime.unscaledTime + _dashCooldown;
            ChronosTime.Plan(_dashDuration, delegate { StopDash(); });
        }
    }

    private void StartDash()
    {
        _previousPlayerState = _player.State;
        _player.State = PlayerState.DASH;
        _lastImagePos = transform.position;
        SpawnAfterImage();
        //_player.ChronosTime.rigidbody2D.AddForce(new Vector2(_player.HorizontalMove, _player.VerticalMove) * _dashForce, ForceMode2D.Force);
        _player.ChronosTime.rigidbody2D.velocity = new Vector2(_player.HorizontalMove * _dashForce, _player.VerticalMove * _dashForce);
        //_player.Animation.Animator.SetTrigger("Dash");
    }

    private void StopDash()
    {
        if (_player.State == PlayerState.DASH)
        {
            _player.State = _previousPlayerState;
            _canSlowTime = true;
            ChronosTime.Plan(_slowTimeWindow, delegate { ResetCanSlowTime(); });
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
    }

    private void SpawnAfterImage()
    {
        GameObject afterImage = MF_AutoPool.Spawn(_afterImage, _lastImagePos, Quaternion.identity);
        afterImage.GetComponent<PlayerAfterImageEffect>().OnSpawned();
    }
}
