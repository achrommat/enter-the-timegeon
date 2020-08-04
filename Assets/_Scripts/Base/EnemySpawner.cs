using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    [System.Serializable]
    public class Wave
    {
        public string Name;
        public Transform[] Enemies;
        public int Count;
        public float Rate;
    }
    public Wave[] Waves;
    private int _nextWave = 0;
    public Transform[] SpawnPoints;
    public float TimeBetweenWaves = 5f;
    private float _waveCountdown;
    private float _searchCountdown = 1f;
    public SpawnState State = SpawnState.COUNTING;

    public bool CanSpawn, IsOver = false;
    [SerializeField] private UnityEvent _onChangeDoorState;

    void Start()
    {
        if (SpawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced");
        }
        _waveCountdown = TimeBetweenWaves;
    }

    public void ResetSpawner()
    {
        StopAllCoroutines();
        _waveCountdown = TimeBetweenWaves;
        _nextWave = 0;
        IsOver = false;
    }

    void Update()
    {
        if (!CanSpawn || IsOver)
        {
            return;
        }

        if (State == SpawnState.WAITING)
        {
            // Check if enemies are still alive
            if (!EnemyIsAlive())
            {
                // Begin new round
                WaveCompleted();
                return;
            }
            else
            {
                return;
            }
        }
        if (_waveCountdown <= 0)
        {
            if (State != SpawnState.SPAWNING)
            {
                // Start spawning wave
                StartCoroutine(SpawnWave(Waves[_nextWave]));
            }
        }
        else
        {
            _waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        State = SpawnState.COUNTING;
        _waveCountdown = TimeBetweenWaves;
        if (_nextWave + 1 > Waves.Length - 1)
        {
            _nextWave = 0;
            IsOver = true;
            _onChangeDoorState.Invoke();
            Debug.Log("All Waves Complete Looping...");
        }
        else
        {
            _nextWave++;
        }

    }

    bool EnemyIsAlive()
    {
        _searchCountdown -= Time.deltaTime;
        if (_searchCountdown <= 0f)
        {
            _searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        State = SpawnState.SPAWNING;
        //Spawn
        for (int i = 0; i < _wave.Count; i++)
        {
            SpawnEnemy(_wave.Enemies);
            yield return new WaitForSeconds(1f / _wave.Rate);
        }
        State = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform[] enemies)
    {
        if (SpawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced");
        }
        Transform _sp = SpawnPoints[Random.Range(0, SpawnPoints.Length)];

        int randomIndex = Random.Range(0, enemies.Length);
        GameObject go = MF_AutoPool.Spawn(enemies[randomIndex].gameObject, _sp.position, Quaternion.identity);
        go.GetComponent<EnemyController>().OnSpawned();
    }
}