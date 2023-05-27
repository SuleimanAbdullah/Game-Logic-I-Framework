using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _enemyCountText;

    [SerializeField]
    private Text _timeRemainingText;
    private float _timeRemaining = 180f;
    private bool _isTimeRunning;

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UIManager is NULL:");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _scoreText.text = "Score:" + 0;
        _enemyCountText.text = "EnemyCount: " + 3;
        _isTimeRunning = true;
    }

    private void Update()
    {
        CalculateTime();
        UpdateTimeRemaining(_timeRemaining);
    }

    public void UpdateScore(int amount)
    {
        _scoreText.text = "Score: " + amount;
    }

    public void UpdateEnemyCount(int amount)
    {
        _enemyCountText.text = "Enemy Count: " + amount;
    }

    private void CalculateTime()
    {
        if (_isTimeRunning == true)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                _timeRemaining = 0;
                _isTimeRunning = false;
            }
        }
    }
    void UpdateTimeRemaining(float amount)
    {
        amount += 1;
        float minutes = Mathf.FloorToInt(amount / 60);
        float seconds = Mathf.FloorToInt(amount % 60);
        _timeRemainingText.text = string.Format("Time Remaining {0:00}:{1:00}", minutes, seconds);
    }
}
