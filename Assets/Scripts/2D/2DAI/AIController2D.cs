using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Entity2D))]
[RequireComponent(typeof(MoveToPositionAction))]
[RequireComponent(typeof(MoveToRandomPositionAction))]
[RequireComponent(typeof(MoveToTargetAction))]
[RequireComponent(typeof(FollowTargetAction))]
public class AIController2D : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    public NavMeshAgent Agent
    {
        get
        {
            if (_agent != null)
                return _agent;

            _agent = GetComponentInChildren<NavMeshAgent>();

            return _agent;
        }
    }

    private Entity2D _entity;
    public Entity2D Entity
    {
        get
        {
            if (_entity == null)
            {
                _entity = GetComponent<Entity2D>();
            }

            return _entity;
        }
    }

    private IAAction2D _currentAction;
    private MoveToPositionAction _moveToPositionAction;
    private MoveToRandomPositionAction _moveToRandomPositionAction;
    private MoveToTargetAction _moveToTargetAction;
    private FollowTargetAction _followTargetAction;
    private float _idleTimer = 0;
    private float _idlingLimit;
    private Vector2 _pivot;
    public Vector2 Pivot => _pivot;
    public void SetPivot(Vector2 pivot)
    {
        _pivot = pivot;
    }
    [SerializeField] private Transform _navigationTransformFixer;
    [SerializeField] private AIController2D _target;
    public AIController2D Target => _target;
    public void SetTaret(AIController2D target)
    {
        _target = target;
    }
    [SerializeField] private float _minIdleTimer = 1.5f;
    [SerializeField] private float _maxIdletimer = 3f;
    [SerializeField] private Transform _root;
    [SerializeField] private Transform _scalingTarget;
    [SerializeField] private Transform _mirrorTarget;
    [SerializeField] private List<Transform> _sprites;
    public Transform Root => _root;
    public Transform ScalingTarget => _scalingTarget;
    public Transform MirrorTarget => _mirrorTarget;
    public Transform PositionTarget => Agent.transform;
    public FacingDirections FacingDirection { get; private set; } = FacingDirections.RIGHT;
    private Vector3 _originalScale;
    public Vector3 OriginalScale => _originalScale;

    [Header("Ai test")]
    [SerializeField] private IAAction2D _mainAction;
    public Vector3 Position => new Vector3(_root.position.x, _root.position.y, 0);

    private void Awake()
    {
        _navigationTransformFixer.rotation = Quaternion.Euler(90, 0, 0);

        _originalScale = _root.localScale;
        _pivot = new Vector2(_root.position.x, _root.position.y);

        //move to position action
        _moveToPositionAction = GetComponent<MoveToPositionAction>();
        _moveToPositionAction.SetAIController(this);

        //move to random position action
        _moveToRandomPositionAction = GetComponent<MoveToRandomPositionAction>();
        _moveToRandomPositionAction.SetAIController(this);

        //move to target action
        _moveToTargetAction = GetComponent<MoveToTargetAction>();
        _moveToTargetAction.SetAIController(this);

        //follow target action
        _followTargetAction = GetComponent<FollowTargetAction>();
        _followTargetAction.SetAIController(this);

        if (_mainAction != null)
        {
            _currentAction = _mainAction;
        }
        else
        {
            _currentAction = _moveToRandomPositionAction;
        }
        ResetIdlingTimer();
    }

    private void OnEnable()
    {
        UnsubscribeToEvents();
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _moveToPositionAction.Started += MoveActionStarted;
        _moveToRandomPositionAction.Started += MoveActionStarted;
        _moveToTargetAction.Started += MoveActionStarted;
        _followTargetAction.Started += MoveActionStarted;

        _moveToPositionAction.Ended += MoveActionEnded;
        _moveToRandomPositionAction.Ended += MoveActionEnded;
        _moveToTargetAction.Ended += MoveActionEnded;
        _followTargetAction.Ended += MoveActionEnded;
    }

    private void UnsubscribeToEvents()
    {
        _moveToPositionAction.Started -= MoveActionStarted;
        _moveToRandomPositionAction.Started -= MoveActionStarted;
        _moveToTargetAction.Started -= MoveActionStarted;
        _followTargetAction.Started -= MoveActionStarted;

        _moveToPositionAction.Ended -= MoveActionEnded;
        _moveToRandomPositionAction.Ended -= MoveActionEnded;
        _moveToTargetAction.Ended -= MoveActionEnded;
        _followTargetAction.Ended -= MoveActionEnded;
    }

    private void MoveActionStarted()
    {
        //ExecuteCurrentAction();
        this.Entity.AnimationController.SetAnimationState(AnimationStates.WALK);
        ResetIdlingTimer();
    }

    private void MoveActionEnded()
    {
        //ExecuteCurrentAction();
        Idle();
        ResetIdlingTimer();
    }

    private void ResetIdlingTimer()
    {
        _idleTimer = 0;
        _idlingLimit = Random.Range(_minIdleTimer, _maxIdletimer);
    }

    public void SetSpritesObjectsLayer(string nameOfLayer)
    {
        _sprites.ForEach(s => s.gameObject.layer = LayerMask.NameToLayer(nameOfLayer));
    }

    public void ExecuteCurrentAction()
    {
        if (_currentAction.Viable())
        {
            _currentAction.Execute();
        }
    }

    public void StopCurrentAction()
    {
        if (_currentAction != null && _currentAction.IsPerforming)
        {
            _currentAction.Interrupt();
        }
    }

    private void Update()
    {
        //if (_agent.enabled)
        //{
        //    _root.transform.position = _agent.transform.position + Vector3.down * _agent.baseOffset;
        //}

        if (_idleTimer >= _idlingLimit)
        {
            if (_currentAction == null )
            {
                if(_mainAction != null)
                {
                    _currentAction = _mainAction;
                }
            }

            if (_currentAction != null && !_currentAction.IsPerforming && _agent.enabled)
            {
                ExecuteCurrentAction();
            }   
        }
        else
        {
            _idleTimer += Time.deltaTime;
        }
    }

    public void Idle()
    {
        StopCurrentAction();
        this.Entity.AnimationController.SetAnimationState(AnimationStates.IDLE);
        _currentAction = null;
        if (Agent.enabled)
        {
            Agent.isStopped = true;
        }
    }

    public void SetAgentState(bool agentEnabled)
    {
        Agent.enabled = agentEnabled;
    }

    public void UpdateFacingDirectionBasedOnPosition(float targetX, bool flipIfSame = false)
    {
        var direction = Root.position.x < targetX ? FacingDirections.RIGHT : FacingDirections.LEFT;
        if (flipIfSame)
        {
            if (Agent.destination.x == targetX)
            {
                if (FacingDirection == FacingDirections.LEFT)
                    direction = FacingDirections.RIGHT;
                else
                    direction = FacingDirections.LEFT;
            }
        }

        switch (direction)
        {
            case FacingDirections.RIGHT:
                _mirrorTarget.localScale = Vector3.one;
                break;
            case FacingDirections.LEFT:
                _mirrorTarget.localScale = new Vector3(-1, 1, 1);
                break;
        }
        FacingDirection = direction;
    }

    public void UpdateFacingDirectionToDestination(float targetX, bool flipIfSame = false)
    {
        var direction = Agent.destination.x > targetX ? FacingDirections.RIGHT : FacingDirections.LEFT;
        if (flipIfSame)
        {
            if (Agent.destination.x == targetX)
            {
                if (FacingDirection == FacingDirections.LEFT)
                    direction = FacingDirections.RIGHT;
                else
                    direction = FacingDirections.LEFT;
            }
        }

        switch (direction)
        {
            case FacingDirections.RIGHT:
                _mirrorTarget.localScale = Vector3.one;
                break;
            case FacingDirections.LEFT:
                _mirrorTarget.localScale = new Vector3(-1, 1, 1);
                break;
        }
        FacingDirection = direction;
    }

    public void UpdateFacingDirectionToDestination(bool flipIfSame = false)
    {
        var direction = Agent.destination.x > _root.position.x ? FacingDirections.RIGHT : FacingDirections.LEFT;
        if (flipIfSame)
        {
            if (Agent.destination.x == _root.position.x)
            {
                if (FacingDirection == FacingDirections.LEFT)
                    direction = FacingDirections.RIGHT;
                else
                    direction = FacingDirections.LEFT;
            }
        }

        switch (direction)
        {
            case FacingDirections.RIGHT:
                _mirrorTarget.localScale = Vector3.one;
                break;
            case FacingDirections.LEFT:
                _mirrorTarget.localScale = new Vector3(-1, 1, 1);
                break;
        }
        FacingDirection = direction;
    }    
}
