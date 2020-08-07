using UnityEngine;

public class PlayerShield : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private AbilityUI _abilityUI;
    [SerializeField] private GameObject _shield;    
    [SerializeField] private float _shieldCooldown = 5f;
    private float _shieldCooldownTimer;

    private void Update()
    {
        if (!_player.Stats.IsAlive() || _player.State != PlayerState.UNDER_CONTROL)
        {
            return;
        }

        if (_shieldCooldownTimer > 1)
        {
            _shieldCooldownTimer -= ChronosTime.deltaTime;
            _abilityUI.CooldownTimer.text = Mathf.Floor(_shieldCooldownTimer).ToString();
        }
        else
        {
            _shieldCooldownTimer = 0;
            _abilityUI.Cooldown.gameObject.SetActive(false);
        }

        CreateShield();
    }

    private void CreateShield()
    {
        if (!_player.Stats.HasShards())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q) && _shieldCooldownTimer == 0)
        {
            _player.Stats.TakeShard();
            GameObject shieldObj = MF_AutoPool.Spawn(_shield, transform.position, Quaternion.identity);
            shieldObj.GetComponent<Shield>().OnSpawned();
            _shieldCooldownTimer = _shieldCooldown;
            _abilityUI.Cooldown.gameObject.SetActive(true);
        }
    }
}
