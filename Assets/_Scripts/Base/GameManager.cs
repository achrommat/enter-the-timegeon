using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    public GameObject DeathX;

    public float TimeToRestart = 5f;
    private AudioSource[] allAudioSources;

    private void Start()
    {
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
        Player.gameObject.SetActive(false);
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(TimeToRestart);
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in gos)
        {
            MF_AutoPool.Despawn(item);
        }

        GameObject[] gos1 = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var item in gos1)
        {
            MF_AutoPool.Despawn(item);
        }

        GameObject[] gos2 = GameObject.FindGameObjectsWithTag("Sheep");
        foreach (var item in gos2)
        {
            MF_AutoPool.Despawn(item);
        }

        Player.transform.position = Player.StartPosition;
        Player.gameObject.SetActive(true);
        EnemySpawner.ResetSpawner();
    }

    public void StartGame()
    {
        Clip.SetActive(false);
        EnemySpawner.CanSpawn = true;
        Canvas.gameObject.SetActive(true);
    }

    private void Update()
    {
    }
}
