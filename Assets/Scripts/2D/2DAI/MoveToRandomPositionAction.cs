using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToRandomPositionAction : IAAction2D
{
    private Vector2 _position;
    private Coroutine _moveToRandomPoint;
    [SerializeField] private NavMeshQueryFilter _navMeshFilter;
    [SerializeField] private float _radius = 3;

    public override void SetAIController(AIController2D controller)
    {
        base.SetAIController(controller);
        _navMeshFilter.agentTypeID = Agent.agentTypeID;
    }

    public override bool Viable()
    {
        _viable = false;
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle * _radius;
            randomDirection += _controller.Pivot;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, _radius, 1);
            Vector2 finalPosition = hit.position;
            if (hit.hit)
            {
                _position = finalPosition;
                _viable = true;
                break;
            }
        }

        return _viable;
    }

    public override bool Interrupt()
    {
        StopMovingToRandomPoint();
        return base.Interrupt();
    }

    public override void End()
    {
        StopMovingToRandomPoint();
        Agent.isStopped = true;
        Debug.Log("ENDED");
        base.End();
    }

    public override void Execute()
    {
        IsPerforming = true;
        StopMovingToRandomPoint();
        _moveToRandomPoint = StartCoroutine(MoveToRandomPoint());
        base.Execute();
    }

    private void StopMovingToRandomPoint()
    {
        if (_moveToRandomPoint != null)
            StopCoroutine(_moveToRandomPoint);
    }

    IEnumerator MoveToRandomPoint()
    {
        Debug.Log("STARTED MOVE TO RANDOM POINT");

        Agent.SetDestination(_position);
        Agent.isStopped = false;

        //needs 2 frames for the navmesh agent to update the new path
        yield return Wait.ForEndOfFrame;
        yield return Wait.ForEndOfFrame;

        while (!ReachedDestination)
        {
            Agent.isStopped = false;
            Agent.SetDestination(_position);

            _controller.UpdateFacingDirectionToDestination();
            yield return Wait.ForEndOfFrame;
        }

        End();
    }
}
