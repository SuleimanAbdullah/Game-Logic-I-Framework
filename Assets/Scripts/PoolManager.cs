using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _aiContainer;
    [SerializeField]
    private GameObject _objectTobePooled;
    [SerializeField]
    private List<GameObject> _aiObjects;

    GameObject _newAIEnemy;

    [SerializeField]
    private int _amountOfAI;
    [SerializeField]
    public int _aiSpawnedCount;

    private int _remainingAI;


    [SerializeField]
    private Transform _spawnPos;

    private static PoolManager _instance;

    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Pool Manager is Null:");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        _aiObjects = GenerateAI();
    }

    private void Start()
    {
        _remainingAI = _amountOfAI;
    }

    private List<GameObject> GenerateAI()
    {
        for (int i = 0; i < _amountOfAI; i++)
        {
            GameObject enemy = Instantiate(_objectTobePooled);
            enemy.transform.parent = _aiContainer.transform;
            enemy.SetActive(false);
            _aiObjects.Add(enemy);
        }

        return _aiObjects;
    }

    public GameObject RequestPooledAI()
    {
        foreach (var enemy in _aiObjects)
        {
            if (enemy.activeInHierarchy == false && _aiSpawnedCount >0)
            {
                enemy.SetActive(true);
                _aiSpawnedCount--;
                
                if (_aiSpawnedCount ==0)
                {
                    _aiSpawnedCount = 0;
                }

                return enemy;
            }
        }


        if (_amountOfAI < _aiObjects.Count)
        {
            _newAIEnemy = Instantiate(_objectTobePooled);
            _newAIEnemy.transform.parent = _aiContainer.transform;
            _aiObjects.Add(_newAIEnemy);
        }


        return _newAIEnemy;
    }

   public void EnemyCount()
    {
        _remainingAI -= 1;
        UIManager.Instance.UpdateEnemyCount(_remainingAI);
    }
}
