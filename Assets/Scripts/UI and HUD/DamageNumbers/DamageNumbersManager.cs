using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.UI
{
    public class DamageNumbersManager : Utils.Singleton<DamageNumbersManager>
    {
        private static Dictionary<string, DamageNumberComponent> DamageNumberInstances 
        { 
            get => Instance._damageNumeberInstances;
            set => Instance._damageNumeberInstances = value;
        }
        private Dictionary<string, DamageNumberComponent> _damageNumeberInstances;

        private static GameObject DamageNumberPrefab { get => Instance._damageNumberPrefab; }
        [SerializeField] private GameObject _damageNumberPrefab;

        private static Queue<DamageNumberComponent> DamageNumbersPool { get => Instance._damageNumbersPool; }
        private Queue<DamageNumberComponent> _damageNumbersPool;

        protected override void Awake()
        {
            base.Awake();
            _damageNumeberInstances = new Dictionary<string, DamageNumberComponent>();
            _damageNumbersPool = new Queue<DamageNumberComponent>();
        }

        public class DamageNumberSettings
        {
            public IMonster mon;
            public AnimationPoints point = AnimationPoints.CENTER;
            public int damage;
            public DamageTypes damageType;
            public bool miss;
            public bool critical;
            public bool instanced;

        }

        public static void DisplayDamageNumber(DamageNumberSettings settings)
        {
            DamageNumberComponent damageNumberComponent;
            if (settings.instanced)
            {
                if (DamageNumberInstances.ContainsKey(settings.mon.GetGUID()))
                {
                    damageNumberComponent = DamageNumberInstances[settings.mon.GetGUID()];
                }
                else
                {
                    damageNumberComponent = GetNextFreeDamageNumberComponent();
                    DamageNumberInstances.Add(settings.mon.GetGUID(), damageNumberComponent);
                }

                damageNumberComponent.EndCoroutines();
                damageNumberComponent.SetInstanced(true);
                damageNumberComponent.Enable();
                damageNumberComponent.SetWorldPosition(settings.mon.GetPointsManager().GetPoint(settings.point));
                damageNumberComponent.ConcatDamage(settings.damage, settings.miss, settings.critical);
                return;
            }
            else
            {
                damageNumberComponent = GetNextFreeDamageNumberComponent();
            }

            damageNumberComponent.EndCoroutines();
            damageNumberComponent.SetInstanced(false);
            damageNumberComponent.Enable();
            damageNumberComponent.SetWorldPosition(settings.mon.GetPointsManager().GetPoint(settings.point));
            damageNumberComponent.SetDamage(settings.damage, settings.miss, settings.critical);
        }

        private static DamageNumberComponent GetNextFreeDamageNumberComponent()
        {
            if(DamageNumbersPool.Count == 0)
            {
                DamageNumbersPool.Enqueue(SpawnDamageNumberComponent());
            }

            return DamageNumbersPool.Dequeue();
        }

        private static DamageNumberComponent SpawnDamageNumberComponent()
        {
            var damageNumberGameObject = Instantiate(DamageNumberPrefab);
            var damageNumberComponent = damageNumberGameObject.GetComponent<DamageNumberComponent>();
            return damageNumberComponent;
        }

        public static void ReturnDamageNumberToPool(DamageNumberComponent damageNumberComponent)
        {
            DamageNumbersPool.Enqueue(damageNumberComponent);
            if (damageNumberComponent.Instanced)
            {
                damageNumberComponent.Clear();
            }
            damageNumberComponent.Disable();
        }
    }
}