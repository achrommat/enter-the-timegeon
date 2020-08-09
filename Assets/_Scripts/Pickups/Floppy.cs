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
                    stats.GetComponent<PlayerController>().Animator.SetTrigger("Floppy");
                    stats.GetComponent<PlayerController>().WeaponAnim.gameObject.SetActive(false);
                    GameManager.Instance.ActivateFinalDialog();
                    MF_AutoPool.Despawn(gameObject);
                }
            }
        }
    }

    protected override void Despawn()
    {
        //MF_AutoPool.Despawn(gameObject);
    }
}
