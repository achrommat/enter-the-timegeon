using UnityEngine;

public class PlayerShield : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _shield;
    private float _nextShieldTime;
    [SerializeField] private float _shieldCooldown = 5f;

    private void Update()
    {
        if (!_player.Stats.IsAlive() || _player.State != PlayerState.UNDER_CONTROL || !_player.Stats.HasShards())
        {
            return;
        }

        CreateShield();
    }

    private void CreateShield()
    {
        if (Input.GetKeyDown(KeyCode.Q) && ChronosTime.unscaledTime >= _nextShieldTime)
        {
            _player.Stats.TakeShard();
            GameObject shieldObj = MF_AutoPool.Spawn(_shield, transform.position, Quaternion.identity);
            shieldObj.GetComponent<Shield>().OnSpawned();
            _nextShieldTime = ChronosTime.unscaledTime + _shieldCooldown;
        }
    }
}
