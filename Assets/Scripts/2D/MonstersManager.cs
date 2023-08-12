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
        public MovesIDs move1 = MovesIDs.NONE;
        public MovesIDs move2 = MovesIDs.NONE;
        public MovesIDs move3 = MovesIDs.NONE;
        public MovesIDs move4 = MovesIDs.NONE;
        public MovesIDs move5 = MovesIDs.NONE;
        public MovesIDs move6 = MovesIDs.NONE;
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
        public MovesIDs move1 = MovesIDs.NONE;
        public MovesIDs move2 = MovesIDs.NONE;
        public MovesIDs move3 = MovesIDs.NONE;
        public MovesIDs move4 = MovesIDs.NONE;
        public MovesIDs move5 = MovesIDs.NONE;
        public MovesIDs move6 = MovesIDs.NONE;
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
    private static readonly string PATH = "monsterManagerData.json";
    private static Dictionary<Monster2DIDs, GameObject> _monstersPrefabs = new Dictionary<Monster2DIDs, GameObject>();
    private static Dictionary<Monster2DIDs, GameObject> _monstersAnimatedSprites = new Dictionary<Monster2DIDs, GameObject>();
    [SerializeField] private MonstersResources _monstersResources;
    public static MonstersResources MonstersResources => Instance._monstersResources;
    [SerializeField] private MovesResources _movesResources;
    public static MovesResources MovesResources => Instance._movesResources;

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

    //public MonsterStatsScaled ScaleStats(Entity2DStats baseStats, int level)
    //{
    //    MonsterStatsScaled scaledStats = new MonsterStatsScaled();

    //    return scaledStats;
    //}


    //[SerializeField] private MonsterStatsData _stats;
    [SerializeField] [Range(1, 100)] private int _level = 1;

    //private MonsterStats _updatedStats;

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

    //public void CalculateStats()
    //{
    //    _level = Mathf.Min(_level, GameManager2D.MaxLevel);
    //    _updatedStats = StatsManager.UpdateStats(_stats, _level);
    //    if (_updatedStats != null)
    //    {
    //        _HP = _updatedStats.HP;
    //        _attack = _updatedStats.Attack;
    //        _magic = _updatedStats.Magic;
    //        _speed = _updatedStats.Speed;
    //        _defense = _updatedStats.Defense;
    //        _resistance = _updatedStats.Resistance;

    //        var physArmor = StatsManager.CalcPhysicalArmor(_defense);
    //        var magiArmor = StatsManager.CalcPhysicalArmor(_resistance);

    //        _physicalArmor = 100f * (1f - physArmor);
    //        _magicalArmor = 100f * (1f - magiArmor);

    //        _physicalEffectiveHealth = StatsManager.CalcEffectivePhysicalHP(physArmor, _HP);
    //        _physicalHealthIncrease = _physicalEffectiveHealth / (float)_HP;
    //        _magicalEffectiveHealth = StatsManager.CalcEffectivePhysicalHP(magiArmor, _HP);
    //        _magicalHealthIncrease = _magicalEffectiveHealth / (float)_HP;

    //        _expNeededForNextLevel = GameManager.GetExperienceNeeded(_stats.ExpNeededCategory, _level);
    //        _expGiven = GameManager.GetExperienceGivenScaled(_level, _stats.BaseExp);
    //    }
    //}

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
        monsterData.HPTrainingPoints = createData.HPTrainingPoints;
        monsterData.SpeedTrainingPoints = createData.SpeedTrainingPoints;
        monsterData.AttackTrainingPoints = createData.AttackTrainingPoints;
        monsterData.MagicTrainingPoints = createData.MagicTrainingPoints;
        monsterData.DefenseTrainingPoints = createData.DefenseTrainingPoints;
        monsterData.ResistanceTrainingPoints = createData.ResistanceTrainingPoints;

        var moves = new List<MovesIDs>()
        {
            createData.move1,
            createData.move2,
            createData.move3,
            createData.move4,
            createData.move5,
            createData.move6
        };
        var prefab = GetMonsterPrefab(createData.id);
        var movesLearnByLevelData = prefab.GetComponent<Entity2D>().MovesLearnedByLevelUp;
        int currentLevel = monsterData.level;
        var availableMoves = movesLearnByLevelData.MovesLearnData.Select(m => m.move).Except(moves).ToList();
        while (availableMoves.Count > 0 && moves.Any(m => m == MovesIDs.NONE))
        {
            var emptySlotIndex = moves.IndexOf(moves.Where(m => m == MovesIDs.NONE).FirstOrDefault());
            var move = availableMoves.Last();
            moves[emptySlotIndex] = move;
            availableMoves.Remove(move);
        }

        monsterData.move1 = moves[0];
        monsterData.move2 = moves[1];
        monsterData.move3 = moves[2];
        monsterData.move4 = moves[3];
        monsterData.move5 = moves[4];
        monsterData.move6 = moves[5];

        return monsterData;
    }

    public static GameObject GetMonsterPrefab(Monster2DIDs monsterId)
    {
        var resource = MonstersResources.Resources.Where(m => m.ID == monsterId).FirstOrDefault();

        if (resource == null)
        {
            Debug.LogError("Monster resource not found for ID: " + monsterId.ToString());
            return null;
        }

        return resource.prefab;
    }

    public static GameObject GetMonsterAnimatedSprite(Monster2DIDs monsterId)
    {
        var resource = MonstersResources.Resources.Where(m => m.ID == monsterId).FirstOrDefault();

        if (resource == null)
        {
            Debug.LogError("Monster resource not found for ID: " + monsterId.ToString());
            return null;
        }

        return resource.animatedSprite;
    }
}