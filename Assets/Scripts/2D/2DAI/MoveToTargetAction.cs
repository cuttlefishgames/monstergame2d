using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetAction : IAAction2D
{
    private Coroutine _moveToTarget;

    public override bool Viable()
    {
        _viable = true;

        if (_controller.Target == null)
            _viable = false;

        return _viable;
    }

    public override bool Interrupt()
    {
        StopMoveToTarget();
        return base.Interrupt();
    }

    public override void End()
    {
        StopMoveToTarget();
        base.End();
    }

    public override void Execute()
    {
        IsPerforming = true;
        StopMoveToTarget();
        _moveToTarget = StartCoroutine(MoveToTarget());
    }

    private void StopMoveToTarget()
    {
        if (_moveToTarget != null)
            StopCoroutine(_moveToTarget);
    }

    IEnumerator MoveToTarget()
    {
        Agent.SetDestination(_controller.Target.Position);
        Agent.isStopped = false;

        //needs 2 frames for the navmesh agent to update the new path
        yield return Wait.ForEndOfFrame;
        yield return Wait.ForEndOfFrame;

        while (!ReachedDestination && _controller.Target != null)
        {
            Agent.SetDestination(_controller.Target.Position);
            yield return Wait.ForEndOfFrame;
            yield return Wait.ForEndOfFrame;
            _controller.UpdateFacingDirectionToDestination();
            yield return Wait.ForEndOfFrame;
        }

        End();
    }
}