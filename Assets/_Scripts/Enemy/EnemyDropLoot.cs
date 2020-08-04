using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropLoot : MonoBehaviour
{
    [SerializeField] private GameObject _shardPrefab;
    [SerializeField] private float _probability = 0.25f;

    public void Drop()
    {
        float random = Random.Range(0f, 1f);
        if (random <= _probability)
        {
            GameObject shardObj = MF_AutoPool.Spawn(_shardPrefab, transform.position, Quaternion.identity);
            shardObj.GetComponent<Shard>().Drop();
        }
    }
}
