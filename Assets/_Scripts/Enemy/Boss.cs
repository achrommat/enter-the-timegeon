using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyController
{
    [Header("Boss")]
    [SerializeField] private int _stage = 0;
    [SerializeField] private float _healthToChangeStage = 5;
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private int _shotCountToSwitch = 3;
    [SerializeField] private int _swithCount = 3;
    [SerializeField] private int _swithCountToShootHard = 3;
    [SerializeField] private int _hardShotCountToSwitch = 3;
    [SerializeField] private HealthBar _healthBar;

    [SerializeField] private Transform[] _explosionPoints;

    public bool IsBattleStarted = false;
    public EnemySpawner Spawner;

    public void Restart()
    {
        IsBattleStarted = false;
        Spawner.CanSpawn = false;
        Stats.CurrentHealth = Stats.MaxHealth;
        _healthBar.Bar.localScale = new Vector3(Stats.CurrentHealth / Stats.MaxHealth, _healthBar.Bar.localScale.y, _healthBar.Bar.localScale.z);
        ShotCount = 0;
        Weapon = _weapons[1];
        _stage = 0;
    }

    public void StartBattle()
    {
        IsBattleStarted = true;
        Spawner.CanSpawn = true;
    }

    protected override void Update()
    {
        if (!IsBattleStarted)
        {
            return;
        }

        _healthBar.Bar.localScale = new Vector3(Stats.CurrentHealth / Stats.MaxHealth, _healthBar.Bar.localScale.y, _healthBar.Bar.localScale.z);

        if (!Stats.IsAlive())
        {
            DeathHandler();
            return;
        }

        ChangeStage();
        Shoot();
    }

    private void ChangeStage()
    {
        if (_stage == 0 && Stats.CurrentHealth <= _healthToChangeStage)
        {
            _stage++;
            Weapon = _weapons[3];
        }        

        if (Stats.CurrentHealth <= 20)
        {
            Spawner.IsOver = true;
        }
    }

    protected override void Shoot()
    {
        SwitchWeapon();
        switch (Weapon.Type)
        {
            case Weapon.TypeEnum.SinCos:
                Shootable.SinCosShoot();
                break;
            case Weapon.TypeEnum.Radial:
                Shootable.HardRadialShoot();
                break;
            default:
                Shootable.GroupShoot();
                break;
        }
    }

    private void SwitchWeapon()
    {
        if (ShotCount >= _shotCountToSwitch)
        {
            if (_stage == 0)
            {
                if (Weapon == _weapons[0])
                {
                    Weapon = _weapons[1];
                    Animator.SetTrigger("Attack");
                }
                else if (Weapon == _weapons[1])
                {
                    Weapon = _weapons[0];
                    Animator.SetTrigger("SinCos");
                }
            }
            else
            {
                if (Weapon == _weapons[2])
                {
                    Weapon = _weapons[3];
                    _swithCount++;
                }
                else if (Weapon == _weapons[3])
                {
                    Weapon = _weapons[2];
                    _swithCount++;
                }
                else if (Weapon == _weapons[4])
                {
                    Weapon = _weapons[2];
                }
            }            
            ShotCount = 0;
        }

        if (_swithCount >= _swithCountToShootHard)
        {
            Weapon = _weapons[4];
            _swithCount = 0;
            _shotCountToSwitch = _hardShotCountToSwitch;
        }
    }

    protected override void DeathHandler()
    {
        if (!_isDead)
        {
            Explode();


            StartCoroutine(AudioFadeScript.FadeOut(GameManager.Instance.BossSource, 1f));
            //StartCoroutine(AudioFadeScript.FadeIn(GameManager.Instance.NormalSource, 3f));

            ChronosTime.rewindable = false;
            //GameObject x = MF_AutoPool.Spawn(GameManager.Instance.DeathX, Stats.DamageFeedback.transform.position, Quaternion.identity);
            //x.GetComponent<FX>().OnSpawned();
            Animator.SetTrigger("Death");
            IsSpawned = false;
            DropLoot.Drop();

            //ChronosTime.Plan(2f, delegate () {
                //MF_AutoPool.Despawn(gameObject);
            //});
            //MF_AutoPool.Despawn(gameObject);
            _isDead = true;
        }
    }

    protected override void Explode()
    {
        StartCoroutine(ExplodeSequentely());
        
        /*foreach (var point in _explosionPoints)
        {
            ChronosTime.Plan(0.1f, delegate {
                
            });
        }   */
    }

    IEnumerator ExplodeSequentely()
    {
        GlobalClock clock = Timekeeper.instance.Clock("Root");
        //clock.localTimeScale = 0.3f;
        GlobalClock clock2 = Timekeeper.instance.Clock("Player");
        //clock2.localTimeScale = 0.3f;
        foreach (var point in _explosionPoints)
        {
            yield return ChronosTime.WaitForSeconds(0.15f);
            GameObject exp = MF_AutoPool.Spawn(_explosion, point.transform.position, Quaternion.identity);
            exp.GetComponent<BoxCollider2D>().enabled = false;
            exp.GetComponent<Explosion>().OnSpawned();            
            GameManager.Instance.HitStop.Stop(0.1f);
        }
        //clock.localTimeScale = 1f;
        //clock2.localTimeScale = 1f;
    }
}
