using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MovesLearnedByLevelUpData : ScriptableObject
{
    [System.Serializable]
    public class MoveLearnData
    {
        public int requiredLevel = 1;
        public MovesIDs move = MovesIDs.NONE;
    }

    [SerializeField] List<MoveLearnData> _movesLearnData = new List<MoveLearnData>();
    public List<MoveLearnData> MovesLearnData => _movesLearnData;
}
