using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnemiesManager : MonoBehaviour
{
    #region Fields
    [field:SerializeField] public int i_MaxEnemiesOnScreen { get; private set; }

    [SerializeField] private Transform[] spawnPoints;

    private List<GameObject> currentEnemyList;
    private List<GameObject> disabledEnemyList;
    #endregion

    #region Monobehaviour Methods
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        currentEnemyList = new List<GameObject>();
        disabledEnemyList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    public void CheckIfNextSpawnIsValid(List<GameObject> shootEntities)
    {
        //Debug.Log(shootEntities.Count);
        currentEnemyList.Clear();
        disabledEnemyList.Clear();
        CheckCurrentEnemies(shootEntities);
        if(currentEnemyList.Count < i_MaxEnemiesOnScreen)
        {
            SpawnNewEnemy();
        }
    }

    private void CheckCurrentEnemies(List<GameObject> shootEntities)
    {
        foreach (var entity in shootEntities)
        {
            if (entity.tag == "Enemy")
                if (entity.activeInHierarchy)
                    currentEnemyList.Add(entity);
                else
                    disabledEnemyList.Add(entity);
        }
    }

    public void SpawnNewEnemy()
    {
        if (disabledEnemyList.Count <= 0)
            return;

        int spawnPoint = UnityEngine.Random.Range(0, spawnPoints.Length);
        int enemyToSpawn = UnityEngine.Random.Range(0, disabledEnemyList.Count);
        disabledEnemyList[enemyToSpawn].transform.position = new Vector3(spawnPoints[spawnPoint].position.x, spawnPoints[spawnPoint].position.y, 0.5f);
        disabledEnemyList[enemyToSpawn].SetActive(true);
    }
    #endregion
}
