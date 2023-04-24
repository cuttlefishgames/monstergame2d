using System.Collections;
using UnityEngine;

namespace Monster.BattleOrders
{
    public class BasicMeleeBattleOrder : BattleOrder
    {
        [SerializeField] private GameObject _VFX;

        private float _moveDuration;
        private float _castDuration;
        private float _hitDuration;

        public override void SetCaster(IMonster caster)
        {
            base.SetCaster(caster);

            //subscribe to events
        }

        public override void Execute()
        {
            _excute = StartCoroutine(Execution());
        }

        public override void End()
        {
            if(_excute != null)
            {
                StopCoroutine(_excute);
            }

            //unsubscribe to events
        }

        private void Prepare()
        {

        }

        private void Move()
        {

        }

        private void Cast()
        {

        }

        private void Hit()
        {

        }

        IEnumerator Execution()
        {
            yield return Wait.ForEndOfFrame;
        }
    }
}