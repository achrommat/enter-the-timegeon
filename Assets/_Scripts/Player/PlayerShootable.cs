using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerShootable : ChronosMonoBehaviour
{
    [SerializeField] private PlayerController _player;
    public Transform ShootPosition;
    private float _nextAttackTime;

    [Header("Shell")]
    [SerializeField] private GameObject _shellPrefab;
    [SerializeField] private Transform _shellEjectionPosition;
    private LinkedList<BulletShell> _shells;
    [SerializeField] private int _maxShellCount = 10;

    [Header("CameraShake")]
    [SerializeField] private float _shakeMagnitude = 1f;
    [SerializeField] private float _shakeRoughness = 1f;
    [SerializeField] private float _shakeFadeInTime = 0.2f;
    [SerializeField] private float _shakeFadeOutTime = 0.2f;

    private void Start()
    {
        _shells = new LinkedList<BulletShell>();
    }

    public void Shoot()
    {
        if (ChronosTime.unscaledTime >= _nextAttackTime && _player.Weapon.HasAmmo())
        {
            ChronosTime.Do
            (
                false,
                delegate ()
                {
                    _player.WeaponAnim.SetTrigger("Knockback");
                    GameObject bullet = CreateBullet(ShootPosition);
                    return bullet;
                },
                delegate (GameObject bullet)
                {
                    MF_AutoPool.Despawn(bullet);
                }
            );

            ChronosTime.Do
            (
                false,
                delegate ()
                {
                    _player.Weapon.CurrentAmmo--;
                    GameObject shell = CreateBulletShell();
                    return shell;
                },
                delegate (GameObject shell)
                {
                    if (_player.Weapon.CurrentAmmo < _player.Weapon.MaxAmmo)
                    {
                        _player.Weapon.CurrentAmmo++;
                    }                    
                    _shells.Remove(shell.GetComponent<BulletShell>());
                    MF_AutoPool.Despawn(shell);
                }
            );

            //CreateBullet(ShootPosition);
            CameraShaker.Instance.ShakeOnce(_shakeMagnitude, _shakeRoughness, _shakeFadeInTime, _shakeFadeOutTime);
            _nextAttackTime = ChronosTime.unscaledTime + _player.Weapon.AttackDelay;
        }
    }

    private GameObject CreateBullet(Transform shootPosition)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, shootPosition.eulerAngles.z + Random.Range(-_player.Weapon.Recoil, _player.Weapon.Recoil));
        GameObject projectileObj = MF_AutoPool.Spawn(_player.Weapon.Projectile, shootPosition.position, rotation);
        Vector3 velocity = projectileObj.transform.rotation * Vector3.right;
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Initialize(velocity, _player.Weapon);

        //CreateBulletShell();
        return projectileObj;
    }

    public GameObject CreateBulletShell()
    {
        GameObject shellObj = MF_AutoPool.Spawn(_shellPrefab, _shellEjectionPosition.position, Quaternion.identity);
        BulletShell shell = shellObj.GetComponent<BulletShell>();
        shell.Eject();
        HandleShellList(shell);
        return shellObj;
    }

    private void HandleShellList(BulletShell shell)
    {
        
        _shells.AddFirst(shell);
        if (_shells.Count > _maxShellCount)
        {
            _shells.Last.Value.Despawn();
            _shells.RemoveLast();
        }
    }
}
