using UnityEngine;

public class PlayerShield : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private AbilityUI _abilityUI;
    [SerializeField] private GameObject _shield;    
    [SerializeField] private float _shieldCooldown = 5f;
    private float _shieldCooldownTimer;

    [SerializeField] private Sprite[] _icons;
    [SerializeField] private UnityEngine.UI.Image _image;

    private void Update()
    {
        if (!_player.Stats.IsAlive() || _player.State != PlayerState.UNDER_CONTROL || _player.IsInDialog || _player.IsInFinalDialog || _player.IsInPortal)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _image.sprite = _icons[1];
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            _image.sprite = _icons[0];
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
            ChronosTime.Plan(0.1f, delegate () { _abilityUI.Cooldown.gameObject.SetActive(true); });
        }
    }
}
