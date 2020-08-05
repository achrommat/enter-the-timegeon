using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Sprite[] _crosshairs;
    [SerializeField] private SpriteRenderer _sprite;

    public void UpdateSprite(int dashCount)
    {
        _sprite.sprite = _crosshairs[dashCount];
    }
}
