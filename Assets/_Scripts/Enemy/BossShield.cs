using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : ChronosMonoBehaviour
{
    private PlayerController _player;
    [SerializeField] private Transform _shield;
    [SerializeField] private float _rotSpeed = 20f;
    private bool _isActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
    }

    public void Activate()
    {
        _shield.gameObject.SetActive(true);
        _isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            _shield.transform.RotateAround(transform.position, Vector3.forward, _rotSpeed * ChronosTime.deltaTime);
        }
    }
}
