using UnityEngine;

namespace Monster
{
    public interface IMonster
    {
        public Elements GetElement();
        string GetGUID();
        void SetGUID(string GUID);
        GameObject GetGameObject();
        MonsterStatsData GetMonsterStats();
        void SetLevel(int level);
        int GetLevel();
        MonsterAnimationController GetAnimationController();
        MonsterAnimationSpecialPoints GetPointsManager();
        MonsterEventsManager GetMonsterEventsManager();
        void UpdateStats();
        float GetHPStatus();
        void ReduceHP(int hp);
    }
}