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

    public virtual void Damage(float amount)
    {
        if (!IsAlive())
        {
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            if (!IsAlive())
            {
                break;
            }
            CurrentHealth--;
        }

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

        for (int i = 0; i < amount; i++)
        {
            if (CurrentHealth >= MaxHealth)
            {
                break;
            }
            CurrentHealth++;
        }
    }
}
