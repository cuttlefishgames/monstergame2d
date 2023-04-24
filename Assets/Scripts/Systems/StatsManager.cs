using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monster
{
    public class StatsManager : MonoBehaviour
    {
        public static StatsManager Instance;

        public static StatsManagerSettings Settings { get => Instance._settings; }
        [SerializeField] private StatsManagerSettings _settings;

        public static GameObject ResourcesDisplayerPrefab { get => Instance._resourcesDisplayerPrefab; }
        [SerializeField] private GameObject _resourcesDisplayerPrefab;

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            var mons = FindObjectsOfType<MonsterEntity>().ToList();
            foreach(var mon in mons)
            {
                mon.UpdateStats();
                AttachResourceDisplayerToMon(mon);
            }
        }

        public static void AttachResourceDisplayerToMon(IMonster mon)
        {
            var resourceObeject = Instantiate(ResourcesDisplayerPrefab);
            var resourceDisplayer = resourceObeject.GetComponent<UI.MonsterResourcesDisplayer>();
            resourceDisplayer.SetWorldCamera(Camera.main);
            resourceDisplayer.SetMonster(mon);
            resourceDisplayer.SubscribeToMonsterEvents();
        }

        public static MonsterStats UpdateStats(MonsterStatsData baseStats, int level)
        {
            MonsterStats updatedStats = new MonsterStats();

            var curvePoint = level / (float)Settings.MaxLevel;
            var curveValue = Settings.StatsGrowthCurve.Evaluate(curvePoint) * Settings.CurveMax;

            int hp = Mathf.RoundToInt((baseStats.Constitution * curveValue * Settings.CurveMax * Settings.HPMultiplier) + Settings.ConstitutionDamping);
            int attack = Mathf.RoundToInt((baseStats.Attack * curveValue * Settings.CurveMax * Settings.AttackMultiplier) + Settings.AttackDamping);
            int magic = Mathf.RoundToInt((baseStats.Magic * curveValue * Settings.CurveMax * Settings.MagicMultiplier) + Settings.MagicDamping);
            int speed = Mathf.RoundToInt((baseStats.Speed * curveValue * Settings.CurveMax * Settings.SpeedMultiplier) + Settings.SpeedDamping);
            int defense = Mathf.RoundToInt((baseStats.Defense * curveValue * Settings.CurveMax * Settings.DefenseMultiplier) + Settings.DefenseDamping);
            int resistance = Mathf.RoundToInt((baseStats.Resistance * curveValue * Settings.CurveMax * Settings.ResistanceMultiplier) + Settings.ResistanceDamping);

            updatedStats.SetLevel(level);
            updatedStats.SetHP(hp);
            updatedStats.SetAttack(attack);
            updatedStats.SetMagic(magic);
            updatedStats.SetSpeed(speed);
            updatedStats.SetDefense(defense);
            updatedStats.SetResistance(resistance);

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

        public static int CalcEffectivePhysicalHP(float physicalArmor, int hp)
        {
            return Mathf.RoundToInt(hp * (1 / physicalArmor));
        }

        public static int CalcEffectiveMagicalHP(float magicalArmor, int hp)
        {
            return Mathf.RoundToInt(hp * (1 / magicalArmor));
        }
    }
}