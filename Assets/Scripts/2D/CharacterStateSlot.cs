using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateSlot : MonoBehaviour
{
    [SerializeField] private bool _mirror;
    [SerializeField] private ActionBar2D _actionBar;
    public ActionBar2D ActionBar => _actionBar;
    public Entity2D Entity { get; private set; }

    private void OnValidate()
    {
        if (_mirror)
        {
            if(ActionBar != null)
            {
                ActionBar.Mirror();
            }
        }
    }

    public void Clear()
    {
        Entity = null;
    }

    public void SetEntity(Entity2D entity)
    {
        Entity = entity;

        if (ActionBar != null)
        {
            ActionBar.SetEntity(entity);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
