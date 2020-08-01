using UnityEngine;

public class PlayerCrosshair : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _crosshairPrefab;
    private GameObject _crosshair;

    void Start()
    {
        Cursor.visible = false;
        _crosshair = MF_AutoPool.Spawn(_crosshairPrefab, _player.MousePos, Quaternion.identity);
    }

    void Update()
    {
        _crosshair.transform.position = new Vector2(_player.MousePos.x, _player.MousePos.y);
    }
}
