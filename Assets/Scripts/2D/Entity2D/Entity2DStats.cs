using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Entity2DStats : ScriptableObject
{
    [SerializeField] private int _speed;
    public int Speed => _speed;
}
