using MoreMountains.Feedbacks;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Main Stats")]
    public float MaxHealth = 4;
    public float CurrentHealth;
    public float Speed = 6;

    public MMFeedbacks DamageFeedback;

    public void OnEnable()
    {
        CurrentHealth = MaxHealth;
    }

    public void Damage(float amount)
    {
        if (!IsAlive())
        {
            return;
        }

        CurrentHealth -= amount;

        if (DamageFeedback != null)
        {
            DamageFeedback.PlayFeedbacks();
        }
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }

    public void Heal(float amount)
    {
        if (!IsAlive() || CurrentHealth >= MaxHealth)
        {
            return;
        }

        CurrentHealth += amount;
    }
}
