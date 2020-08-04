using Chronos;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRewind : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private float _healAmount = 2f;
    private float _nextRewindTime;
    [SerializeField] private float _rewindCooldown = 5f;
    [SerializeField] private float _rewindCapacity;
    private float _currentCapacity;
    private float _capacityTimer;
    private float _tick = 1f;

    private void Start()
    {
        _currentCapacity = _rewindCapacity;
    }

    private void Update()
    {
        if (!_player.Stats.IsAlive())
        {
            return;
        }

        Rewind();
    }   

    private void Rewind()
    {
        if (Input.GetKey(KeyCode.E) && _currentCapacity > 0 && ChronosTime.time >= _nextRewindTime)
        {
            if (_player.State != PlayerState.REWIND)
            {
                if (!_player.Stats.HasShards())
                {
                    return;
                }
                _player.Stats.TakeShard();
            }
            DecreaseCapacity();
            ChangeTimeScale("Player", -1f);
            ChangeTimeScale("Root", 0.2f);
            _player.State = PlayerState.REWIND;
        }

        if (Input.GetKeyUp(KeyCode.E) || _currentCapacity <= 0)
        {
            StopRewind();
        }
    }

    private void DecreaseCapacity()
    {
        if (_player.State != PlayerState.REWIND)
        {
            _capacityTimer = ChronosTime.unscaledTime + _tick;
        }
        if (ChronosTime.unscaledTime >= _capacityTimer)
        {
            _currentCapacity--;
            _capacityTimer = ChronosTime.unscaledTime + _tick;
        }
    }

    private void StopRewind()
    {
        if (_player.State == PlayerState.REWIND)
        {
            _player.State = PlayerState.UNDER_CONTROL;
            ChangeTimeScale("Player", 1f);
            ChangeTimeScale("Root", 1f);
            HealIfCan();
            _currentCapacity = _rewindCapacity;
            _nextRewindTime = ChronosTime.time + _rewindCooldown;
        }
    }

    private void HealIfCan()
    {
        List<float> hpSnapshots = _player.Stats.HPSnapshots.ToList();
        var result = hpSnapshots.FindAll(x => x > _player.Stats.CurrentHealth);
        if (result.Count > 0)
        {
            _player.Stats.Heal(_healAmount);
        }        
    }

    private void ChangeTimeScale(string clockName, float timeScale)
    {
        GlobalClock clock = Timekeeper.instance.Clock(clockName);
        clock.localTimeScale = timeScale;
    }
}
