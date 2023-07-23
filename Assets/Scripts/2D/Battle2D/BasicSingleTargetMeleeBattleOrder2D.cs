using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BasicSingleTargetMeleeBattleOrder2D : BattleOrder2D
{
    [Header("Delays before attack start, delay after attack animation, attack animation duration")]
    [SerializeField] [Range(0f, 5f)] private float _delayBefore = 0f;
    [SerializeField] [Range(0f, 5f)] private float _delayAfter = 0f;
    [SerializeField] [Range(0f, 5f)] private float _attackDuration = 1f;

    [SerializeField] private bool _useCast = true;
    [SerializeField] private bool _useAura = true;
    [SerializeField] private bool _useImpact = true;

    [Header("Cast particle times. Only happens if charge particle is not null")]
    [SerializeField] [Range(0f, 5f)] private float _castDelayBeforeAnimation = 0f;
    [SerializeField] [Range(0f, 5f)] private float _castDuration = 1f;
    [SerializeField] [Range(0f, 5f)] private float _afterCastDelay = 0f;
    [SerializeField] [Range(0f, 5f)] private float _afterAuraDelay = 0f;

    [Header("If checked, the caster will move to position")]
    [SerializeField] private bool _hopToPosition = true;
    [SerializeField] private HopToPositionsTags _hopToPositionsTag;
    [SerializeField] [Range(0f, 5f)] private float _hopDuration = 0.5f;

    [Header("If checked, the caster will lunge towards the target during the melee attack animation")]
    [SerializeField] private bool _thrustTowardsTheTarget = true;
    [SerializeField] [Range(0f, 5f)] private float _thrustBackDistance = 0.3f;
    [SerializeField] [Range(0f, 5f)] private float _thrustBuildUp = 0.3f;
    [SerializeField] [Range(0f, 5f)] private float _thrustDuration = 0.3f;
    [SerializeField] [Range(0f, 5f)] private float _thrustReturnDuration = 0.2f;
    [SerializeField] private Ease _thurstEase = Ease.InElastic;

    [Header("If checked, the target will be knocked back when the hit happens")]
    [SerializeField] private bool _knocksbackTarget = true;
    [SerializeField] [Range(0f, 5f)] private float _knockBackDistance = 3f;
    [SerializeField] [Range(0f, 5f)] private float _knockBackDuration = 0.5f;
    [SerializeField] [Range(0f, 5f)] private float _knockBackDelay = 0.0f;
    [SerializeField] [Range(0f, 5f)] private float _knockBackReturnDuration = 0.5f;

    [Header("Particles")]
    [SerializeField] private VFXData _castParticleData;
    [SerializeField] private VFXData _auraParticleData;
    [SerializeField] private VFXData _impactParticleData;
    [SerializeField] private bool _disableAuraAfterHit = true;

    public override void Execute()
    {
        if (IsExecuting)
            return;

        if (_excute != null)
            StopCoroutine(_excute);

        _excute = StartCoroutine(ExecuteCoroutine());
    }

    IEnumerator ExecuteCoroutine()
    {
        IsExecuting = true;
        
        var target = Targets[0];
        var targetOriginalPos = Battlefield2D.GetCharacterSlotByEntity(target);
        var jumpPos = _hopToPosition ? Battlefield2D.HopToPositionToFieldPosition(_hopToPositionsTag, Caster, target) : Battlefield2D.HopToPositionToFieldPosition(HopToPositionsTags.STAY_IN_PLACE, Caster, target);
        var casterAnimatorController = Caster.AnimationController;
        var casterOriginalPos = Battlefield2D.GetCharacterSlotByEntity(Caster);
        var middlePos = Battlefield2D.Positions[BattlefieldPositions.MIDDLE];
        var direction = Vector3.Normalize(new Vector3(target.Root.position.x - Caster.Root.position.x, 0, 0));

        //place vfx
        //cast
        if (_castParticleData != null)
        {
            if (_castParticleData.FollowAnimationPoint)
            {
                _castParticleData.Particle.transform.SetParent(Caster.Root);
                _castParticleData.Particle.transform.localScale = Vector3.one;
            }

            _castParticleData.Particle.transform.position = 
                Caster.AnimationController.GetAnimationFixedPoint(_castParticleData.PointTag).position;
        }
        //aura
        if (_auraParticleData != null)
        {
            if (_auraParticleData.FollowAnimationPoint)
            {
                _auraParticleData.Particle.transform.SetParent(Caster.Root);
                _auraParticleData.Particle.transform.localScale = Vector3.one;
            }

            _auraParticleData.Particle.transform.position =
                Caster.AnimationController.GetAnimationFixedPoint(_auraParticleData.PointTag).position;
        }
        //impact
        if (_impactParticleData != null)
        {
            if (_impactParticleData.FollowAnimationPoint)
            {
                _impactParticleData.Particle.transform.SetParent(target.Root);
                _impactParticleData.Particle.transform.localScale = Vector3.one;
            }

            _impactParticleData.Particle.transform.position =
                target.AnimationController.GetAnimationFixedPoint(_impactParticleData.PointTag).position;
        }

        //short delay before the whole thing starts
        if (_delayBefore > 0)
        {
            yield return new WaitForSeconds(_delayBefore);
        }

        //cast vfx
        if (_useCast && _castParticleData.Particle != null)
        {
            _castParticleData.Particle.Play();

            if (_castDelayBeforeAnimation > 0)
            {
                yield return new WaitForSeconds(_castDelayBeforeAnimation);
            }

            //set cast animation
            //_caster.AnimationController.SetAnimationState(0);

            if (_castDuration > 0)
            {
                yield return new WaitForSeconds(_castDuration);
            }

            if (_afterCastDelay > 0)
            {
                Caster.AnimationController.SetAnimationState(AnimationStates.IDLE);
                yield return new WaitForSeconds(_castDuration);
            }
        }

        if(_useAura && _auraParticleData.Particle != null)
        {
            _auraParticleData.Particle.Play();

            if (_afterAuraDelay > 0)
            {
                Caster.AnimationController.SetAnimationState(AnimationStates.IDLE);
                yield return new WaitForSeconds(_castDuration);
            }
        }                

        if (_hopToPosition)
        {
            //make caster jump the attack position
            if(_hopToPositionsTag != HopToPositionsTags.STAY_IN_PLACE)
            {
                //_caster.ScalingTarget.DOMove(jumpPos.position, _hopDuration).SetEase(Ease.Linear);
                Caster.Root.DOMove(jumpPos.position, _hopDuration).SetEase(Ease.Linear);
                //_caster.AnimationController.SetAnimation("HopIn");
                yield return new WaitForSeconds(_hopDuration);
            }
        }

        //melee animation
        //_caster.AnimationController.SetAnimation("MeleeAttack");

        float thrustWait = 0;
        if (_thrustTowardsTheTarget)
        {
            thrustWait = _thrustReturnDuration;

            //animate caster
            var sequence = DOTween.Sequence();
            //sequence.Append(_caster.ScalingTarget.DOMove(jumpPos.position + direction * -1 * _thrustBackDistance, _thrustBuildUp).SetEase(_thurstEase));
            //sequence.Append(_caster.ScalingTarget.DOMove(targetOriginalPos.Transform.position, _thrustDuration));
            sequence.Append(Caster.Root.DOMove(jumpPos.position + direction * -1 * _thrustBackDistance, _thrustBuildUp).SetEase(_thurstEase));
            sequence.Append(Caster.Root.DOMove(targetOriginalPos.Transform.position, _thrustDuration));
            sequence.AppendInterval(_knockBackDelay);
            //sequence.Append(_caster.ScalingTarget.DOMove(casterOriginalPos.Transform.position, _thrustReturnDuration).SetEase(Ease.Linear));
            sequence.Append(Caster.Root.DOMove(casterOriginalPos.Transform.position, _thrustReturnDuration).SetEase(Ease.Linear));
            sequence.OnComplete(() => { Caster.AnimationController.SetAnimationState(AnimationStates.IDLE); });

            yield return new WaitForSeconds(_thrustBuildUp + _thrustDuration + _knockBackDelay);
        }
        else
        {
            //wait for the build up
            yield return new WaitForSeconds(_attackDuration);
        }

        //set caster to idle again after hit
        Caster.AnimationController.SetAnimationState(AnimationStates.IDLE);

        //impact vfx
        if (_useImpact && _impactParticleData.Particle != null)
        {
            _impactParticleData.Particle.Play();
        }

        if(_disableAuraAfterHit && _auraParticleData.Particle != null)
        {
            _auraParticleData.Particle.Stop();
        }

        //play target's pain animation
        target.ScalingTarget.DOShakePosition(0.3f);
        target.AnimationController.SetAnimationState(AnimationStates.FREEZE);

        //move target due to impact
        if (_knocksbackTarget)
        {
            thrustWait = Mathf.Max(_knockBackDuration + _knockBackReturnDuration + _knockBackDelay, thrustWait);
            var sequence = DOTween.Sequence();
            //sequence.Append(target.ScalingTarget.transform.DOMove(targetOriginalPos.Transform.position + (direction * _knockBackDistance), _knockBackDuration).SetEase(Ease.OutExpo));
            //sequence.Append(target.ScalingTarget.transform.DOMove(targetOriginalPos.Transform.position, _knockBackReturnDuration).SetEase(Ease.InExpo));
            sequence.Append(target.Root.transform.DOMove(targetOriginalPos.Transform.position + (direction * _knockBackDistance), _knockBackDuration).SetEase(Ease.OutExpo));
            sequence.Append(target.Root.transform.DOMove(targetOriginalPos.Transform.position, _knockBackReturnDuration).SetEase(Ease.InExpo));
            sequence.OnComplete(() => { target.AnimationController.SetAnimationState(AnimationStates.IDLE); });
        }

        //delay after attack
        if (thrustWait > 0)
        {
            yield return new WaitForSeconds(thrustWait);
        }        

        //wait for any particles effects to end
        if(_delayAfter > 0)
        {
            yield return new WaitForSeconds(_delayAfter);
        }

        if(_castParticleData.Particle != null)
        {
            _castParticleData.Particle.transform.SetParent(transform);
        }
        if (_auraParticleData.Particle != null)
        {
            _auraParticleData.Particle.transform.SetParent(transform);
        }
        if (_impactParticleData.Particle != null)
        {
            _impactParticleData.Particle.transform.SetParent(transform);
        }

        End();
    }
}
