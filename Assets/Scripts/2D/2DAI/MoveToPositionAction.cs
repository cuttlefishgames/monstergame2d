using System.Collections;
using UnityEngine;

public class MoveToPositionAction : IAAction2D
{
    private Vector2 _position;
    private Coroutine _moveToPosition;

    public void SetPosition(Vector2 position)
    {
        _position = position;
    }

    public override bool Interrupt()
    {
        StopMoving();
        return base.Interrupt();
    }

    public override void End()
    {
        StopMoving();
        base.End();
    }

    public override void Execute()
    {
        IsPerforming = true;
        StopMoving();
        _moveToPosition = StartCoroutine(MoveToPosition());
        base.Execute();
    }

    private void StopMoving()
    {
        if (_moveToPosition != null)
            StopCoroutine(_moveToPosition);
    }

    IEnumerator MoveToPosition()
    {
        Agent.SetDestination(_position);
        Agent.isStopped = false;

        //needs 2 frames for the navmesh agent to update the new path
        yield return Wait.ForEndOfFrame;
        yield return Wait.ForEndOfFrame;

        while (!ReachedDestination)
        {
            _controller.UpdateFacingDirectionToDestination();
            yield return Wait.ForEndOfFrame;
        }

        End();
    }
}
