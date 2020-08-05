using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    // Настройки оружия
    [Header("Main Stats")]
    public float Damage = 1f;
    public float Recoil = 1f;
    public float ProjectileSpeed = 1f;
    public float ProjectileLifetime = 1f;
    public int MaxAmmo = 6;
    public int CurrentAmmo;
    
    public enum TypeEnum
    {
        Single,
        Burst,
        Shotgun,
        Radial,
        SinCos
    }
    public TypeEnum Type;

    [Header("Fire Rate")]
    public float AttackDelay = 1f;
    public int ProjectileCount = 3;
    public float ProjectileFireRate = 1f;

    [Header("Settings")]
    public GameObject Projectile;
    public AudioSource AudioSource;
    public GameObject MuzzleFlash;

    public bool HasAmmo()
    {
        return CurrentAmmo > 0;
    }

    private void OnEnable()
    {
        CurrentAmmo = MaxAmmo;
    }
}
