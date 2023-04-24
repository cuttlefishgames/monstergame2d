using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Monster
{
    public enum MonsterTiers 
    { 
        WORST = 200, 
        BAD = 250,
        SUPER_WEAK = 300,
        WEAK = 350,
        BELOW_AVERAGE = 400,
        AVERAGE = 450,
        GOOD = 500,
        STRONG = 550,
        SUPER_STRONG = 600,
        LEGENDARY = 700
    }

    public enum MonsterElements
    {
        NEUTRAL = 0,
        SOUL = 100,
        LIGHT = 200,
        SHADOW = 300,
        CAELUS = 400,
        ACQUA = 500,
        PYRO = 600,
        TERRA = 700,
        CRYO = 800,
        NATURA = 900,
        CORRUPTION = 1000,
        DEVOIDED = 1100
    }

    [CreateAssetMenu(fileName = "MonstersStatsSettings", menuName = "Stats/Monsters General Stats Settings")]
    public class MonstersStatsSettings : ScriptableObject
    {
        [SerializeField] private AnimationCurve _expGrowthCurve;
        [SerializeField] private int _expBase = 100;
        //[SerializeField] private AnimationCurve _baby_exp_exp_curve;
        //[SerializeField] private AnimationCurve _infant_exp_curve;
        //[SerializeField] private AnimationCurve _toddler_exp_curve;
        //[SerializeField] private AnimationCurve _kid_exp_curve;
        //[SerializeField] private AnimationCurve _young_exp_curve;
        //[SerializeField] private AnimationCurve _average_exp_curve;
        //[SerializeField] private AnimationCurve _strong_exp_curve;
        //[SerializeField] private AnimationCurve _very_strong_exp_curve;
        //[SerializeField] private AnimationCurve _super_strong_exp_curve;
        //[SerializeField] private AnimationCurve _legendary_exp_curve;

        [SerializeField] private MonsterTiers _exempleTier = MonsterTiers.WORST;
        [SerializeField] [Range(1, 100)] private int _exempleLevel = 1;
        [SerializeField] private int _experienceNeeded;

        private void OnValidate()
        {
            var level = (float)_exempleLevel;
            //_experienceNeeded = Mathf.CeilToInt(_expBase * ((float)_exempleTier) * _expGrowthCurve.Evaluate(level/100f));
        }
    }
}