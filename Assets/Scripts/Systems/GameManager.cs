using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Monster
{
    public class GameManager : Utils.Singleton<GameManager>
    {
        [Serializable]
        public class ExpCategoryData
        {
            public ExpNeededCategories ExpCategory;
            [Range(1, 10000000)] public int ExpNeededAtMaxLevel = 1000000;
        }

        [Header("The curve is normalized, always multiply the value by ExpScalingMultiplier")]
        [SerializeField] [Range(1, 100)] private int _maxLevel = 50;
        [SerializeField] private AnimationCurve _neededExpScalingCurve;
        [SerializeField] private int _neededExpScalingMultiplier = 100;
        [SerializeField] private AnimationCurve _expGivenScalingCurve;
        [SerializeField] private int _expGivenScalingMultiplier = 100;
        [SerializeField] private List<ExpCategoryData> _expCategoriesData;

        //public properties
        public static int MaxLevel { get => Instance._maxLevel; }
        public static AnimationCurve NeededExpScalingCurve { get => Instance._neededExpScalingCurve; }
        public static int NeededExpScalingMultiplier { get => Instance._neededExpScalingMultiplier; }
        public static AnimationCurve ExpGivenScalingCurve { get => Instance._expGivenScalingCurve; }
        public static int ExpGivenScalingMultiplier { get => Instance._expGivenScalingMultiplier; }
        public static List<ExpCategoryData> ExpCategoriesData { get => Instance._expCategoriesData; }

        private void OnValidate()
        {
            if (_expCategoriesData == null || _expCategoriesData.Count == 0)
            {
                _expCategoriesData = new List<ExpCategoryData>();

                _expCategoriesData.Add(
                    new ExpCategoryData()
                    {
                        ExpCategory = ExpNeededCategories.VERY_EASY,
                        ExpNeededAtMaxLevel = 1000000
                    });

                _expCategoriesData.Add(
                    new ExpCategoryData()
                    {
                        ExpCategory = ExpNeededCategories.EASY,
                        ExpNeededAtMaxLevel = 1250000
                    });

                _expCategoriesData.Add(
                    new ExpCategoryData()
                    {
                        ExpCategory = ExpNeededCategories.AVERAGE,
                        ExpNeededAtMaxLevel = 1500000
                    });

                _expCategoriesData.Add(
                    new ExpCategoryData()
                    {
                        ExpCategory = ExpNeededCategories.HARD,
                        ExpNeededAtMaxLevel = 1750000
                    });

                _expCategoriesData.Add(
                    new ExpCategoryData()
                    {
                        ExpCategory = ExpNeededCategories.VERY_HARD,
                        ExpNeededAtMaxLevel = 2000000
                    });
            }
        }

        public static int GetExperienceNeeded(ExpNeededCategories expNeededCategory, int level)
        {
            var expData = ExpCategoriesData.Where(s => s.ExpCategory == expNeededCategory).FirstOrDefault();
            var scaledExp =
                NeededExpScalingCurve.Evaluate(((float)level / (float)MaxLevel)) * (float)expData.ExpNeededAtMaxLevel;
            return Mathf.CeilToInt(scaledExp);
        }

        public static int GetExperienceGivenScaled(int baseExp, int level)
        {
            var scaledExp = 
                ExpGivenScalingCurve.Evaluate(((float)level / (float)MaxLevel)) * (float)baseExp * (float)ExpGivenScalingMultiplier;
            return baseExp + Mathf.CeilToInt(scaledExp);
        }
    }
}