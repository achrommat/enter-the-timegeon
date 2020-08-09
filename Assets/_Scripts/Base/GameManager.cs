using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : ChronosMonoBehaviour
{
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
    }

    #endregion

    [Header("Links")]
    public PlayerController Player;
    public EnemySpawner EnemySpawner;
    public Canvas Canvas;
    public GameObject Clip;
    public AudioSource Audio;
    public PortalManager PortalManager;
    public Crosshair Crosshair;
    public CustomHitStop HitStop;

    public GameObject DeathX;

    public float TimeToRestart = 5f;
    private AudioSource[] allAudioSources;


    public GameObject BossDialog;
    public GameObject BossHP;
    [SerializeField] private UnityEvent _onPlayerEnteredBossArena;
    private bool _bossBattleStarted = false;
    public GameObject FinalDialog;
    public GameObject DeathDialog;

    public EnemySpawner[] Spawners;
    public Boss Boss;

    [SerializeField] private AudioClip _normalClip;
    [SerializeField] private AudioClip _battleClip;
    [SerializeField] private AudioClip _bossClip;
    public AudioSource NormalSource;
    public AudioSource BattleSource;
    public AudioSource BossSource;

    private void Start()
    {
    }

    public void ActivateBossBattle()
    {
        BossDialog.SetActive(false);
        BossHP.SetActive(true);
        _onPlayerEnteredBossArena.Invoke();
        Player.IsInDialog = false;
        _bossBattleStarted = true;
        StartCoroutine(AudioFadeScript.FadeOut(NormalSource, 1f));
        StartCoroutine(AudioFadeScript.FadeIn(BattleSource, 1f));
        StartCoroutine(AudioFadeScript.FadeIn(BossSource, 2f));
    }

    public void BattleStartMusic()
    {
        StartCoroutine(AudioFadeScript.FadeOut(NormalSource, 1f));
        StartCoroutine(AudioFadeScript.FadeIn(BattleSource, 3f));
    }

    public void BattleEndMusic()
    {
        StartCoroutine(AudioFadeScript.FadeOut(BattleSource, 1f));
        StartCoroutine(AudioFadeScript.FadeIn(NormalSource, 3f));
    }

    public void ActivateBossDialog()
    {
        BossDialog.SetActive(true);
        Player.IsInDialog = true;
        Player.ChronosTime.rigidbody2D.velocity = new Vector2();
    }

    public void ActivateFinalDialog()
    {
        FinalDialog.SetActive(true);
        Player.IsInFinalDialog = true;
        Player.ChronosTime.rigidbody2D.velocity = new Vector2();
    }

    public void StopAllAudio()
    {
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        Audio.clip = clip;
        Audio.Play();
        //Audio.clip = null;
    }

    public void PlayerDeath()
    {
        DeathDialog.SetActive(true);
        Player.gameObject.SetActive(false);
        //StartCoroutine(Restart());
    }

    private void Restart()
    {
        //yield return new WaitForSeconds(0.5f);
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in gos)
        {
            MF_AutoPool.Despawn(item);
        }

        GameObject[] gos1 = GameObject.FindGameObjectsWithTag("PlayerProjectile");
        foreach (var item in gos1)
        {
            MF_AutoPool.Despawn(item);
        }

        GameObject[] gos11 = GameObject.FindGameObjectsWithTag("EnemyProjectile");
        foreach (var item in gos11)
        {
            MF_AutoPool.Despawn(item);
        }

        GameObject[] gos2 = GameObject.FindGameObjectsWithTag("Pickup");
        foreach (var item in gos2)
        {
            MF_AutoPool.Despawn(item);
        }

        Player.transform.position = Player.StartPosition;
        Player.gameObject.SetActive(true);

        foreach (var item in Spawners)
        {
            item.ResetSpawner();
        }

        BossDialog.SetActive(false);
        DeathDialog.SetActive(false);
        FinalDialog.SetActive(false);
        BossHP.SetActive(false);

        Boss.Restart();

        PortalManager.HandlePortalActivation(true);
    }

    public void StartGame()
    {
        Clip.SetActive(false);
        EnemySpawner.CanSpawn = true;
        Canvas.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Player.IsDead && Input.GetKeyDown(KeyCode.E))
        {
            Restart();
            //StartCoroutine(Restart());
        }
    }
}
