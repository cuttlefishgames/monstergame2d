using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class BattleOrdersQueueManager : MonoBehaviour
    {
        private Queue<BattleOrder> _battleOrders;

        private void Awake()
        {
            _battleOrders = new Queue<BattleOrder>();
        }

        private void Update()
        {
            
        }

        public void EnqueueOrder(BattleOrder order)
        {
            _battleOrders.Enqueue(order);
        }
    }
}