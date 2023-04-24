using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Monster
{
    [CreateAssetMenu(fileName = "MonsterStatsData", menuName = "Stats/Monster Stats Data")]
    public class MonsterStatsData : ScriptableObject
    {
        [SerializeField] private int _constitution;
        [SerializeField] private int _attack;
        [SerializeField] private int _magic;
        [SerializeField] private int _speed;
        [SerializeField] private int _defense;
        [SerializeField] private int _resistance;
        [SerializeField] private ExpNeededCategories _expNeededCategory = ExpNeededCategories.AVERAGE;
        [SerializeField] [Range(1, 1000)] private int _baseExp = 100;

        public int Constitution { get => _constitution; }
        public int Attack { get => _attack; }
        public int Magic { get => _magic; }
        public int Speed { get => _speed; }
        public int Defense { get => _defense; }
        public int Resistance { get => _resistance; }
        public int BaseExp { get => _baseExp; }
        public ExpNeededCategories ExpNeededCategory { get => _expNeededCategory; }
    }
}