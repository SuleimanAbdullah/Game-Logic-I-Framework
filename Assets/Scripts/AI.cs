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

    private GameDevHQ.FileBase.Plugins.FPS_Character_Controller.FPS_Controller _player;
    [SerializeField]
    private int _health = 3;
    [SerializeField]
    private bool _isDead;
    private Animator _anim;

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
        _health = 3;
        _isDead = false;
    }

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<GameDevHQ.FileBase.Plugins.FPS_Character_Controller.FPS_Controller>();
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
                StartCoroutine(Hide());
                break;

            case AIState.Death:
                Die();
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

                if (transform.position.x == _hidingWaypoints[_randomHideWayPointIndex].position.x && _currentState == AIState.Running)
                {
                    _currentState = AIState.Hide;
                }
                else
                {
                    _anim.SetFloat("Speed", 5f);
                    _anim.SetBool("Hiding", false);
                    _agent.SetDestination(_hidingWaypoints[_randomHideWayPointIndex].position);
                }
            }
            else
            {
                Debug.Log("Move forward");
                if (_currentWayPoint == 0)
                {
                    _isAgentReachDestination = false;
                }
                _anim.SetFloat("Speed", 5f);
                _currentWayPoint++;
                _agent.SetDestination(_wayPoints[_currentWayPoint].position);
            }

        }

    }

    IEnumerator Hide()
    {
        _anim.SetFloat("Speed", 0);
        _anim.SetBool("Hiding", true);
        _agent.isStopped = true;
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        _isAgentHiding = false;
        _agent.isStopped = false;
        _anim.SetBool("Hiding", false);
        if (_isDead==false)
        {
            _currentState = AIState.Running;
        }
    }

    public void Damage()
    {
        if (_isDead == false)
        {
            _health--;
        }
        if (_health < 1)
        {
            _health = 0;
            _isDead = true;
            _currentState = AIState.Death;
        }

    }

    private void Die()
    {
        _agent.isStopped = true;
        _anim.SetFloat("Speed", 0);
        _anim.SetBool("Hiding", false);
        _anim.SetTrigger("Death");
        _player.AddPointToPlayer(50);
        //Player add score if you kill enemy
    }
}
