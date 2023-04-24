using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.Spawner
{
    [System.Serializable]
    public class MonsterSpawnData
    {
        public GameObject monsterPrefab;

        [Range(1, 100)] public int probability = 50;
        [Range(1, 99)] public int minLevel = 1;
        [Range(1, 100)] public int maxLevel = 2;

        [Header("The real chance of this monster being spawned")]
        public int chance;
    }

    [CreateAssetMenu(fileName = "MonsterAreaSettings", menuName = "Monster Area/Monsters Area Settings")]
    public class MonsterAreaSettings : ScriptableObject
    {
        [SerializeField] [Range(1, 1000)] private int _minSpawnInterval = 60;
        [SerializeField] [Range(1, 1000)] private int _maxSpawnInterval = 60;
        [SerializeField] private List<MonsterSpawnData> _monstersData = new List<MonsterSpawnData>();

        public int MinSpawnInterval => _minSpawnInterval;
        public int MaxSpawnInterval => _maxSpawnInterval;
        public List<MonsterSpawnData> MonstersData => _monstersData;

        private void OnValidate()
        {
            if (_maxSpawnInterval < _minSpawnInterval)
            {
                _maxSpawnInterval = _minSpawnInterval;
            }

            int totalProbability = 0;
            foreach (var monsterData in _monstersData)
            {
                totalProbability += monsterData.probability;
                if (monsterData.maxLevel < monsterData.minLevel)
                {
                    monsterData.maxLevel = monsterData.minLevel;
                }
            }

            foreach (var monsterData in _monstersData)
            {
                monsterData.chance = Mathf.CeilToInt(((float)monsterData.probability / (float)totalProbability) * 100f);
            }
        }
    }
}