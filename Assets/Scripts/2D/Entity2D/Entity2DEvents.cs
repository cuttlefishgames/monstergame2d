using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity2DEvents : Entity2DComponent
{
    public delegate void ActionPointsUpdatedEvent(int actionPoints);
    public ActionPointsUpdatedEvent ActionPointsUpdated;
    public void OnActionPointsUpdated(int actionPoints)
    {
        ActionPointsUpdated?.Invoke(actionPoints);
    }

    public delegate void HPUpdatedEvent(int currentHP, int maxHP, float normalizedHP);
    public HPUpdatedEvent HPUpdated;
    public void OnHPUpdated(int currentHP, int maxHP, float normalizedHP)
    {
        HPUpdated?.Invoke(currentHP, maxHP, normalizedHP);
    }

    public delegate void EnergyUpdatedEvent(int currentEN, int maxEN, float normalizedEN);
    public EnergyUpdatedEvent EnergyUpdated;
    public void OnEnergyUpdated(int currentEN, int maxEN, float normalizedEN)
    {
        EnergyUpdated?.Invoke(currentEN, maxEN, normalizedEN);
    }
}
