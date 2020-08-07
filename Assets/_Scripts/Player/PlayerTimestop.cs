using UnityEngine;

public class PlayerTimestop : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private AbilityUI _abilityUI;
    [SerializeField] private GameObject _timestop;   
    [SerializeField] private float _timestopCooldown = 5f;
    private float _timestopCooldownTimer;

    private void Update()
    {
        if (!_player.Stats.IsAlive())
        {
            return;
        }

        if (_timestopCooldownTimer > 0)
        {
            _timestopCooldownTimer -= ChronosTime.deltaTime;
            _abilityUI.CooldownTimer.text = Mathf.Floor(_timestopCooldownTimer).ToString();
        }
        else
        {
            _timestopCooldownTimer = 0;
            _abilityUI.Cooldown.gameObject.SetActive(false);
        }

        CreateTimestop();
    }

    private void CreateTimestop()
    {
        if (!_player.Stats.HasShards())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && _timestopCooldownTimer == 0 && _player.State == PlayerState.UNDER_CONTROL)
        {
            _player.Stats.TakeShard();
            Vector2 pos = new Vector2(_player.MousePos.x, _player.MousePos.y);
            GameObject timestopObj = MF_AutoPool.Spawn(_timestop, pos, Quaternion.identity);
            timestopObj.GetComponent<Timestop>().OnSpawned();
            _timestopCooldownTimer = _timestopCooldown;
            _abilityUI.Cooldown.gameObject.SetActive(true);
        }
    }
}
