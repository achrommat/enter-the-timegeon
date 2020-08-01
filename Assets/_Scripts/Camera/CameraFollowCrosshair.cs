using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowCrosshair : MonoBehaviour
{
	private Transform _player;
	private Vector3 _target, _mousePos, _refVel;
	private float _cameraDist = 3.5f;
	private float _smoothTime = 0.2f;
	private float _zStart;

	private void Start()
	{
		_player = GameManager.Instance.Player.transform;
		_target = _player.position;
		_zStart = transform.position.z;
	}

	private void FixedUpdate()
	{
		_mousePos = CaptureMousePos();
		_target = UpdateTargetPos();
		UpdateCameraPosition();
	}

	private Vector3 CaptureMousePos()
	{
		Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		ret *= 2;
		ret -= Vector2.one;
		float max = 0.9f;
		if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
		{
			ret = ret.normalized;
		}
		return ret;
	}

	private Vector3 UpdateTargetPos()
	{
		Vector3 mouseOffset = _mousePos * _cameraDist; 
		Vector3 ret = _player.position + mouseOffset;
		ret.z = _zStart;
		return ret;
	}

	private void UpdateCameraPosition()
	{
		Vector3 tempPos;
		tempPos = Vector3.SmoothDamp(transform.position, _target, ref _refVel, _smoothTime);
		transform.position = tempPos; 
	}
}
