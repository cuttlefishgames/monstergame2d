using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetAction : IAAction2D
{
    private Coroutine _followTarget;

    public override bool Viable()
    {
        _viable = true;

        if (_controller.Target == null)
            _viable = false;

        return _viable;
    }

    public override bool Interrupt()
    {
        StopFollowTarget();
        return base.Interrupt();
    }

    public override void End()
    {
        StopFollowTarget();
        base.End();
    }

    public override void Execute()
    {
        IsPerforming = true;
        StopFollowTarget();
        _followTarget = StartCoroutine(FollowTarget());
        base.Execute();
    }

    private void StopFollowTarget()
    {
        if (_followTarget != null)
            StopCoroutine(_followTarget);
    }

    IEnumerator FollowTarget()
    {
        Agent.SetDestination(_controller.Target.Position);
        Agent.isStopped = false;

        //needs 2 frames for the navmesh agent to update the new path
        yield return Wait.ForEndOfFrame;
        yield return Wait.ForEndOfFrame;

        while (true)
        {
            Agent.SetDestination(_controller.Target.Position);
            yield return Wait.ForEndOfFrame;
            yield return Wait.ForEndOfFrame;
            _controller.UpdateFacingDirectionToDestination();
            yield return Wait.ForEndOfFrame;
        }
    }
}
