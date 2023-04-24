using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Monster.Spawner
{
    public class MonsterArea : MonoBehaviour
    {
        [SerializeField] private List<SpawnSpot> _spawnSpots;

        public MonsterAreaSettings AreaSettings { get => _areaSettings; }
        [SerializeField] private MonsterAreaSettings _areaSettings;

        private float _timeCounter;

        private void Awake()
        {
            
        }

        private void Update()
        {
            _timeCounter += Time.deltaTime;
            while(_timeCounter >= 1f)
            {
                _timeCounter -= 1f;
                foreach(var spot in _spawnSpots)
                {
                    if (spot.Free)
                    {
                        spot.IncreaseTimer(1);
                        if (spot.Ready)
                        {
                            //spawn at that spot
                            System.Random rand = new System.Random();
                            var monstersData = _areaSettings.MonstersData.OrderBy(d => d.probability).ToList();
                            var probabilityCounts = new List<int>();
                            for(int i = 0; i < monstersData.Count; i++)
                            {
                                for (int j = 0; j < monstersData[i].probability; j++)
                                {
                                    probabilityCounts.Add(i);
                                }
                            }
                            var monsterIndex = probabilityCounts[rand.Next(probabilityCounts.Count)];
                            var monsterData = monstersData[monsterIndex];
                            var monsterObject = Instantiate(monsterData.monsterPrefab);
                            var monsterLevel = rand.Next(monsterData.maxLevel - monsterData.minLevel) + monsterData.minLevel;
                            var imonster = monsterObject.GetComponent<IMonster>();
                            imonster.SetLevel(monsterLevel);
                            spot.SetMon(imonster);
                            spot.SetMaxTimer(rand.Next(spot.MaxTimer - spot.MaxTimer) + spot.MaxTimer);
                            spot.SetTimer(0);
                        }
                    }
                }
            }
        }
    }
}