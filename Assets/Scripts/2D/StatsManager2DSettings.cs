using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsManager2DSettings", menuName = "Stats/Stats Manager 2D Settings")]
public class StatsManagerSettings : ScriptableObject
{
    [SerializeField] [Range(1, 100)] private int _maxLevel = 100;
    [SerializeField] private AnimationCurve _statsGrowthCurve;
    [SerializeField] [Range(1, 10)] private int _curveMax = 2;
    [SerializeField] [Range(1, 10)] private float _HPMultiplier = 3;
    [SerializeField] [Range(1, 10)] private float _attackMultiplier = 1;
    [SerializeField] [Range(1, 10)] private float _magicMultiplier = 1;
    [SerializeField] [Range(1, 10)] private float _speedMultiplier = 1;
    [SerializeField] [Range(1, 10)] private float _defenseMultiplier = 1;
    [SerializeField] [Range(1, 10)] private float _resistanceMultiplier = 1;
    [SerializeField] [Range(0, 1000)] private int _constitutionDamping = 0;
    [SerializeField] [Range(0, 1000)] private int _attackDamping = 0;
    [SerializeField] [Range(0, 1000)] private int _magicDamping = 0;
    [SerializeField] [Range(0, 1000)] private int _speedDamping = 0;
    [SerializeField] [Range(0, 1000)] private int _defenseDamping = 0;
    [SerializeField] [Range(0, 1000)] private int _resistanceDamping = 0;
    [SerializeField] [Range(1, 10000)] private int _defenseMiddleValue = 500;
    [SerializeField] [Range(1, 10000)] private int _resistanceMiddleValue = 500;

    public int MaxLevel { get => _maxLevel; }
    public AnimationCurve StatsGrowthCurve { get => _statsGrowthCurve; }
    public int CurveMax { get => _curveMax; }
    public float HPMultiplier { get => _HPMultiplier; }
    public float AttackMultiplier { get => _attackMultiplier; }
    public float MagicMultiplier { get => _magicMultiplier; }
    public float SpeedMultiplier { get => _speedMultiplier; }
    public float DefenseMultiplier { get => _defenseMultiplier; }
    public float ResistanceMultiplier { get => _resistanceMultiplier; }
    public float ConstitutionDamping { get => _constitutionDamping * _HPMultiplier; }
    public float AttackDamping { get => _attackDamping * _attackMultiplier; }
    public float MagicDamping { get => _magicDamping * _magicMultiplier; }
    public float SpeedDamping { get => _speedDamping * _speedMultiplier; }
    public float DefenseDamping { get => _defenseDamping * _defenseMultiplier; }
    public float ResistanceDamping { get => _resistanceDamping * _resistanceMultiplier; }
    public float DefenseMiddleValue { get => _defenseMiddleValue; }
    public float ResistanceMiddleValue { get => _resistanceMiddleValue; }
}