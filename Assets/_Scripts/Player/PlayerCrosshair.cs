using UnityEngine;

public class PlayerCrosshair : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _crosshairPrefab;
    private GameObject _crosshair;

    void Start()
    {
        Cursor.visible = false;
        _crosshair = MF_AutoPool.Spawn(_crosshairPrefab, _player.MousePos, Quaternion.identity);
    }

    void FixedUpdate()
    {
        Vector3 ret = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ret.z = transform.position.z;
        //_crosshair.transform.position = ret;
    }
}
