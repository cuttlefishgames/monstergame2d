using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvent : MonoBehaviour
{
    public Entity2D Caster { get; protected set; }
    public List<Entity2D> Targets { get; protected set; }
    public bool Resolved { get; protected set; }

    public virtual void SetCaster (Entity2D caster) { Caster = caster; }
    public virtual void SetTargets(List<Entity2D> targets) { Targets = targets; }
    public virtual bool Prepare () { return true; }
    public virtual void Resolve () { }
    public virtual void Discard () { gameObject.SetActive(false); Destroy(gameObject); }
    public virtual void Dispose () { }
}
