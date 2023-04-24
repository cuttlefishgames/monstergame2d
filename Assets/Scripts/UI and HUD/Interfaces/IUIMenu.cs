using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public interface IUIMenu
    {
        bool IsShowing();
        void Show();
        void Hide();
    }
}