using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MonstersResources : ScriptableObject
{
    [System.Serializable]
    public class MonsterResource 
    {
        public Monster2DIDs ID;
        public GameObject prefab;
        public GameObject animatedSprite;
    }

    [SerializeField] private List<MonsterResource> _resources = new List<MonsterResource>();
    public List<MonsterResource> Resources => _resources;
}
