using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Utils;
using Monster.UI;

namespace Monster
{
    public class BattleManager : Singleton<BattleManager>
    {
        private BattleOrdersQueueManager _battleOrdersQueueManager;

        public class AttackResult
        {
            public bool Hit;
            public bool Miss;
            public bool Critical;
            public int RawPower;
            public int FinalPower;
        }

        protected override void Awake()
        {
            base.Awake();
            _battleOrdersQueueManager = gameObject.AddComponent<BattleOrdersQueueManager>();
        }

        public static AttackResult ResolveAttack(DamageSettings damageSettings, IMonster attacker, IMonster defender)
        {
            AttackResult result = new AttackResult();

            bool miss = Mathf.RoundToInt(Time.timeSinceLevelLoad) % 3 == 0;
            result.Miss = miss;

            bool critical = !miss && (Mathf.RoundToInt(Time.timeSinceLevelLoad) % 2 == 0);
            result.Critical = critical;

            CalcDamage(damageSettings, attacker, defender, miss, critical, out result.RawPower, out result.FinalPower);

            DamageNumbersManager.DisplayDamageNumber(
                new DamageNumbersManager.DamageNumberSettings
                {
                    mon = defender,
                    point = AnimationPoints.UPPER_CENTER,
                    damage = result.FinalPower,
                    damageType = DamageTypes.PHYSICAL,
                    miss = miss,
                    critical = critical,
                    instanced = damageSettings.MultiHits
                });
            defender.ReduceHP(result.FinalPower);

            return result;
        }

        public static void CalcDamage(DamageSettings damageSettings, IMonster attacker, IMonster defender, bool miss, bool critical, out int rawPower, out int finalPower)
        {
            var attackerStats = StatsManager.UpdateStats(attacker.GetMonsterStats(), attacker.GetLevel());
            var defenderStats = StatsManager.UpdateStats(defender.GetMonsterStats(), defender.GetLevel());

            var rawDamage = GetRawPowerFromStat(damageSettings.OffensiveStat, attackerStats, damageSettings.MultiplierPerHit);

            if (miss)
            {
                rawDamage = Mathf.CeilToInt((float)rawDamage * 0.33f);
            }
            else if (critical)
            {
                rawDamage = Mathf.CeilToInt((float)rawDamage * 1.5f);
            }

            float armor = 0;
            switch (damageSettings.DamageType)
            {
                case DamageTypes.PHYSICAL:
                    armor = StatsManager.CalcPhysicalArmor(defenderStats.Defense);
                    break;
                case DamageTypes.MAGICAL:
                    armor = StatsManager.CalcPhysicalArmor(defenderStats.Resistance);
                    break;
            }

            var damage = Mathf.CeilToInt(rawDamage * armor);

            var damageAfterElementalInteraction = Mathf.CeilToInt(damage *
                ElementChart.GetEffectiveness(attacker.GetElement(), defender.GetElement()));

            //Debug.Log("Armor " + armor + " | Raw damage " + rawDamage + " | Damage " + damage + " | Damage after elemental interaction " + damageAfterElementalInteraction);

            rawPower = rawDamage;
            finalPower = damageAfterElementalInteraction;
        }

        public static int GetRawPowerFromStat(Stats stat, MonsterStats baseStats, float multiplier = 100)
        {
            int rawPower = 0;
            float rawPowerFloat = 0;

            switch (stat)
            {
                default:
                    rawPowerFloat = baseStats.Attack * (multiplier / 100f);
                    break;
                case Stats.ATTACK:
                    rawPowerFloat = baseStats.Attack * (multiplier / 100f);
                    break;
                case Stats.MAGIC:
                    rawPowerFloat = baseStats.Magic * (multiplier / 100f);
                    break;
                case Stats.SPEED:
                    rawPowerFloat = baseStats.Speed * (multiplier / 100f);
                    break;
                case Stats.HP:
                    rawPowerFloat = baseStats.HP * (multiplier / 100f);
                    break;
                case Stats.DEFENSE:
                    rawPowerFloat = baseStats.Defense * (multiplier / 100f);
                    break;
                case Stats.RESISTANCE:
                    rawPowerFloat = baseStats.Resistance * (multiplier / 100f);
                    break;
            }

            rawPower = Mathf.FloorToInt(rawPowerFloat);
            return rawPower;
        }
    }
}