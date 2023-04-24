using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

namespace Monster
{
    public class MonsterInventoryManager : Utils.Singleton<MonsterInventoryManager>
    {
        private static readonly string PATH = "MonsterInventory.json";

        [Serializable]
        public class MonsterData
        {
            public string GUID;
            public int Level = 1;
            public int Exp = 0;
            public int Happiness = 0;
            public int Food = 0;
            public int ConstitutionTrainingPoints = 0;
            public int AttackTrainingPoints = 0;
            public int MagicTrainingPoints = 0;
            public int SpeedTrainingPoints = 0;
            public int DefenseTrainingPoints = 0;
            public int ResistanceTrainingPoints = 0;
        }

        [Serializable]
        public class MonsterInventoryData
        {
            public List<MonsterData> MonsterData = new List<MonsterData>();
        }

        private MonsterInventoryData _monsterInventoryData = new MonsterInventoryData();
        public static MonsterInventoryData Data
        {
            get => Instance._monsterInventoryData;
            private set
            {
                Instance._monsterInventoryData = value;
            }
        }

        public static void Save()
        {
            var path = Path.Combine(Application.persistentDataPath + PATH);
            var dataAsJson = JsonUtility.ToJson(Data);
            File.WriteAllText(path, dataAsJson);
        }

        public static void Load()
        {
            var path = Path.Combine(Application.persistentDataPath + PATH);
            if (File.Exists(path))
            {
                var loadedData = File.ReadAllText(path);
                Data = JsonUtility.FromJson<MonsterInventoryData>(loadedData);
                Debug.Log("Monster inventory loaded!");
            }
            else
            {
                Debug.LogWarning("No monster inventory data was not found");
            }
        }

        public static void AddMonster(MonsterData monster, bool save = true)
        {
            if (string.IsNullOrEmpty(monster.GUID))
            {
                monster.GUID = Guid.NewGuid().ToString();
            }

            Data.MonsterData.Add(monster);

            if (save)
            {
                Save();
            }
        }

        public static void RemoveMonster(string GUID, bool save = true)
        {
            var monsterData = Data.MonsterData.Where(m => m.GUID == GUID).FirstOrDefault();
            if (monsterData != null)
            {
                Data.MonsterData = Data.MonsterData.Except(new List<MonsterData>() { monsterData }).ToList();
                
                Debug.Log("Removed monster successfully!");

                if (save)
                {
                    Save();
                }
            }
            else
            {
                Debug.LogError("Removing monster failed, monster not found!");
            }
        }
    }
}