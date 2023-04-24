using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class AnimatorFunctions : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if(_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
        }

        public void SetState(MonsterAnimationStates state)
        {
            _animator.SetInteger("State", (int)state);
        }

        public void CheckPainLoop()
        {
            if (!_animator.GetBool("PainLoop"))
                SetState(MonsterAnimationStates.IDLE);
        }
    }
}