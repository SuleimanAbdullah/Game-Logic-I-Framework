using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;

    public bool _isAISpawned;

    [SerializeField]
    private Transform _spawnPosition;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("SpawnManager is Null:");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {

        SpawnEnemy();
    }

    private IEnumerator SpawnAIRoutine()
    {
        GameObject enemy = PoolManager.Instance.RequestPooledAI();
        AI enemyModel = enemy.GetComponentInChildren<AI>();
        enemyModel.transform.position = _spawnPosition.position;
        _isAISpawned = true;
        yield return new WaitForSeconds(5f);
        if (PoolManager.Instance._aiSpawnedCount > 0)
        {
            _isAISpawned = false;
        }

    }

    public void SpawnEnemy()
    {
        if (_isAISpawned == false)
        {
            StartCoroutine(SpawnAIRoutine());
        }
    }
}
