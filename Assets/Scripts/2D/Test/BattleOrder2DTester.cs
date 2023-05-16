using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleOrder2DTester : MonoBehaviour
{
    [SerializeField] private GameObject _order;
    [SerializeField] private List<AIController2D> _casters;
    [SerializeField] private List<AIController2D> _targets;
    private AIController2D _caster;
    private AIController2D _target;
    private BattleOrder2D _currentOrder;

    private void Update()
    {
        if (_currentOrder != null)
        {
            if (!_currentOrder.IsExecuting)
            {
                Destroy(_currentOrder.gameObject);
                _currentOrder = null;
            }
        }
    }

    public void StartOrder()
    {
        if (_currentOrder != null)
            return;

        var random = new System.Random();
        var caster = _casters[random.Next(_casters.Count)];
        var target = _targets[random.Next(_targets.Count)];

        var orderObject = Instantiate(_order);
        _currentOrder = orderObject.GetComponent<BattleOrder2D>();
        _currentOrder.SetCaster(caster.Entity);
        _currentOrder.SetTargets(new List<Entity2D> { target.Entity });
        _currentOrder.Execute();
    }
}
