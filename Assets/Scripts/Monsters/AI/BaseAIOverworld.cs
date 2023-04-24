using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Monster.AI
{
    public class BaseAIOverworld : MonsterComponent, IAIAgent
    {
        public AIStates State { get; protected set; }
        private AIStates _previousState = AIStates.IDLING;

        [SerializeField] private NavMeshAgent _myAgent;
        [SerializeField] [Range(0f, 10f)] private float _stoppingDistance = 0.25f;
        [SerializeField] [Range(0f, 50f)] private float _acceleration = 5f;
        [SerializeField] [Range(0f, 50f)] private float _runAcceleration = 10f;
        [SerializeField] [Range(0f, 50f)] private float _speed = 5f;
        [SerializeField] [Range(0f, 10f)] private float _runSpeed = 10f;
        [SerializeField] [Range(0f, 2000f)] private float _angularSpeed = 720f;
        [SerializeField] [Range(0f, 2000f)] private float _runAngularSpeed = 1080f;
        private Vector3 _pivot;
        private int _roamingRadius = 3;
        private Vector3 _destination;
        private Transform _target;
        private float _minIndlingTime = 2f;
        private float _maxIndlingTime = 5f;
        private Coroutine _resolveAI;

        private bool _hasReachedDestination
        {
            get
            {
                bool reachedDestination = false;

                if (!_myAgent.pathPending)
                {
                    if (_myAgent.remainingDistance <= _myAgent.stoppingDistance)
                    {
                        if (!_myAgent.hasPath || _myAgent.velocity.sqrMagnitude == 0f)
                        {
                            // Done
                            reachedDestination = true;
                        }
                    }
                }

                return reachedDestination;
            }
        }

        private MonsterAnimationController _animatorController
        {
            get
            {
                MonsterAnimationController animationController = null;
                if (ParentMonster != null)
                {
                    animationController = ParentMonster.GetAnimationController();
                }

                return animationController;
            }
        }

        private void Awake()
        {
            if(_myAgent == null)
            {
                _myAgent = GetComponent<NavMeshAgent>();
            }

            if(ParentMonster == null)
            {
                var parentMon = GetComponent<MonsterEntity>();
                if(parentMon != null)
                {
                    AttachToMonster(parentMon);
                }
            }
        }

        private void OnDisable()
        {
            if (_resolveAI != null)
            {
                StopCoroutine(_resolveAI);
            }
        }

        private void FixedUpdate()
        {
            if (_resolveAI == null)
            {
                _resolveAI = StartCoroutine(ResolveAI());
            }
        }

        IEnumerator ResolveAI()
        {
            if (!this.enabled)
                yield break;

            Debug.Log("started AI coroutine");

            bool wasIdle = _previousState == AIStates.IDLING && State != AIStates.IDLING;

            switch (State)
            {
                case AIStates.IDLING:
                    _myAgent.isStopped = true;
                    if (_animatorController != null && _animatorController.State != MonsterAnimationStates.IDLE)
                    {
                        _animatorController.SetState(MonsterAnimationStates.IDLE);
                    }
                    break;
                case AIStates.ROAMING:
                    if (_hasReachedDestination)
                    {
                        if (_animatorController != null)
                            _animatorController.SetState(MonsterAnimationStates.IDLE);

                        var idling = Random.Range(_minIndlingTime, _maxIndlingTime);
                        yield return new WaitForSeconds(idling);
                        SetDestination(GetRandomDestination());
                    }
                    else
                    {
                        _myAgent.acceleration = _acceleration;
                        _myAgent.speed = _speed;
                        _myAgent.angularSpeed = _angularSpeed;
                        _myAgent.isStopped = false;
                        if(_animatorController != null
                             && _animatorController.State != MonsterAnimationStates.START_WALK
                            && _animatorController.State != MonsterAnimationStates.WALK)
                        {
                            _animatorController.SetState(MonsterAnimationStates.START_WALK);
                        }
                    }
                    break;
                case AIStates.CHASING:
                    if (_hasReachedDestination)
                    {
                        //_myAgent.isStopped = true;
                        if (_animatorController != null)
                            _animatorController.SetState(MonsterAnimationStates.IDLE);

                        wasIdle = true;
                    }
                    else
                    {
                        _myAgent.acceleration = _runAcceleration;
                        _myAgent.speed = _runSpeed;
                        _myAgent.angularSpeed = _runAngularSpeed;
                        _myAgent.isStopped = false;
                        if (_animatorController != null
                             && _animatorController.State != MonsterAnimationStates.START_RUN
                            && _animatorController.State != MonsterAnimationStates.RUN)
                        {
                            _animatorController.SetState(MonsterAnimationStates.START_RUN);
                        }
                    }
                    ChaseTarget();
                    break;
                case AIStates.FOLLOWING:
                    if (_hasReachedDestination)
                    {
                        //_myAgent.isStopped = true;
                        if (_animatorController != null)
                            _animatorController.SetState(MonsterAnimationStates.IDLE);

                        wasIdle = true;
                    }
                    else
                    {
                        _myAgent.acceleration = _acceleration;
                        _myAgent.speed = _speed;
                        _myAgent.angularSpeed = _angularSpeed;
                        _myAgent.isStopped = false;
                        if (_animatorController != null
                             && _animatorController.State != MonsterAnimationStates.START_RUN
                            && _animatorController.State != MonsterAnimationStates.RUN)
                        {
                            _animatorController.SetState(MonsterAnimationStates.START_RUN);
                        }
                    }
                    ChaseTarget();
                    break;
                case AIStates.RESETING:
                    if (_hasReachedDestination)
                    {
                        SetState(AIStates.IDLING);
                        
                        if (_animatorController != null)
                            _animatorController.SetState(MonsterAnimationStates.IDLE);
                    }
                    else
                    {
                        _myAgent.acceleration = _acceleration;
                        _myAgent.speed = _speed;
                        _myAgent.angularSpeed = _angularSpeed;
                        _myAgent.isStopped = false;
                        if (_animatorController != null
                             && _animatorController.State != MonsterAnimationStates.START_WALK
                            && _animatorController.State != MonsterAnimationStates.WALK)
                        {
                            _animatorController.SetState(MonsterAnimationStates.START_WALK);
                        }
                    }
                    break;
                case AIStates.RESETING_TO_ROAM:
                    if (_hasReachedDestination)
                    {
                        SetState(AIStates.ROAMING);
                    }
                    else
                    {
                        _myAgent.acceleration = _acceleration;
                        _myAgent.speed = _speed;
                        _myAgent.angularSpeed = _angularSpeed;
                        _myAgent.isStopped = false;
                        if (_animatorController != null
                             && _animatorController.State != MonsterAnimationStates.START_WALK
                            && _animatorController.State != MonsterAnimationStates.WALK)
                        {
                            _animatorController.SetState(MonsterAnimationStates.START_WALK);
                        }
                    }
                    break;
            }

            Debug.Log("Ended AI coroutine");
            _resolveAI = null;
        }

        public virtual NavMeshAgent GetNavMeshAgent()
        {
            return _myAgent;
        }

        public virtual bool HasReachedDestination() 
        {
            return _hasReachedDestination;
        }

        public virtual bool HasReachedTarget()
        {
            return _hasReachedDestination;
        }

        public virtual void SetState(AIStates state)
        {
            if (!this.enabled)
                return;

            //if (_resolveAI != null)
            //{
            //    StopCoroutine(_resolveAI);
            //}

            _previousState = State;
            State = state;
        }

        public virtual void SetPivot(Vector3 pivot)
        {
            _pivot = pivot;
        }

        public virtual void SetRoamingRadius(int roamingRadius)
        {
            _roamingRadius = roamingRadius;
        }

        public virtual void SetDestination(Vector3 destination)
        {
            _destination = destination;
            _myAgent.SetDestination(_destination);
        }

        public virtual void Roam()
        {
            if (!this.enabled)
                return;

            SetDestination(GetRandomDestination());
            SetState(AIStates.ROAMING);            
        }

        public virtual void Stop()
        {
            if (!this.enabled)
                return;

            SetState(AIStates.IDLING);
        }

        public virtual void Follow(Transform target)
        {
            if (!this.enabled)
                return;

            _target = target;
            SetState(AIStates.FOLLOWING);
            ChaseTarget();
        }

        public virtual void Chase(Transform target)
        {
            if (!this.enabled)
                return;

            _target = target;
            SetState(AIStates.CHASING);
            ChaseTarget();
        }

        private void ChaseTarget()
        {
            if (!this.enabled)
                return;

            if (_target != null)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(_target.transform.position, out hit, _roamingRadius, 1);
                Vector3 finalPosition = hit.position;
                _myAgent.SetDestination(finalPosition);
                _myAgent.isStopped = false;
            }
        }

        public virtual void ResetAI()
        {
            if (!this.enabled)
                return;

            if (_animatorController != null
                && _animatorController.State != MonsterAnimationStates.START_WALK
                && _animatorController.State != MonsterAnimationStates.WALK)
            {
                _animatorController.SetState(MonsterAnimationStates.START_WALK);
            }

            SetDestination(_pivot);
            SetState(AIStates.RESETING);
        }

        public virtual void ResetThenRoam()
        {
            if (!this.enabled)
                return;

            ResetAI();
            SetState(AIStates.RESETING_TO_ROAM);
        }

        public virtual Vector3 GetRandomDestination()
        {
            Vector3 randomDirection = Random.insideUnitCircle * _roamingRadius;
            randomDirection = new Vector3(randomDirection.x, _pivot.y, randomDirection.y);
            randomDirection += _pivot;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, _roamingRadius, 1);
            Vector3 finalPosition = hit.position;
            return finalPosition;
        }

        public virtual void SetAIActiveAState(bool AIState)
        {
            if (!AIState)
                Stop();

            this.enabled = AIState;
        }
        
        public virtual void SetIdlingRange(float minTime, float maxTime)
        {
            _minIndlingTime = minTime;
            _maxIndlingTime = maxTime;
        }
    }
}