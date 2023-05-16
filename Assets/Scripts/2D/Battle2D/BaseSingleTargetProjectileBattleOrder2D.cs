using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseSingleTargetProjectileBattleOrder2D : BattleOrder2D
{
    [Header("Delays before attack start, delay after attack animation, attack animation duration")]
    [SerializeField] [Range(0f, 5f)] private float _delayBeforeCast = 0f;
    [SerializeField] [Range(0f, 5f)] private float _delayBeforeProjectile = 0f;
    [SerializeField] [Range(0f, 20f)] private float _projectileSpeed = 5f;

    [SerializeField] private bool _useUserCast = true;
    [SerializeField] private bool _useProjectileCast = true;
    [SerializeField] private bool _useProjectile = true;
    [SerializeField] private bool _useTargetAura = true;
    [SerializeField] private bool _useImpact = true;

    [Header("Cast particle times. Only happens if charge particle is not null")]
    [SerializeField] [Range(0f, 5f)] private float _castDelayBeforeAnimation = 0f;
    [SerializeField] [Range(0f, 5f)] private float _castDuration = 1f;
    [SerializeField] [Range(0f, 5f)] private float _releaseDuration = 0f;
    [SerializeField] [Range(0f, 5f)] private float _afterCastDelay = 0f;

    [Header("Projectile cast times. Only happens if the projectile cast particle is not null")]
    [SerializeField] [Range(0f, 5f)] private float _projectileCastSpawnDelay = 0f;

    [Header("Projectile times. Only happens if the projectile particle is not null")]
    [SerializeField] private Ease _projectileEase = Ease.Linear;

    [Header("Target aura times. Only happens if the target aura particle is not null")]
    [SerializeField] [Range(0f, 5f)] private float _targetAuraDuration = 0f;

    [Header("If checked, the caster will move to position")]
    [SerializeField] private bool _hopToPosition = true;
    [SerializeField] private HopToPositionsTags _hopToPositionsTag;
    [SerializeField] [Range(0f, 5f)] private float _hopDuration = 0.5f;

    [Header("If checked, the target will be knocked back when the hit happens")]
    [SerializeField] private bool _knocksbackTarget = true;
    [SerializeField] [Range(0f, 5f)] private float _knockBackDistance = 3f;
    [SerializeField] [Range(0f, 5f)] private float _knockBackDuration = 0.5f;
    [SerializeField] [Range(0f, 5f)] private float _knockBackDelay = 0.0f;
    [SerializeField] [Range(0f, 5f)] private float _knockBackReturnDuration = 0.5f;

    [Header("Particles")]
    [SerializeField] private VFXData _userCastParticleData;
    [SerializeField] private VFXData _projectileCastParticleData;
    [SerializeField] private VFXData _projectileParticleData;
    [SerializeField] private AnimationFixedPoints _projectileSpawnPoint;
    [SerializeField] private VFXData _impactParticleData;
    [SerializeField] private VFXData _targetAuraParticleData;

    protected Coroutine _projectile;
    private bool _partAExecuted;
    private bool _partBExecuted;

    public override void Execute()
    {
        if (IsExecuting)
            return;

        if (_excute != null)
            StopCoroutine(_excute);

        _excute = StartCoroutine(ExecuteCoroutine());

        if (_projectile != null)
            StopCoroutine(_projectile);

        _projectile = StartCoroutine(ProjectileCoroutine());
    }

    IEnumerator ProjectileCoroutine()
    {
        var target = _targets[0];

        //short delay before the whole thing starts
        if (_delayBeforeProjectile > 0)
        {
            yield return new WaitForSeconds(_delayBeforeProjectile);
        }

        var castPoint = _caster.AnimationController.GetAnimationFixedPoint(_projectileSpawnPoint);
        if (_useProjectileCast && _projectileCastParticleData.Particle != null)
        {
            if (_projectileCastParticleData.FollowAnimationPoint)
            {
                _projectileCastParticleData.Particle.transform.SetParent(castPoint);
                _projectileCastParticleData.Particle.transform.localScale = Vector3.one;
            }
            _projectileCastParticleData.Particle.transform.position = castPoint.position;

            if (_projectileCastSpawnDelay > 0)
            {
                yield return new WaitForSeconds(_projectileCastSpawnDelay);
            }
            _projectileCastParticleData.Particle.Play();
        }

        if (_useProjectile && _projectileParticleData.Particle != null)
        {
            if (_projectileParticleData.FollowAnimationPoint)
            {
                _projectileParticleData.Particle.transform.SetParent(castPoint);
                _projectileParticleData.Particle.transform.localScale = Vector3.one;
            }
            _projectileParticleData.Particle.transform.position = castPoint.position;

            if (_projectileCastSpawnDelay > 0)
            {
                yield return new WaitForSeconds(_projectileCastSpawnDelay);
            }
            _projectileParticleData.Particle.Play();

            //move projectile
            var targetPoint = target.AnimationController.GetAnimationFixedPoint(_projectileParticleData.PointTag);
            var projectileDuration = Vector2.Distance(new Vector2(castPoint.position.x, castPoint.position.y),
                new Vector2(targetPoint.position.x, targetPoint.position.y)) / _projectileSpeed;
            _projectileParticleData.Particle.gameObject.transform.DOMove(targetPoint.position, projectileDuration).SetEase(_projectileEase);

            if (projectileDuration > 0)
            {
                yield return new WaitForSeconds(projectileDuration);
            }
            _projectileParticleData.Particle.Stop();

        }

        var impactDuration = ImpactTarget();
        if (_useImpact && _impactParticleData.Particle != null)
        {
            var impactTargetPoint = target.AnimationController.GetAnimationFixedPoint(_impactParticleData.PointTag);
            if (_impactParticleData.FollowAnimationPoint)
            {
                _impactParticleData.Particle.transform.SetParent(impactTargetPoint);
                _impactParticleData.Particle.transform.localScale = Vector3.one;
            }
            _impactParticleData.Particle.transform.position = impactTargetPoint.position;
            _impactParticleData.Particle.Play();
        }

        if (_useTargetAura && _targetAuraParticleData.Particle != null)
        {
            var auraPoint = target.AnimationController.GetAnimationFixedPoint(_targetAuraParticleData.PointTag);
            if (_targetAuraParticleData.FollowAnimationPoint)
            {
                _targetAuraParticleData.Particle.transform.SetParent(auraPoint);
                _targetAuraParticleData.Particle.transform.localScale = Vector3.one;
            }
            _targetAuraParticleData.Particle.transform.position = auraPoint.position;
            _targetAuraParticleData.Particle.Play();

            impactDuration += _targetAuraDuration;
        }

        if (impactDuration > 0)
        {
            yield return new WaitForSeconds(impactDuration);
        }

        if (_projectileCastParticleData.Particle != null)
        {
            _projectileCastParticleData.Particle.transform.SetParent(transform);
        }
        if (_projectileParticleData.Particle != null)
        {
            _projectileParticleData.Particle.transform.SetParent(transform);
        }
        if (_impactParticleData.Particle != null)
        {
            _impactParticleData.Particle.transform.SetParent(transform);
        }
        if (_targetAuraParticleData.Particle != null)
        {
            _targetAuraParticleData.Particle.transform.SetParent(transform);
        }

        _partBExecuted = true;
        IsExecuting = !(_partAExecuted && _partBExecuted);
    }

    private float ImpactTarget()
    {
        var target = _targets[0];
        var targetOriginalPos = Battlefield2D.GetCharacterSlotByEntity(target);
        var direction = Vector3.Normalize(new Vector3(target.Root.position.x - _caster.Root.position.x, 0, 0));

        //play target's pain animation
        target.ScalingTarget.DOShakePosition(0.3f);
        target.AnimationController.SetAnimation("Pain");

        //move target due to impact
        if (_knocksbackTarget)
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(_knockBackDelay);
            sequence.Append(target.Root.transform.DOMove(targetOriginalPos.Transform.position + (direction * _knockBackDistance), _knockBackDuration).SetEase(Ease.OutExpo));
            sequence.Append(target.Root.transform.DOMove(targetOriginalPos.Transform.position, _knockBackReturnDuration).SetEase(Ease.InExpo));
            sequence.OnComplete(() => { target.AnimationController.SetAnimation("Idle"); });
        }

        return _knockBackDelay + _knockBackDuration + _knockBackReturnDuration;
    }

    IEnumerator ExecuteCoroutine()
    {
        IsExecuting = true;

        var target = _targets[0];
        var targetOriginalPos = Battlefield2D.GetCharacterSlotByEntity(target);
        var jumpPos = _hopToPosition ? Battlefield2D.HopToPositionToFieldPosition(_hopToPositionsTag, _caster, target) : Battlefield2D.HopToPositionToFieldPosition(HopToPositionsTags.STAY_IN_PLACE, _caster, target);
        var casterAnimatorController = _caster.AnimationController;
        var casterOriginalPos = Battlefield2D.GetCharacterSlotByEntity(_caster);
        var middlePos = Battlefield2D.Positions[BattlefieldPositions.MIDDLE];
        var direction = Vector3.Normalize(new Vector3(target.Root.position.x - _caster.Root.position.x, 0, 0));

        //place vfx
        //cast
        if (_userCastParticleData != null)
        {
            if (_userCastParticleData.FollowAnimationPoint)
            {
                _userCastParticleData.Particle.transform.SetParent(_caster.Root);
                _userCastParticleData.Particle.transform.localScale = Vector3.one;
            }

            _userCastParticleData.Particle.transform.position =
                _caster.AnimationController.GetAnimationFixedPoint(_userCastParticleData.PointTag).position;
        }

        //short delay before the whole thing starts
        if (_delayBeforeCast > 0)
        {
            yield return new WaitForSeconds(_delayBeforeCast);
        }

        if (_hopToPosition)
        {
            //make caster jump the attack position
            if (_hopToPositionsTag != HopToPositionsTags.STAY_IN_PLACE)
            {
                //_caster.ScalingTarget.DOMove(jumpPos.position, _hopDuration).SetEase(Ease.Linear);
                _caster.Root.DOMove(jumpPos.position, _hopDuration).SetEase(Ease.Linear);
                _caster.AnimationController.SetAnimation("HopIn");
                yield return new WaitForSeconds(_hopDuration);
            }
        }

        //cast vfx
        if (_useUserCast && _userCastParticleData.Particle != null)
        {
            _userCastParticleData.Particle.Play();

            if (_castDelayBeforeAnimation > 0)
            {
                yield return new WaitForSeconds(_castDelayBeforeAnimation);
            }

            //set cast animation
            _caster.AnimationController.SetAnimation("Cast");

            if (_castDuration > 0)
            {
                yield return new WaitForSeconds(_castDuration);
            }
        }

        //set caster to idle again after hit
        _caster.AnimationController.SetAnimation("CastRelease");
        if (_releaseDuration > 0)
        {
            yield return new WaitForSeconds(_releaseDuration);
        }

        _caster.AnimationController.SetAnimation("Idle");

        if (_afterCastDelay > 0)
        {
            yield return new WaitForSeconds(_afterCastDelay);
        }

        //return caster to oritinal position
        _caster.Root.DOMove(casterOriginalPos.Transform.position, _hopDuration).SetEase(Ease.Linear);

        if (_userCastParticleData.Particle != null)
        {
            _userCastParticleData.Particle.transform.SetParent(transform);
        }

        _partAExecuted = true;
        IsExecuting = !(_partAExecuted && _partBExecuted);
    }
}
