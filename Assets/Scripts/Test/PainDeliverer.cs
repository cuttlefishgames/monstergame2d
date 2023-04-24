using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class PainDeliverer : MonoBehaviour
    {
        [SerializeField] private Animator _target;
        [SerializeField] private ParticleSystem _singleHit;
        [SerializeField] private ParticleSystem _continuousHit;

        private MonsterAnimationStates _state = MonsterAnimationStates.IDLE;
        private bool _painLoop = false;
        private bool _meleeLoop = false;
        private bool _castLoop = false;

        private void Update()
        {
            if (Input.GetKey(KeyCode.F1))
            {
                ResetLoops();
                _target.SetInteger("State", (int)MonsterAnimationStates.IDLE);
                _state = MonsterAnimationStates.IDLE;
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                ResetLoops();
                _target.SetInteger("State", (int)MonsterAnimationStates.PAIN);
                _state = MonsterAnimationStates.PAIN;
                _singleHit.Play();
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                var oldPainLoop = !_painLoop;
                ResetLoops();
                _painLoop = oldPainLoop;
                _target.SetBool("PainLoop", _painLoop);
                if (_painLoop)
                {
                    _target.SetInteger("State", (int)MonsterAnimationStates.PAIN);
                    _state = MonsterAnimationStates.PAIN;
                    _continuousHit.Play();
                }
                else
                {
                    _target.SetInteger("State", (int)MonsterAnimationStates.IDLE);
                    _state = MonsterAnimationStates.IDLE;
                    _continuousHit.Stop();
                }
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                ResetLoops();
                _target.SetInteger("State", (int)MonsterAnimationStates.HOP_IN);
                _state = MonsterAnimationStates.HOP_IN;
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                ResetLoops();
                _target.SetInteger("State", (int)MonsterAnimationStates.MELEE_ATTACK);
                _state = MonsterAnimationStates.MELEE_ATTACK;
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                ResetLoops();
                _target.SetInteger("State", (int)MonsterAnimationStates.CAST_ATTACK);
                _state = MonsterAnimationStates.CAST_ATTACK;
            }

            if (Input.GetKeyDown(KeyCode.F7))
            {
                ResetLoops();
                _target.SetInteger("State", (int)MonsterAnimationStates.MELEE_ATTACK);
                _state = MonsterAnimationStates.MELEE_ATTACK;
                _meleeLoop = true;
                _target.SetBool(MonsterAnimationController.MELEE_LOOP_PARAMETER, _meleeLoop);
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                ResetLoops();
                _target.SetInteger("State", (int)MonsterAnimationStates.CAST_ATTACK);
                _state = MonsterAnimationStates.CAST_ATTACK;
                _castLoop = true;
                _target.SetBool(MonsterAnimationController.CAST_LOOP_PARAMETER, _castLoop);
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                ResetLoops();
                _target.SetInteger("State", (int)MonsterAnimationStates.HOP_OUT);
                _state = MonsterAnimationStates.HOP_OUT;
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                ResetLoops();
                _target.SetInteger("State", (int)MonsterAnimationStates.FAINT);
                _state = MonsterAnimationStates.FAINT;
            }
        }

        private void ResetLoops()
        {
            _painLoop = false;
            _meleeLoop = false;
            _castLoop = false;
            _target.SetBool(MonsterAnimationController.PAIN_LOOP_PARAMETER, _painLoop);
            _target.SetBool(MonsterAnimationController.MELEE_LOOP_PARAMETER, _meleeLoop);
            _target.SetBool(MonsterAnimationController.CAST_LOOP_PARAMETER, _castLoop);
            _singleHit.Stop();
            _continuousHit.Stop();

        }
    }
}