using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewind : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    private float _nextRewindTime;
    [SerializeField] private float _rewindCooldown = 5f;
    [SerializeField] private float _rewindDuration = 3f;
    private PlayerState _previousPlayerState;

    void Update()
    {
        if (!_player.Stats.IsAlive() || _player.State != PlayerState.UNDER_CONTROL)
        {
            return;
        }

        Rewind();
    }

    private void Rewind()
    {
        if (Input.GetKeyDown(KeyCode.E) && ChronosTime.unscaledTime >= _nextRewindTime)
        {
            _previousPlayerState = _player.State;
            _player.State = PlayerState.REWIND;
            
            ChangeTimeScale("Player", -1f);
            ChronosTime.Memory(-_rewindDuration, false, delegate { Debug.Log(123); }, delegate { StopRewind(); });
            _nextRewindTime = ChronosTime.unscaledTime + _rewindCooldown;
        }
    }

    private void StopRewind()
    {
        if (_player.State == PlayerState.REWIND)
        {
            _player.State = _previousPlayerState;
            _player.Stats.Heal(2f);
            ChangeTimeScale("Player", 1f);
        }
    }

    private void ChangeTimeScale(string clockName, float timeScale)
    {
        GlobalClock clock = Timekeeper.instance.Clock(clockName);
        clock.localTimeScale = timeScale;
    }
}
