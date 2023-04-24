using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Monster
{
    public class MeleeBaseSingleAttackOrder : BattleOrder
    {
        [Header("Delays before attack and/or charge and delay after attack animation, attack animation duration")]
        [SerializeField] [Range(0f, 5f)] private float _delayBefore = 0.25f;
        [SerializeField] [Range(0f, 5f)] private float _delayAfter = 0.25f;
        [SerializeField] [Range(0f, 5f)] private float _attackDuration = 0.6f;

        [Header("If checked, the caster will charge before the attack")]
        [SerializeField] private bool _charge = false;
        [SerializeField] [Range(0f, 5f)] private float _chargeDuration = 1f;
        [SerializeField] [Range(0f, 5f)] private float _chargeLoopDuration = 0f;
        [SerializeField] [Range(0f, 5f)] private float _afterChargeDelay = 0f;

        [Header("If checked, the caster will move to position")]
        [SerializeField] private bool _hopToPosition = true;
        [SerializeField] [Range(0f, 5f)] private float _hopDuration = 1f;

        [Header("If checked, the caster will lunge towards the target during the melee attack animation")]
        [SerializeField] private bool _thrustTowardsTheTarget = true;
        [SerializeField] [Range(0f, 5f)] private float _thrustBuildUp = 0.3f;
        [SerializeField] [Range(0f, 5f)] private float _thrustDuration = 0.3f;

        [Header("If checked, the target will be knocked back when the hit happens")]
        [SerializeField] private bool _knocksbackTarget = true;
        [SerializeField] [Range(0f, 5f)] private float _knockBackDistance = 3f;

        [Header("Particles: 0 is for charge, 1 is the trail, 2 is the impact. Leave the field null if the order doesn't have one of the vfx. The positions data must have the same length of the vfx list.")]
        [SerializeField] private List<ParticleSystem> _particles;
        [SerializeField] private List<BattlePositionsManager.PointAnimationData> _particlesPositionData;

        private void OnValidate()
        {
            if(_particles == null)
            {
                _particles = new List<ParticleSystem>();
            }
            if(_particles.Count < 3)
            {
                while (_particles.Count < 3)
                    _particles.Add(null);
            }

            if(_particlesPositionData == null)
            {
                _particlesPositionData = new List<BattlePositionsManager.PointAnimationData>();
            }
            if (_particlesPositionData.Count < 3)
            {
                while (_particlesPositionData.Count < 3)
                    _particlesPositionData.Add(null);
            }
        }

        private void Awake()
        {
            //_particles.ForEach(p => p.gameObject.SetActive(false));
        }

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

            var targetGameObject = _targets[0].GetGameObject();
            var targetAnimatorController = _targets[0].GetAnimationController();
            var targetPointsManager = _targets[0].GetPointsManager();
            var targetOriginalPos = targetPointsManager.FeetPoint.position;
            var middlePos = BattlePositionsManager.Instance.MiddlePosition;
            var jumpPos = new Vector3(middlePos.position.x, 0, targetOriginalPos.z);
            var attackPos = targetOriginalPos;
            var direction = Vector3.Normalize(targetOriginalPos - jumpPos);

            var casterGameObject = _caster.GetGameObject();
            var casterAnimatorController = _caster.GetAnimationController();
            var casterOriginalPos = _caster.GetPointsManager().FeetPoint.position;
            var casterPointsManager = _caster.GetPointsManager();

            //place vfx
            //charge
            if (_particles[0] != null && _charge)
            {
                if (_particlesPositionData[0] != null)
                {
                    if (_particlesPositionData[0].FollowPoint)
                        _particles[0].transform.SetParent(casterGameObject.transform);

                    _particles[0].transform.position = casterPointsManager.GetPoint(_particlesPositionData[0].Point).position;
                }
            }
            //user vfx
            if (_particles[1] != null)
            {
                if (_particlesPositionData[1] != null)
                {
                    if (_particlesPositionData[1].FollowPoint)
                        _particles[1].transform.SetParent(casterGameObject.transform);

                    _particles[1].transform.position = casterPointsManager.GetPoint(_particlesPositionData[1].Point).position;
                }
            }
            //impact vfx
            if (_particles[2] != null)
            {
                if (_particlesPositionData[2] != null)
                {
                    if (_particlesPositionData[2].FollowPoint)
                        _particles[2].transform.SetParent(targetGameObject.transform);

                    _particles[2].transform.position = targetPointsManager.GetPoint(_particlesPositionData[2].Point).position;
                }
            }

            //short delay before the whole thing starts
            yield return new WaitForSeconds(0.25f);

            //casterAnimatorController.SetState(MonsterAnimationStates.IDLE);

            //charge vfx
            if (_particles[0] != null && _charge)
            {                
                _particles[0].gameObject.SetActive(true);
                _particles[0].Play();
            }

            //delays before attack or charge animation
            yield return new WaitForSeconds(_delayBefore);

            if (_charge)
            {
                casterAnimatorController.SetState(MonsterAnimationStates.CAST_ATTACK);
                yield return new WaitForSeconds(_chargeDuration);

                casterAnimatorController.SetState(MonsterAnimationStates.CAST_ATTACK_LOOP);
                yield return new WaitForSeconds(_chargeLoopDuration);
            }

            if (_afterChargeDelay != 0)
            {
                casterAnimatorController.SetState(MonsterAnimationStates.IDLE);
                yield return new WaitForSeconds(_afterChargeDelay);
            }

            //user vfx
            if (_particles[1] != null)
            {                
                _particles[1].gameObject.SetActive(true);
                _particles[1].Play();
            }

            if (_hopToPosition)
            {
                //make caster jump to front of enemy
                casterGameObject.transform.DOMove(jumpPos, _hopDuration).SetEase(Ease.Linear);
                casterAnimatorController.SetState(MonsterAnimationStates.HOP_IN);
                //sequence.OnKill(() => sequence = null);

                //wait for caster to jump to position
                yield return new WaitForSeconds(_hopDuration);
            }

            //melee animation
            casterAnimatorController.SetState(MonsterAnimationStates.MELEE_ATTACK);

            if (_thrustTowardsTheTarget)
            {
                //wait for the build up
                yield return new WaitForSeconds(_thrustBuildUp);

                //animate caster
                casterGameObject.transform.DOMove(attackPos, _thrustDuration).SetEase(Ease.InQuint);

                yield return new WaitForSeconds(_thrustDuration);
            }
            else
            {
                //wait for the build up
                yield return new WaitForSeconds(_attackDuration);
            }

            //set caster to idle again after hit
            casterAnimatorController.SetState(MonsterAnimationStates.IDLE);

            //impact vfx
            if (_particles[2] != null)
            {
                _particles[2].gameObject.SetActive(true);
                _particles[2].Play();
            }

            //a little delay for the parcile to play
            //yield return new WaitForSeconds(0.05f);

            //play target's pain animation
            targetAnimatorController.SetState(MonsterAnimationStates.PAIN);

            //move target due to impact
            if (_knocksbackTarget)
            {
                var sequence = DOTween.Sequence();
                sequence.Append(targetGameObject.transform.DOMove(targetOriginalPos + (direction * _knockBackDistance), 0.5f).SetEase(Ease.OutExpo));
                sequence.Append(targetGameObject.transform.DOMove(targetOriginalPos, 0.5f).SetEase(Ease.InExpo));
            }

            //cal damage here
            var attackResult = BattleManager.ResolveAttack(_damageSettings[0], _caster, _targets[0]);

            //wait for the target impact then move caster to original position again
            yield return new WaitForSeconds(0.3f);
            //if (_particles[1] != null)
            //{
            //    _particles[1].gameObject.SetActive(false);
            //}

            if (_targets[0].GetHPStatus() == 0)
            {
                targetAnimatorController.SetState(MonsterAnimationStates.FAINT);
            }
            else
            {
                targetAnimatorController.SetState(MonsterAnimationStates.IDLE);
            }

            if (_thrustTowardsTheTarget && !_hopToPosition)
            {
                casterGameObject.transform.DOMove(casterOriginalPos, _hopDuration).SetEase(Ease.Linear);
                casterAnimatorController.SetState(MonsterAnimationStates.HOP_OUT);

                //wait for caster to return to original pos
                yield return new WaitForSeconds(_hopDuration);
                casterAnimatorController.SetState(MonsterAnimationStates.IDLE);
            }
            else if (_hopToPosition)
            {
                casterGameObject.transform.DOMove(casterOriginalPos, _hopDuration).SetEase(Ease.Linear);
                casterAnimatorController.SetState(MonsterAnimationStates.HOP_OUT);

                //wait for caster to return to original pos
                yield return new WaitForSeconds(_hopDuration);
                casterAnimatorController.SetState(MonsterAnimationStates.IDLE);
            }
            else
            {
                casterAnimatorController.SetState(MonsterAnimationStates.IDLE);
                yield return new WaitForSeconds(_hopDuration / 2f);
            }

            //delay after attack
            yield return new WaitForSeconds(_delayAfter);

            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i] != null)
                    _particles[i].transform.SetParent(transform);
            }

            IsExecuting = false;
        }
    }
}