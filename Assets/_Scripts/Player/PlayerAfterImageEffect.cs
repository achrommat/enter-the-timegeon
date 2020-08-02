using UnityEngine;

public class PlayerAfterImageEffect : ChronosMonoBehaviour
{
    [SerializeField] private float _activeTime = 0.1f;
    private float _timeActivated;
    private float _alpha;
    [SerializeField] private float _alphaSet = 0.8f;
    private float _alphaMultiplier = 0.85f;

    private Transform _player;

    [SerializeField] private SpriteRenderer _sprite;
    private SpriteRenderer _playerSprite;

    private Color color;

    public void OnSpawned()
    {
        _player = GameManager.Instance.Player.transform;
        _playerSprite = GameManager.Instance.Player.Sprite;

        _alpha = _alphaSet;
        _sprite.sprite = _playerSprite.sprite;
        transform.position = _player.position;
        transform.rotation = _player.rotation;
        _timeActivated = ChronosTime.time;
    }

    private void Update()
    {
        _alpha *= _alphaMultiplier;
        color = new Color(1f, 1f, 1f, _alpha);
        _sprite.color = color;

        if (ChronosTime.time >= (_timeActivated + _activeTime))
        {
            MF_AutoPool.Despawn(gameObject);
        }
    }
}
