using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity2DEvents))]
[RequireComponent(typeof(Entity2DAnimationController))]
[RequireComponent(typeof(TransformPropertiesTransfer))]
public class Entity2D : MonoBehaviour
{
    public int ActionPoints { get; private set; }
    public TeamSides Side => _side;
    public Transform Root => transform;
    public Transform ScalingTarget => transform;
    public Entity2DStats BaseStats => _baseStats;
    [SerializeField] private Entity2DStats _baseStats;
    //[SerializeField] private GameObject _genericAnimatorPrefab;
    //[SerializeField] private Transform _root;
    //[SerializeField] private Transform _scalingTarget;
    [SerializeField] private Animator _animator;
    //private Animator _genericAnimator;
    private TeamSides _side = TeamSides.LEFT;
    
    private Entity2DEvents _events;
    public Entity2DEvents Events
    {
        get
        {
            if (_events == null)
            {
                _events = GetComponent<Entity2DEvents>();
            }
                
            return _events;
        }
    }

    private Entity2DAnimationController _animationController;
    public Entity2DAnimationController AnimationController
    {
        get
        {
            if (_animationController == null)
            {
                _animationController = GetComponent<Entity2DAnimationController>();
            }

            return _animationController;
        }
    }

    private TransformPropertiesTransfer _transferer;
    public TransformPropertiesTransfer Transferer
    {
        get
        {
            if (_transferer == null)
            {
                _transferer = GetComponent<TransformPropertiesTransfer>();
            }

            return _transferer;
        }
    }

    private void Awake()
    {
        //set parent references
        Events.SetParentEntity(this);
        AnimationController.SetParentEntity(this);

        //if(_genericAnimator == null)
        //{
        //    var animatorObject = Instantiate(_genericAnimatorPrefab);
        //    var transferer = animatorObject.GetComponent<Transferer>();
        //    _genericAnimator = transferer.Animator;
        //    animatorObject.name = name + "Generic Animator";
        //    animatorObject.transform.SetParent(transform);
        //    Transferer.FillBones(transferer.Bones);
        //}
        AnimationController.SetAnimator(_animator);
        AnimationController.SetTrasnferer(Transferer);
    }

    public void SetAnimationState(int animationState)
    {
        _animator.SetInteger("State", animationState);
    }

    public void AddActionPoints(int actionPoints)
    {
        ActionPoints += actionPoints;
        Events.OnActionPointsUpdated(ActionPoints);
    }

    public void ResetActionPoints()
    {
        ActionPoints = 0;
        Events.OnActionPointsUpdated(ActionPoints);
    }
}

