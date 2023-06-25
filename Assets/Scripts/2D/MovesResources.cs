using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MovesResources : ScriptableObject
{
    [System.Serializable]
    public class MoveResource
    {
        public MovesIDs moveID;
        public GameObject movePrefab;
    }

    [SerializeField] private List<MoveResource> _resources;
    public List<MoveResource> Resources => _resources;
}
