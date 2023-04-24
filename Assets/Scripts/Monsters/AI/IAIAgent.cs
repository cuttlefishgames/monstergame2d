using UnityEngine;
using UnityEngine.AI;

namespace Monster.AI
{
    public enum AIStates { IDLING = 0, ROAMING = 100, CHASING = 200, RESETING = 300, RESETING_TO_ROAM = 400, FOLLOWING = 500 }

    public interface IAIAgent
    {
        NavMeshAgent GetNavMeshAgent();
        bool HasReachedDestination();
        bool HasReachedTarget();
        void SetState(AIStates state);
        void SetPivot(Vector3 pivot);
        void SetRoamingRadius(int radius);
        void SetDestination(Vector3 destination);
        void Roam();
        void Stop();
        void Follow(Transform target);
        void Chase(Transform target);
        void ResetAI();
        void ResetThenRoam();
        Vector3 GetRandomDestination();
        void SetAIActiveAState(bool AIState);
        void SetIdlingRange(float minTime, float maxTime);
    }
}