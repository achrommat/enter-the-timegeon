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

    public PlayerController Player;
    public GameObject ChainLighting;
    public EnemySpawner EnemySpawner;

    public float TimeToRestart = 5f;

    public GameObject Clip;

    public AudioSource Audio;

    [SerializeField] private float _timeToShuffle = 30f;
    private float _startTimeToShuffle;

    private AudioSource[] allAudioSources;

    public Canvas Canvas;

    public void StopAllAudio()
    {
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

    private void Start()
    {
        _startTimeToShuffle = _timeToShuffle;
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

    /*private void ShuffleSpell()
    {
        _timeToShuffle -= Time.fixedDeltaTime;
        if (_timeToShuffle <= 0f)
        {
            SpellGenerator.ShuffleSpell();
            _timeToShuffle = _startTimeToShuffle;
        }
    }*/
}
