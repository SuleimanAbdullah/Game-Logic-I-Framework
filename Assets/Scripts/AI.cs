using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _wayPoints;

    private int _currentWayPoint = 0;
    private NavMeshAgent _agent;
    private bool _isAgentReachDestination;
    
    [SerializeField]
    private GameObject _parentObject;
    [SerializeField]
    private Transform _spawnedPosition;
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
                if (_currentWayPoint == _wayPoints.Count - 1)
                {
                    _isAgentReachDestination = true;
                    _currentWayPoint = _wayPoints.Count;
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
                _parentObject.SetActive(false);
                _currentWayPoint = 0;
                this.gameObject.transform.position = _spawnedPosition.position;
                _isAgentReachDestination = false;
            }

        }
    }

}
