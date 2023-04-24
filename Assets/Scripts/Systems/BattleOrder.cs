using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class BattleOrder : MonoBehaviour
    {
        public bool IsExecuting;

        [SerializeField] protected List<DamageSettings> _damageSettings;

        protected Coroutine _excute;
        protected IMonster _caster;
        protected List<IMonster> _targets;

        public virtual void SetCaster(IMonster caster)
        {
            _caster = caster;
        }

        public virtual void SetTargets(List<IMonster> targets)
        {
            _targets = targets;
        }

        public virtual void Execute()
        {
            //starts the coroutine that will control this order
            IsExecuting = true;
        }

        public virtual void End()
        {
            //clear any stuff left
            IsExecuting = false;
        }
    }
}