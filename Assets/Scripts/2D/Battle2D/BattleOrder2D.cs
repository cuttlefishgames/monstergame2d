using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOrder2D : MonoBehaviour
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
    protected Entity2D _caster;
    protected List<Entity2D> _targets;

    public virtual void SetCaster(Entity2D caster)
    {
        _caster = caster;
    }

    public virtual void SetTargets(List<Entity2D> targets)
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
