﻿using Chronos;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRewind : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private AbilityUI _abilityUI;
    [SerializeField] private HealthBar _rewindBar;

    [SerializeField] private float _healAmount = 2f;    
    [SerializeField] private float _rewindCooldown = 5f;
    private float _rewindCooldownTimer;
    [SerializeField] private float _rewindCapacity;
    [SerializeField] private float _currentCapacity;
    private float _currentCapacityUI;
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

        if (_rewindCooldownTimer > 0)
        {
            _rewindCooldownTimer -= ChronosTime.deltaTime;
            _abilityUI.CooldownTimer.text = Mathf.Floor(_rewindCooldownTimer).ToString();
        }
        else
        {
            _rewindCooldownTimer = 0;
            _abilityUI.Cooldown.gameObject.SetActive(false);
        }

        Rewind();

        _rewindBar.Bar.localScale = new Vector3(_currentCapacityUI / _rewindCapacity, _rewindBar.Bar.localScale.y, _rewindBar.Bar.localScale.z);
    }   

    private void Rewind()
    {
        if (Input.GetKey(KeyCode.E) && _currentCapacity >= 0 && _rewindCooldownTimer == 0)
        {
            if (_player.State != PlayerState.REWIND)
            {
                if (!_player.Stats.HasShards())
                {
                    return;
                }
                _currentCapacity = _rewindCapacity;
                _currentCapacityUI = _currentCapacity;
                _player.Stats.TakeShard();
                _capacityTimer = ChronosTime.unscaledTime + _tick;
                _rewindBar.gameObject.SetActive(true);
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
        _currentCapacityUI += ChronosTime.deltaTime;

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

            _rewindCooldownTimer = _rewindCooldown;
            _abilityUI.Cooldown.gameObject.SetActive(true);
            _rewindBar.gameObject.SetActive(false);
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
