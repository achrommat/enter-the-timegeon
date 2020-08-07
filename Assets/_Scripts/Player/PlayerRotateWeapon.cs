using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateWeapon : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    public SpriteRenderer WeaponSprite;

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float gunAngle = -1 * Mathf.Atan2(_player.MouseVector.y, _player.MouseVector.x) * Mathf.Rad2Deg;
        WeaponSprite.transform.rotation = Quaternion.AngleAxis(gunAngle, Vector3.back);
        /*WeaponSprite.sortingOrder = _player.Sprite.sortingOrder - 1;
        if (gunAngle > 0)
        {
            WeaponSprite.sortingOrder = _player.Sprite.sortingOrder + 1;
        }*/

        WeaponSprite.flipY = _player.Sprite.flipX;
    }
}
