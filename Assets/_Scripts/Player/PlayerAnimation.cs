using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        _player.Animator.SetFloat("Speed", _player.Rigidbody.velocity.magnitude);
    }
}
