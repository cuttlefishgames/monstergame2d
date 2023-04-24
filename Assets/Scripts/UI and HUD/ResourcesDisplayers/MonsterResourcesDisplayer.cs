using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monster.UI
{
    public class MonsterResourcesDisplayer : MonoBehaviour
    {
        public IMonster Mon { get; protected set; }

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _hpFill;

        public void SetWorldCamera(Camera worldCamera)
        {
            _canvas.worldCamera = worldCamera;
        }

        public void SetMonster(IMonster mon)
        {
            Mon = mon;
            _canvas.transform.position = Mon.GetPointsManager().GetPoint(AnimationPoints.UPPER_CENTER).position;
        }

        public void SubscribeToMonsterEvents()
        {
            UnSubscribeToMonsterEvents();
            if (Mon as Object != null)
            {
                var monEventsManager = Mon.GetMonsterEventsManager();
                monEventsManager.HPReduced += OnHPReduced;
            }
        }

        public void UnSubscribeToMonsterEvents()
        {
            if (Mon as Object != null)
            {
                var monEventsManager = Mon.GetMonsterEventsManager();
                monEventsManager.HPReduced -= OnHPReduced;
            }
        }

        private void OnHPReduced(int hp)
        {
            _hpFill.fillAmount = Mon.GetHPStatus();
        }
    }
}