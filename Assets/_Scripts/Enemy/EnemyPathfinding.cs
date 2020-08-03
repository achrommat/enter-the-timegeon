using GridPathfindingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : ChronosMonoBehaviour
{
    [SerializeField] private EnemyController _enemy;
    private List<Vector3> _pathVectorList;
    private int _currentPathIndex;
    private float _pathfindingTimer;
    private Vector3 _moveDir;
    private Vector3 _lastMoveDir;

    private void Start()
    {
        Vector3 pos = GameManager.Instance.Player.transform.position;
        MoveToTimer(pos);
    }

    private void Update()
    {
        _pathfindingTimer -= Time.deltaTime;

        Move();
    }

    private void FixedUpdate()
    {
        ChronosTime.rigidbody2D.velocity = _moveDir * _enemy.Stats.Speed;
    }

    private void Move()
    {
        PrintPathfindingPath();
        if (_pathVectorList != null)
        {
            Vector3 targetPosition = _pathVectorList[_currentPathIndex];
            float reachedTargetDistance = 5f;
            if (Vector3.Distance(transform.position, targetPosition) > reachedTargetDistance)
            {
                _moveDir = (targetPosition - transform.position).normalized;
                //Debug.Log(moveDir + " " + targetPosition + " " + Vector3.Distance(GetPosition(), targetPosition));
                _lastMoveDir = _moveDir;
            }
            else
            {
                _currentPathIndex++;
                if (_currentPathIndex >= _pathVectorList.Count)
                {
                    _pathVectorList = null;
                    _moveDir = Vector3.zero;
                    MoveToTimer(GameManager.Instance.Player.transform.position);
                }
            }
        }
    }

    public void StopMoving()
    {
        _pathVectorList = null;
        _moveDir = Vector3.zero;
    }

    public List<Vector3> GetPathVectorList()
    {
        return _pathVectorList;
    }

    private void PrintPathfindingPath()
    {
        if (_pathVectorList != null)
        {
            for (int i = 0; i < _pathVectorList.Count - 1; i++)
            {
                Debug.DrawLine(_pathVectorList[i], _pathVectorList[i + 1]);
            }
        }
    }

    public void MoveToTimer(Vector3 targetPosition)
    {
        if (_pathfindingTimer <= 0f)
        {
            SetTargetPosition(targetPosition);
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _currentPathIndex = 0;

        _pathVectorList = GridPathfinding.instance.GetPathRouteWithShortcuts(transform.position, targetPosition).pathVectorList;
        _pathfindingTimer = .1f;
        //pathVectorList = new List<Vector3> { targetPosition };

        if (_pathVectorList != null && _pathVectorList.Count > 1)
        {
            _pathVectorList.RemoveAt(0);
        }
    }

    public Vector3 GetLastMoveDir()
    {
        return _lastMoveDir;
    }

    public void Enable()
    {
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
    }
}
