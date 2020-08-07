using UnityEngine;

public class Floppy : PickupObjectBase
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats stats;
            if ((stats = collision.GetComponent(typeof(PlayerStats)) as PlayerStats) != null)
            {
                if (stats.IsAlive())
                {
                    
                    MF_AutoPool.Despawn(gameObject);
                }
            }
        }
    }
}
