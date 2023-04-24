using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class StatsTester : MonoBehaviour
    {
        [SerializeField] private MonsterStatsData _stats;
        [SerializeField] [Range(1, 100)] private int _level = 1;

        private MonsterStats _updatedStats;

        [Header("Base Stats")]
        [SerializeField] private int _constitution;
        [SerializeField] private int _baseAttack;
        [SerializeField] private int _baseMagic;
        [SerializeField] private int _baseSpeed;
        [SerializeField] private int _baseDefense;
        [SerializeField] private int _baseResistance;
        [Header("Final Stats")]
        [SerializeField] private int _HP;
        [SerializeField] private int _attack;
        [SerializeField] private int _magic;
        [SerializeField] private int _speed;
        [SerializeField] private int _defense;
        [SerializeField] private int _resistance;
        [Header("Armors")]
        [SerializeField] private float _physicalArmor;
        [SerializeField] private float _magicalArmor;
        [Header("Effective Health")]
        [SerializeField] private int _physicalEffectiveHealth;
        [SerializeField] private float _physicalHealthIncrease;
        [SerializeField] private int _magicalEffectiveHealth;
        [SerializeField] private float _magicalHealthIncrease;
        [Header("Experience")]
        [SerializeField] private int _expNeededForNextLevel;
        [SerializeField] private float _expGiven;

        [ExecuteInEditMode]
        private void Start()
        {
            if(_stats != null)
            {
                _constitution = _stats.Constitution;
                _baseAttack = _stats.Attack;
                _baseMagic = _stats.Magic;
                _baseSpeed = _stats.Speed;
                _baseDefense = _stats.Defense;
                _baseResistance = _stats.Resistance;
            }
        }

        private void Update()
        {
            CalculateStats();
        }

        public void CalculateStats()
        {
            _level = Mathf.Min(_level, GameManager.MaxLevel);
            _updatedStats = StatsManager.UpdateStats(_stats, _level);
            if(_updatedStats != null)
            {
                _HP = _updatedStats.HP;
                _attack = _updatedStats.Attack;
                _magic = _updatedStats.Magic;
                _speed = _updatedStats.Speed;
                _defense = _updatedStats.Defense;
                _resistance = _updatedStats.Resistance;

                var physArmor = StatsManager.CalcPhysicalArmor(_defense);
                var magiArmor = StatsManager.CalcPhysicalArmor(_resistance);

                _physicalArmor = 100f * (1f - physArmor);
                _magicalArmor = 100f * (1f - magiArmor);

                _physicalEffectiveHealth = StatsManager.CalcEffectivePhysicalHP(physArmor, _HP);
                _physicalHealthIncrease = _physicalEffectiveHealth / (float)_HP;
                _magicalEffectiveHealth = StatsManager.CalcEffectivePhysicalHP(magiArmor, _HP);
                _magicalHealthIncrease = _magicalEffectiveHealth / (float)_HP;

                _expNeededForNextLevel = GameManager.GetExperienceNeeded(_stats.ExpNeededCategory, _level);
                _expGiven = GameManager.GetExperienceGivenScaled(_level, _stats.BaseExp);        
            }
        }
    }
}