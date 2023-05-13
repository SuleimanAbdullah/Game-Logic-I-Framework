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
    private bool _isAgentHiding;

    [SerializeField]
    private GameObject _parentObject;
    [SerializeField]
    private Transform _spawnedPosition;

    [SerializeField]
    private List<Transform> _hidingWaypoints;
    [SerializeField]
    private int _randomHideWayPointIndex;

    private int _health = 3;
    private bool _isDead;
    private Animator _anim;

    private int _pointToGrantToPlayer;
    private enum AIState
    {
        Running,
        Hide,
        Death
    }

    [SerializeField]
    private AIState _currentState;

    private void OnEnable()
    {
        _randomHideWayPointIndex = Random.Range(0, _hidingWaypoints.Count - 1);
        _isDead = false;
    }
   
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            Debug.LogError("NavmeshAgent is NULL: ");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL: ");
        }
        _agent.SetDestination(_wayPoints[_currentWayPoint].position);
        _currentState = AIState.Running;
        _isAgentHiding = true;
    }

    void Update()
    {


        switch (_currentState)
        {
            case AIState.Running:

                CalculateAIMovement();
                break;
            case AIState.Hide:
                // Ability to stop running
                // when they are at their selected barrier
                // for a random amount of time.
                StartCoroutine(Hide());

                break;
            case AIState.Death:
                Damage();
                //Trigger Death when AI is shot down 
                //Communicate with Animation Scripts 
                //50 Points Awarded

                break;
        }
    }

    private void CalculateAIMovement()
    {
        if (_agent.remainingDistance < 1f)
        {
            if (_isAgentReachDestination == false)
            {
                AIMovement();
                // Intelligently select barriers to run and hide behind.
                // Always moving forward towards the end point
            }

            else
            {
                AIIntrudePlayerGate();

            }
        }
    }

    private void AIIntrudePlayerGate()
    {
        _parentObject.SetActive(false);
        _currentState = AIState.Running;
        _currentWayPoint = 0;
        this.gameObject.transform.position = _spawnedPosition.position;
        _isAgentReachDestination = false;
        _isAgentHiding = true;
    }

    private void AIMovement()
    {
        if (_currentWayPoint == _wayPoints.Count - 1)
        {
            _isAgentReachDestination = true;
            _currentWayPoint = _wayPoints.Count;
        }
       
        else
        {
            if (_isAgentHiding == true)
            {
                Debug.Log("Hide behind the Pillar");
                _agent.SetDestination(_hidingWaypoints[_randomHideWayPointIndex].position);
                if (transform.position.x == _hidingWaypoints[_randomHideWayPointIndex].position.x &&_currentState ==AIState.Running)
                {
                    _currentState = AIState.Hide;
                }
            }
            else
            {
                Debug.Log("Move forward");
                if (_currentWayPoint == 0)
                {
                    _isAgentReachDestination = false;
                }
                _currentWayPoint++;
                _agent.SetDestination(_wayPoints[_currentWayPoint].position);
            }
           
        }
       
    }

    IEnumerator Hide()
    {
        _agent.isStopped = true;
        yield return new WaitForSeconds(Random.Range(3f,6f));
        _isAgentHiding = false;
        _agent.isStopped = false;
        _currentState = AIState.Running;
        Debug.Log("Deactivate this bool"); 
    }

    void Damage()
    {
        if (_isDead ==false)
        {
            _health--;
        }
        if (_health ==0 && _isDead ==false)
        {
            _health = 0;
            _isDead = true;
            _agent.isStopped = true;
            _anim.SetTrigger("OnDeath");
            Destroy(this, 3f);
            //Player add score if you kill enemy
        }
    }

    public void ActivateDeath()
    {
        _currentState = AIState.Death;
    }
}
