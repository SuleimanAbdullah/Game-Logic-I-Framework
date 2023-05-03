using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField]
    private Transform[] _wayPoints;
    private int _currentWayPoint = 0;
    private NavMeshAgent _agent;
    private bool _isAgentReachDestination;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(_wayPoints[_currentWayPoint].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.remainingDistance < 1f)
        {
            if (_isAgentReachDestination == false)
            {
                if (_currentWayPoint == _wayPoints.Length - 1)
                {
                    _isAgentReachDestination = true;
                    _currentWayPoint = _wayPoints.Length;
                }
                else
                {
                    if (_currentWayPoint == 0)
                    {
                        _isAgentReachDestination = false;
                    }
                    _currentWayPoint++;
                    _agent.SetDestination(_wayPoints[_currentWayPoint].position);
                }

            }
            else
            {
                _agent.isStopped = true;
                Debug.Log("Agent Reach Destinatio: ");
            }
        }
       

    }
}
