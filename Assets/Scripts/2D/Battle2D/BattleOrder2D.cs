using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOrder2D : BattleEvent
{
    [System.Serializable]
    public class VFXData
    {
        public ParticleSystem Particle;
        public AnimationFixedPoints PointTag;
        public bool FollowAnimationPoint;
    }

    public bool IsExecuting;

    [SerializeField] protected List<Monster.DamageSettings> _damageSettings;

    protected Coroutine _excute;

    public override void Resolve()
    {
        base.Resolve();
        Execute();
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
        Resolved = true;
    }
}
