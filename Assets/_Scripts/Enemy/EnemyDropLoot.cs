using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropLoot : MonoBehaviour
{
    [SerializeField] private PickupObjectBase[] _dropItems;

    public void Drop()
    {
        foreach (var item in _dropItems)
        {
            float random = Random.Range(0, _dropItems.Length);

            if (random <= item.DropChance)
            {
                GameObject shardObj = MF_AutoPool.Spawn(item.gameObject, transform.position, Quaternion.identity);
                shardObj.GetComponent<PickupObjectBase>().Drop();
            }
        }
    }
}
