using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public class MonsterStatsScaled
    {
        public int level;
        public int hp;
        public int speed;
        public int attack;
        public int defense;
        public int magic;
        public int resistance;
    }

    public static StatsManager Instance;

    public static StatsManagerSettings Settings { get => Instance._settings; }
    [SerializeField] private StatsManagerSettings _settings;

    public static GameObject ResourcesDisplayerPrefab { get => Instance._resourcesDisplayerPrefab; }
    [SerializeField] private GameObject _resourcesDisplayerPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
    }

    public static MonsterStatsScaled ScaleStats(Entity2DStats baseStats, int level)
    {
        MonsterStatsScaled updatedStats = new MonsterStatsScaled();

        var curvePoint = level / (float)Settings.MaxLevel;
        var curveValue = Settings.StatsGrowthCurve.Evaluate(curvePoint) * Settings.CurveMax;

        int hp = Mathf.RoundToInt((baseStats.BaseConstitution * curveValue * Settings.CurveMax * Settings.HPMultiplier) + Settings.ConstitutionDamping);
        int attack = Mathf.RoundToInt((baseStats.BaseAttack * curveValue * Settings.CurveMax * Settings.AttackMultiplier) + Settings.AttackDamping);
        int magic = Mathf.RoundToInt((baseStats.BaseMagic * curveValue * Settings.CurveMax * Settings.MagicMultiplier) + Settings.MagicDamping);
        int speed = Mathf.RoundToInt((baseStats.BaseSpeed * curveValue * Settings.CurveMax * Settings.SpeedMultiplier) + Settings.SpeedDamping);
        int defense = Mathf.RoundToInt((baseStats.BaseDefense * curveValue * Settings.CurveMax * Settings.DefenseMultiplier) + Settings.DefenseDamping);
        int resistance = Mathf.RoundToInt((baseStats.BaseResistance * curveValue * Settings.CurveMax * Settings.ResistanceMultiplier) + Settings.ResistanceDamping);

        updatedStats.level = level;
        updatedStats.hp = hp;
        updatedStats.attack = attack;
        updatedStats.magic = magic;
        updatedStats.speed = speed;
        updatedStats.defense = defense;
        updatedStats.resistance = resistance;

        return updatedStats;
    }

    public static float CalcPhysicalArmor(int defense, float penetrationPercent = 0)
    {
        var defenseAfterPercentageReduction = Mathf.Max(0, defense * (1f - penetrationPercent));
        return Settings.DefenseMiddleValue / (Settings.DefenseMiddleValue + defenseAfterPercentageReduction);
    }

    public static float CalcMagicalArmor(int resistance, float penetrationPercent = 0)
    {
        var resistanceAfterPercentageReduction = Mathf.Max(0, resistance * (1f - penetrationPercent));
        return Settings.ResistanceMiddleValue / (Settings.ResistanceMiddleValue + resistanceAfterPercentageReduction);
    }

    public static int GetRawPowerFromStat(Stats stat, MonsterStatsScaled stats, float multiplier = 100)
    {
        int rawPower = 0;
        float rawPowerFloat = 0;

        switch (stat)
        {
            default:
                rawPowerFloat = stats.attack * (multiplier / 100f);
                break;
            case Stats.ATTACK:
                rawPowerFloat = stats.attack * (multiplier / 100f);
                break;
            case Stats.MAGIC:
                rawPowerFloat = stats.magic * (multiplier / 100f);
                break;
            case Stats.SPEED:
                rawPowerFloat = stats.speed * (multiplier / 100f);
                break;
            case Stats.HP:
                rawPowerFloat = stats.hp * (multiplier / 100f);
                break;
            case Stats.DEFENSE:
                rawPowerFloat = stats.defense * (multiplier / 100f);
                break;
            case Stats.RESISTANCE:
                rawPowerFloat = stats.resistance * (multiplier / 100f);
                break;
        }

        rawPower = Mathf.FloorToInt(rawPowerFloat);
        return rawPower;
    }

    public static int CalcEffectivePhysicalHP(float physicalArmor, int hp)
    {
        return Mathf.RoundToInt(hp * (1 / physicalArmor));
    }

    public static int CalcEffectiveMagicalHP(float magicalArmor, int hp)
    {
        return Mathf.RoundToInt(hp * (1 / magicalArmor));
    }
}
