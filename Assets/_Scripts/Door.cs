using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public bool IsClosed = false;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _sprite;

    public void ChangeState()
    {
        IsClosed = !IsClosed;

        _collider.enabled = IsClosed;
        _sprite.enabled = IsClosed;
        /*if (IsClosed)
        {
            _collider.enabled = true;
            _sprite.enabled = true;
        } 
        else
        {
            _collider.enabled = true;
            _sprite.enabled = true;
        }*/
    }

    public void ResetState()
    {
        IsClosed = false;
        _collider.enabled = IsClosed;
        _sprite.enabled = IsClosed;
    }
}
