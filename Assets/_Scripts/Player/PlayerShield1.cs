using UnityEngine;

public class PlayerShieldOLD : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private AbilityUI _abilityUI;
    [SerializeField] private GameObject _shield;    
    [SerializeField] private float _shieldCooldown = 5f;
    private float _shieldCooldownTimer;

    [SerializeField] private HealthBar _shieldBar;

    [SerializeField] private float _shieldCapacity;
    private float _currentCapacityUI;

    private void Start()
    {
        _currentCapacityUI = _shieldCapacity;
        _abilityUI.Cooldown.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!_player.Stats.IsAlive())
        {
            return;
        }

        if (_shieldCooldownTimer > 0)
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

        _shieldBar.Bar.localScale = new Vector3(_currentCapacityUI / _shieldCapacity, _shieldBar.Bar.localScale.y, _shieldBar.Bar.localScale.z);
    }

    private void CreateShield()
    {
        if (Input.GetKey(KeyCode.Q) && _currentCapacityUI >= 0 && _shieldCooldownTimer == 0)
        {
            if (_player.State != PlayerState.SHIELD)
            {
                if (!_player.Stats.HasShards())
                {
                    return;
                }
                _currentCapacityUI = _shieldCapacity;
                _player.Stats.TakeShard();
                _player.WeaponAnim.gameObject.SetActive(false);
                _player.ChronosTime.rigidbody2D.velocity = new Vector2();

                _player.ChronosTime.Do
                (
                    false, // Repeatable

                    delegate () // On forward
                    {
                        _player.Animator.SetTrigger("Shield");
                        _shieldBar.gameObject.SetActive(true);
                        GameObject shieldObj = MF_AutoPool.Spawn(_shield, transform.position, Quaternion.identity);
                        shieldObj.GetComponent<Shield>().OnSpawned();
                    },
                    delegate () { _player.Animator.SetTrigger("Shield"); }
                );


                
                //ChronosTime.ResetRecording();
            }
            _currentCapacityUI -= ChronosTime.deltaTime;
            _player.State = PlayerState.SHIELD;            
        }

        if (Input.GetKeyUp(KeyCode.Q) || _currentCapacityUI <= 0)
        {
            StopShield();
        }
    }

    private void StopShield()
    {
        if (_player.State == PlayerState.SHIELD)
        {
            
            _player.State = PlayerState.UNDER_CONTROL;
            _player.WeaponAnim.gameObject.SetActive(true);
            _player.ChronosTime.Do
                (
                    false, // Repeatable

                    delegate () // On forward
                    {
                        _player.Animator.SetTrigger("Shield");
                    },
                    delegate () { _player.Animator.SetTrigger("Shield"); }
                );
            _player.ChronosTime.rewindable = false;
            _currentCapacityUI = _shieldCapacity;
            _shieldCooldownTimer = _shieldCooldown;
            _abilityUI.Cooldown.gameObject.SetActive(true);
            _shieldBar.gameObject.SetActive(false);
            
        }
    }    
}
