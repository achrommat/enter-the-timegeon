using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowCrosshair : MonoBehaviour
{
	private Transform _player;
	private Vector3 _target, _refVel;
	private Vector2 _mousePos;
	private float _cameraDist = 3.5f;
	[SerializeField] private float _smoothTime = 0.2f;
	private float _zStart;
	[SerializeField] private Transform _crosshair;

	private void Start()
	{
		_player = GameManager.Instance.Player.transform;
		_target = _player.position;
		_zStart = transform.position.z;
		_crosshair.gameObject.SetActive(true);
		Cursor.visible = false;
	}

    private void FixedUpdate()
	{		
		_mousePos = CaptureMousePos();
		UpdateCrosshairPos();
		_target = UpdateTargetPos();
		UpdateCameraPosition();
	}

	private void UpdateCrosshairPos()
    {
		if (Cursor.visible)
        {
			Cursor.visible = false;
		}
		Vector3 ret = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		ret.z = 0;
		_crosshair.position = ret;
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

		//transform.position = Vector3.Lerp(transform.position, _target, _smoothTime * Time.fixedDeltaTime);

		tempPos = Vector3.SmoothDamp(transform.position, _target, ref _refVel, _smoothTime);
		transform.position = tempPos; 
	}
}
