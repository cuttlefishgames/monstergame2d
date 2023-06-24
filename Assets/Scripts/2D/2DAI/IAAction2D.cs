using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAAction2D : MonoBehaviour
{
    public delegate void OnIAAction2DStartedEvent();
    public OnIAAction2DStartedEvent Started;

    public delegate void OnIAAction2DEndedEvent();
    public OnIAAction2DEndedEvent Ended;

    [Header("Leave at 0 if there is no time limite")] [SerializeField] [Min(0)] private int _timeLimit = 0;
    protected float _currentTimer;
    protected bool _viable = true;
    public bool IsViable => _viable;
    private bool _isPeforming;
    public bool IsPerforming 
    { 
        get => _isPeforming;
        protected set 
        { 
            _currentTimer = 0;
            _isPeforming = value;
        } 
    }
    protected AIController2D _controller;
    public virtual void SetAIController(AIController2D controller)
    {
        _controller = controller;
    }

    public NavMeshAgent Agent => _controller.Agent;

    public bool ReachedDestination
    {
        get
        {
            bool reachedDestination = false;
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                // Done
                reachedDestination = true;
            }

            return reachedDestination;
        }
    }

    public virtual void Execute() { OnStarted(); }

    public virtual bool Viable() { _viable = true; return _viable; }
    public virtual bool Interrupt()
    {
        _isPeforming = false;
        return true; 
    }

    public virtual void End() { _isPeforming = false; _viable = false; OnEnded(); }
    public virtual void OnStarted() { Started?.Invoke(); }
    public virtual void OnEnded() { Ended?.Invoke(); }

    private void Update()
    {
        if (IsPerforming && _timeLimit > 0)
        {
            _currentTimer += Time.deltaTime;
            if(_currentTimer > _timeLimit)
            {
                End();
            }
        }
    }
}
