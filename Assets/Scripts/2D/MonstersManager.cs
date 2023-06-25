using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Utils;
using System.IO;
using System;
using System.Linq;

public class MonstersManager : Singleton<MonstersManager>
{
    [Serializable]
    public class MonsterData2D
    {
        public Monster2DIDs id = Monster2DIDs.NONE;
        public string guid = string.Empty;
        public string nickName = string.Empty;
        public int level = 1;
        public int experience = 0;
        public Moves move1 = Moves.NONE;
        public Moves move2 = Moves.NONE;
        public Moves move3 = Moves.NONE;
        public Moves move4 = Moves.NONE;
        public int HPTrainingPoints = 0;
        public int SpeedTrainingPoints = 0;
        public int AttackTrainingPoints = 0;
        public int MagicTrainingPoints = 0;
        public int DefenseTrainingPoints = 0;
        public int ResistanceTrainingPoints = 0;
    }

    public class MonsterCreateData
    {
        public Monster2DIDs id;
        public int minLevel = 1;
        public int maxLevel = 1;
        public Moves move1 = Moves.NONE;
        public Moves move2 = Moves.NONE;
        public Moves move3 = Moves.NONE;
        public Moves move4 = Moves.NONE;
        public int HPTrainingPoints = 0;
        public int SpeedTrainingPoints = 0;
        public int AttackTrainingPoints = 0;
        public int MagicTrainingPoints = 0;
        public int DefenseTrainingPoints = 0;
        public int ResistanceTrainingPoints = 0;
    }

    [Serializable]
    public class MonsterManagerData
    {
        public int version = 0;
        public List<MonsterData2D> monsters;
    }

    public static MonsterManagerData Data = null;
    public static List<MonsterData2D> Monsters { get; private set; } = new List<MonsterData2D>();
    public static Dictionary<Monster2DIDs, GameObject> MonstersPrefabs => _monstersPrefabs;
    public static Dictionary<Monster2DIDs, GameObject> MonstersAnimatedSprites => _monstersAnimatedSprites;
    public static MonstersResources Resources => Instance._resources;
    private static readonly string PATH = "monsterManagerData.json";
    private static Dictionary<Monster2DIDs, GameObject> _monstersPrefabs = new Dictionary<Monster2DIDs, GameObject>();
    private static Dictionary<Monster2DIDs, GameObject> _monstersAnimatedSprites = new Dictionary<Monster2DIDs, GameObject>();
    [SerializeField] private MonstersResources _resources;


    public static void Save()
    {
        var path = Path.Combine(Application.persistentDataPath, PATH);
        var data = new MonsterManagerData();
        data.monsters = Monsters;

        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    public static void Load()
    {
        var path = Path.Combine(Application.persistentDataPath, PATH);
        if (File.Exists(path))
        {
            Data = JsonUtility.FromJson<MonsterManagerData>(path);
        }
        else
        {
            Data = new MonsterManagerData();
        }
    }

    public static void AddMonster(MonsterData2D monsterData, bool save = true)
    {
        if (string.IsNullOrEmpty(monsterData.guid))
        {
            monsterData.guid = Guid.NewGuid().ToString();
        }

        Data.monsters.Add(monsterData);
        if (save)
        {
            Save();
        }
    }

    public static MonsterData2D CreateMonster(MonsterCreateData createData)
    {
        var monsterData = new MonsterData2D();
        var random = new System.Random();
        monsterData.id = createData.id;
        monsterData.guid = Guid.NewGuid().ToString();
        monsterData.level = random.Next(createData.minLevel, createData.maxLevel);
        monsterData.move1 = createData.move1;
        monsterData.move2 = createData.move2;
        monsterData.move3 = createData.move3;
        monsterData.move4 = createData.move4;
        monsterData.HPTrainingPoints = createData.HPTrainingPoints;
        monsterData.SpeedTrainingPoints = createData.SpeedTrainingPoints;
        monsterData.AttackTrainingPoints = createData.AttackTrainingPoints;
        monsterData.MagicTrainingPoints = createData.MagicTrainingPoints;
        monsterData.DefenseTrainingPoints = createData.DefenseTrainingPoints;
        monsterData.ResistanceTrainingPoints = createData.ResistanceTrainingPoints;

        return monsterData;
    }

    public static GameObject GetMonsterPrefab(Monster2DIDs monsterId)
    {
        var resource = Resources.Resources.Where(m => m.ID == monsterId).FirstOrDefault();

        if(resource == null)
        {
            Debug.LogError("Monster resource not found for ID: " + monsterId.ToString());
            return null;
        }

        return resource.prefab;
    }

    public static GameObject GetMonsterAnimatedSprite(Monster2DIDs monsterId)
    {
        var resource = Resources.Resources.Where(m => m.ID == monsterId).FirstOrDefault();

        if (resource == null)
        {
            Debug.LogError("Monster resource not found for ID: " + monsterId.ToString());
            return null;
        }

        return resource.animatedSprite;
    }
}