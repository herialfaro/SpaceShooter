using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(ActiveEnemiesManager))]
public class SpaceShooterManager : MonoBehaviour
{
    #region Fields
    public static SpaceShooterManager Instance { get; private set; }
    private ActiveEnemiesManager activeEnemiesManager;

    [field: SerializeField] public Transform _EntityReturnPool { get; private set; }
    [field: SerializeField] public Transform _PlayerSpawnPoint {  get; private set; }
    [field: SerializeField] public GameObject[] bulletTypes { get; private set; }

    [SerializeField] private Transform _EntityParentObject;
    [SerializeField] private AudioSource _MusicSource, _SFXSource;
    [SerializeField] private AudioClip _CountdownSecond, _CountdownReady;

    [SerializeField] private int i_InitialLives = 3;
    [SerializeField] private int i_initialSeconds = 2;

    private List<GameObject> _entities;
    private List<GameObject> _shootEntities;
    private EntityInformation[] _pausedEntities;
    private bool b_IsPaused = false;
    private bool b_GameStarted = false;
    private int i_RemainingLives;
    private int i_RemainingSeconds;
    #endregion

    #region Monobehaviour Methods
    private void Awake()
    {
        Instance = this;
        _entities = new List<GameObject>();
        _shootEntities = new List<GameObject>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        DOTween.KillAll();
    }

    private void Start()
    {
        if (_EntityParentObject == null)
            return;

        activeEnemiesManager = GetComponent<ActiveEnemiesManager>();

        InitializeGame();
    }
    #endregion

    #region Events
    public event Action onGameOver;
    public event Action onPlayerPaused;
    public void GameOver()
    {
        if(onGameOver != null)
        {
            onGameOver();
            b_GameStarted = false;
        }
    }
    public void PlayerPaused()
    {
        if(onPlayerPaused != null)
        {
            onPlayerPaused();
            SwitchPause();
        }
    }
    #endregion

    #region Public Methods
    public void SetMusicAudioClip(AudioClip clip) => _MusicSource.clip = clip;
    public void SetMusicAudioLoop(bool b_loop) => _MusicSource.loop = b_loop;
    public void PlayMusicAudioClip() => _MusicSource?.Play();
    public void PauseMusicAudioClip() => _MusicSource?.Pause();
    public void StopMusicAudioClip() => _MusicSource?.Stop();
    public void PlaySFXClip(AudioClip clip) => _SFXSource?.PlayOneShot(clip);
    public void AddEntity(GameObject gameObject) => _entities.Add(gameObject);
    public void AddShootEntity(GameObject gameObject) => _shootEntities.Add(gameObject);
    public void PlayerLostLife()
    {
        i_RemainingLives--;
        StartCoroutine(WaitForPlayerToReturnCoroutine());
    }
    #endregion

    #region Private Methods
    private void InitiatePauseEntityArray() => _pausedEntities = new EntityInformation[_entities.Count];
    private void ClearPauseEntityArray() => Array.Clear(_pausedEntities, 0, _pausedEntities.Length);
    private void InitializeGame()
    {
        i_RemainingLives = i_InitialLives;
        i_RemainingSeconds = i_initialSeconds;

        for (int i = 0; i < _EntityParentObject.childCount; i++)
        {
            IDamageable entity = _EntityParentObject.GetChild(i).GetComponent<IDamageable>();
            IShootEntity shootEntity = _EntityParentObject.GetChild(i).GetComponent<IShootEntity>();

            if (entity != null)
                _entities.Add(_EntityParentObject.GetChild(i).gameObject);

            if (shootEntity != null)
            {
                GameObject g_ShootEntity = _EntityParentObject.GetChild(i).gameObject;
                g_ShootEntity.GetComponent<ShootEntity>().InitializeDataValues();
                _shootEntities.Add(g_ShootEntity);
            }
        }

        StartCoroutine(WaitForGameToStart());
        InitializePlayer();
    }
    private void InitializePlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = _PlayerSpawnPoint.position;
        StartCoroutine(PlayerPositionCoroutine(player.transform));
    }
    private void EnablePlayerComponent(bool b_Enable)
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayableEntity>().enabled = b_Enable;
    }
    private void SaveEntitiesPositionAndVelocities()
    {
        for(int i = 0; i < _entities.Count; i++)
        {
            EntityInformation entity = new EntityInformation();
            entity.v_Position = _entities[i].transform.position;
            entity.v_Velocity = _entities[i].GetComponent<Rigidbody2D>().velocity;
            entity.b_Active = _entities[i].activeInHierarchy;
            _pausedEntities[i] = entity;
        }
    }
    private void DisableSavedEntities ()
    {
        foreach(var  entity in _entities)
        {
            entity.SetActive(false);
            entity.transform.position = _EntityReturnPool.position;
            entity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    private void RestoreSavedEntities()
    {
        for (int i = 0; i < _entities.Count; i++)
        {
            if (_pausedEntities[i].b_Active)
            {
                _entities[i].SetActive(true);
                _entities[i].transform.position = _pausedEntities[i].v_Position;
                _entities[i].GetComponent<Rigidbody2D>().velocity = _pausedEntities[i].v_Velocity;
            }
        }
    }
    private void SwitchPause()
    {
        if (b_IsPaused)
        {
            RestoreSavedEntities();
            ClearPauseEntityArray();
        }
        else
        {
            InitiatePauseEntityArray();
            SaveEntitiesPositionAndVelocities();
            DisableSavedEntities();
        }

        b_IsPaused = !b_IsPaused;
    }
    #endregion

    #region IEnums
    private IEnumerator PlayerPositionCoroutine(Transform _Player)
    {
        Tween positionTween = _Player.DOMoveY(_Player.position.y + 4f, 0.5f).SetEase(Ease.Flash);
        yield return positionTween.WaitForCompletion();
        if (b_GameStarted)
            EnablePlayerComponent(true);
        else
            StartCoroutine(WaitForCountdown());
    }

    private IEnumerator WaitForGameToStart()
    {
        yield return new WaitForSeconds(i_initialSeconds + 1.5f);
        b_GameStarted = true;
        EnablePlayerComponent(true);
    }

    private IEnumerator WaitForCountdown()
    {
        PlaySFXClip(_CountdownSecond);
        yield return new WaitForSeconds(1);
        if (i_RemainingSeconds > 0)
        {
            i_RemainingSeconds--;
            StartCoroutine(WaitForCountdown());
        }
        else
        {
            PlaySFXClip(_CountdownReady);
        }
    }

    private IEnumerator WaitForPlayerToReturnCoroutine()
    {
        yield return new WaitForSeconds(3);
        if (i_RemainingLives <= 0)
            GameOver();
        else
            InitializePlayer();
    }
    #endregion
}

public struct EntityInformation
{
    public Vector2 v_Position;
    public Vector2 v_Velocity;
    public bool b_Active;
}