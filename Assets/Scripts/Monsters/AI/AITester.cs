using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.AI
{
    public class AITester : MonoBehaviour
    {
        [SerializeField] private BaseAIOverworld _targetAI;
        [SerializeField] [Range(1, 100)] private int _radius = 10;
        [SerializeField] private Vector3 _destination;
        [SerializeField] private Transform _chaseTarget;

        private void Awake()
        {
            if(_targetAI != null)
            {
                SetPivot();
            }
        }

        public void SetPivot()
        {
            _targetAI.SetPivot(_targetAI.transform.position);
        }

        public void SetRoamingRadius()
        {
            _targetAI.SetRoamingRadius(_radius);
        }

        public void SetDestination()
        {
            _targetAI.SetDestination(_destination);
        }

        public void Roam()
        {
            _targetAI.Roam();
        }

        public void Stop()
        {
            _targetAI.Stop();
        }

        public void Follow()
        {
            _targetAI.Follow(_chaseTarget);
        }

        public void Chase()
        {
            _targetAI.Chase(_chaseTarget);
        }

        public void ResetAI()
        {
            _targetAI.ResetAI();
        }

        public void ResetThenRoam()
        {
            _targetAI.ResetThenRoam();
        }

        public void SetAIActiveState(bool AIState)
        {
            _targetAI.SetAIActiveAState(AIState);
        }
    }
}
