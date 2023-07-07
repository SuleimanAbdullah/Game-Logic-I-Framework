using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _TransitionCut;
    [SerializeField]
    private GameObject _winText;
    [SerializeField]
    private GameObject _looseText;

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
    private int _amountOfAIBreach;
    private int _totalAI;


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
        _TransitionCut.SetActive(false);
        _winText.SetActive(false);
        _looseText.SetActive(false);

        _remainingAI = _amountOfAI;
        _totalAI = _amountOfAI;

        _aiSpawnedCount = _amountOfAI;
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
            if (enemy.activeInHierarchy == false && _aiSpawnedCount > 0)
            {
                enemy.SetActive(true);
                _aiSpawnedCount--;

                if (_aiSpawnedCount == 0)
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
        if (_remainingAI < 1)//Kill all AI to Win aka wining condition
        {
            //Win Screen
            _TransitionCut.SetActive(true);
            _winText.SetActive(true);
        }
        UIManager.Instance.UpdateEnemyCount(_remainingAI);
    }

    public void EnemyReachDestination()
    {
        _amountOfAIBreach++;
        if (_amountOfAIBreach == _totalAI / 2)//if 50% AI reach destination you loose aka loose condition
        {
            //enable loose screen.
            _TransitionCut.SetActive(true);
            _looseText.SetActive(true);
            StartCoroutine(WaitBeforeDiactivateAI());
        }
    }

    IEnumerator WaitBeforeDiactivateAI()
    {
        foreach (var enemy in _aiObjects)
        {
            if (enemy.activeInHierarchy == true)
            {
                yield return new WaitForSeconds(1.5f);
                enemy.SetActive(false);
            }
        }
    }
}
