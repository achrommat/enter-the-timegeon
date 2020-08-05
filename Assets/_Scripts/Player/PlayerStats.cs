using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private ResourcesUI _resources;

    [Header("HP Snapshots")]
    public LinkedList<float> HPSnapshots;
    [SerializeField] private float _maxSnapshotCount = 5f;
    private float _snapshotTimer;
    private float _tick = 1f;

    [Header("Shards")]
    public float MaxShards = 5f;
    public float CurrentShards;

    public override void OnEnable()
    {
        base.OnEnable();
        HPSnapshots = new LinkedList<float>();
        _snapshotTimer = 0f;
        CurrentShards = MaxShards;
    }

    private void Update()
    {
        CaptureSnapshot();
        UpdateUIElements();
    }

    private void UpdateUIElements()
    {
        _healthBar.Bar.localScale = new Vector3(CurrentHealth / MaxHealth, _healthBar.Bar.localScale.y, _healthBar.Bar.localScale.z);
        _healthBar.Text.text = CurrentHealth + "/" + MaxHealth;
        _resources.TapeText.text = CurrentShards.ToString();
        _resources.AmmoText.text = _player.Weapon.CurrentAmmo.ToString();
    }

    private void CaptureSnapshot()
    {
        if (!IsAlive())
        {
            return;
        }

        _snapshotTimer += ChronosTime.deltaTime;
        if (_snapshotTimer >= _tick)
        {
            if (HPSnapshots.Count >= _maxSnapshotCount)
            {
                HPSnapshots.RemoveFirst();
            }
            HPSnapshots.AddLast(CurrentHealth);
            _snapshotTimer = 0f;
        }
    }

    public override void Damage(float amount)
    {
        if (_player.State == PlayerState.DASH || _player.State == PlayerState.REWIND)
        {
            return;
        }
        base.Damage(amount);
    }

    public bool HasShards()
    {
        return CurrentShards > 0;
    }

    public bool CanTakeShards()
    {
        return CurrentShards < MaxShards;
    }

    public void AddShard()
    {
        if (!IsAlive() || CurrentShards >= MaxShards)
        {
            return;
        }

        CurrentShards++;
    }

    public void TakeShard()
    {
        if (!IsAlive() || CurrentShards <= 0f)
        {
            return;
        }

        CurrentShards--;
    }
}
