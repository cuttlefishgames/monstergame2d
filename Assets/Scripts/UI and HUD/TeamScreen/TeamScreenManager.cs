using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Monster
{
    public class TeamScreenManager : Utils.Singleton<TeamScreenManager>, IUIMenu
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private GameObject _monSlotPrefab;
        [SerializeField] private RectTransform _monSlotsContainer;

        private List<GameObject> _monSlots;

        private bool _isShowing;

        #region interface implementation
        public bool IsShowing()
        {
            return _isShowing;
        }       

        public void Show()
        {
            _canvas.SetActive(true);
            _isShowing = true;
        }

        public void Hide()
        {
            _canvas.SetActive(false);
            _isShowing = false;
        }
        #endregion interface implementation

    }
}