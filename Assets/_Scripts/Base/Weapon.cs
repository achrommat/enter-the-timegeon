using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    // Настройки оружия
    [Header("Main Stats")]
    public float Damage = 1;
    public float Recoil = 1;
    public float ProjectileSpeed = 1;
    public float ProjectileLifetime = 1;
    public bool IsBurst = false;
    public bool IsRadial = false;

    [Header("Fire Rate")]
    public float AttackDelay = 1;

    [Header("Settings")]
    public GameObject Projectile;
    public AudioSource AudioSource;
    public GameObject MuzzleFlash;
}
