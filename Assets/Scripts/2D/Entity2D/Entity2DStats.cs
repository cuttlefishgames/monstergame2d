using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Entity2DStats : ScriptableObject
{
    [SerializeField] private int _baseConstitution;
    [SerializeField] private int _baseSpeed;
    [SerializeField] private int _baseAttack;
    [SerializeField] private int _baseDefense;
    [SerializeField] private int _baseMagic;
    [SerializeField] private int _baseResistance;
    public int BaseConstitution => _baseConstitution;
    public int BaseSpeed => _baseSpeed;
    public int BaseAttack => _baseAttack;
    public int BaseDefense => _baseDefense;
    public int BaseMagic => _baseMagic;
    public int BaseResistance => _baseResistance;
}
