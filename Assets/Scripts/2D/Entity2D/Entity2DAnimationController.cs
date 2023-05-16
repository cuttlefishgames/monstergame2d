using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity2DAnimationController : Entity2DComponent
{
    //[SerializeField] private Animator _animator;
    [Header("Use only one")]
    private Animator _animator;
    private TransformPropertiesTransfer _transferer;
    [Header("Animation Fixed Points, if null will return entity's Root")]
    [SerializeField] private Transform _over;
    [SerializeField] private Transform _face;
    [SerializeField] private Transform _front_bottom;
    [SerializeField] private Transform _front_face;
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _top;
    [SerializeField] private Transform _bottom;
    [SerializeField] private Transform _back;
    [SerializeField] private Transform _behind_bottom;
    [SerializeField] private Transform _behind_center;
    private Dictionary<AnimationFixedPoints, Transform> _animationFixedPoints;
    public bool Paused
    {
        get => _paused;
        set
        {
            _paused = value;
            if (_animator != null)
            {
                _animator.speed = _paused ? 0 : 1;
            }
        }
    }
    private bool _paused = false;

    public override void SetParentEntity(Entity2D parentEntity)
    {
        base.SetParentEntity(parentEntity);

        _animationFixedPoints = new Dictionary<AnimationFixedPoints, Transform>();
        _animationFixedPoints.Add(AnimationFixedPoints.OVER, _over != null ? _over : ParentEntity.Root);
        _animationFixedPoints.Add(AnimationFixedPoints.FACE, _face != null ? _face : ParentEntity.Root);
        _animationFixedPoints.Add(AnimationFixedPoints.FRONT_BOTTOM, _front_bottom != null ? _front_bottom : ParentEntity.Root);
        _animationFixedPoints.Add(AnimationFixedPoints.FRONT_FACE, _front_face != null ? _front_face : ParentEntity.Root);
        _animationFixedPoints.Add(AnimationFixedPoints.CENTER, _center != null ? _center : ParentEntity.Root);
        _animationFixedPoints.Add(AnimationFixedPoints.TOP, _top != null ? _top : ParentEntity.Root);
        _animationFixedPoints.Add(AnimationFixedPoints.BOTTOM, _bottom != null ? _bottom : ParentEntity.Root);
        _animationFixedPoints.Add(AnimationFixedPoints.BACK, _back != null ? _back : ParentEntity.Root);
        _animationFixedPoints.Add(AnimationFixedPoints.BEHIND_BOTTOM, _behind_bottom != null ? _behind_bottom : ParentEntity.Root);
        _animationFixedPoints.Add(AnimationFixedPoints.BEHIND_CENTER, _behind_center != null ? _behind_center : ParentEntity.Root);
    }

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void SetTrasnferer(TransformPropertiesTransfer trasferer)
    {
        _transferer = trasferer;
    }

    public Transform GetAnimationFixedPoint(AnimationFixedPoints pointTag)
    {
        if (_animationFixedPoints.ContainsKey(pointTag))
        {
            return _animationFixedPoints[pointTag];
        }

        return ParentEntity.Root;
    }

    private void LateUpdate()
    {
        if (!_paused)
        {
            _transferer.Transfer();
        }
    }

    private void OnValidate()
    {
        if (_animator != null)
        {
            _transferer = null;
        }

        if (_transferer != null)
        {
            _animator = null;
        }
    }

    public void SetAnimation(string animation, float transition = 0.25f)
    {
        if (_animator != null)
        {
            _animator.CrossFade(animation, transition);
        }
        //else if (_transferer != null)
        //{
        //    _transferer.Animator.CrossFade(animation, transition);
        //}
    }
}
