using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster {
    public class OrderTester : MonoBehaviour
    {
        [SerializeField] private GameObject _orderPrefab;
        [SerializeField] private MonsterEntity _caster;
        [SerializeField] private MonsterEntity _target;

        private BattleOrder _order;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space) && _order == null)
            {
                var orderObject = Instantiate(_orderPrefab);
                _order = orderObject.GetComponent<BattleOrder>();
                _order.SetCaster(_caster);
                _order.SetTargets(new List<IMonster> { _target });
                _order.Execute();
            }

            if(_order != null)
            {
                if (!_order.IsExecuting)
                {
                    Destroy(_order.gameObject);
                    _order = null;
                }
            }
        }
    }
}
