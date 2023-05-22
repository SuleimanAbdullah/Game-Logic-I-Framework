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

    [SerializeField]
    private int _amountOfAI;

    [SerializeField]
    private Transform _spawnPos;
    
    private static PoolManager _instance;

    public static PoolManager Instance
    {
        get
        {
            if (_instance ==null)
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
            if (enemy.activeInHierarchy == false)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }

        GameObject newAIEnemy = Instantiate(_objectTobePooled);
        newAIEnemy.transform.parent = _aiContainer.transform;
        _aiObjects.Add(newAIEnemy);

        return newAIEnemy;
    }
}
