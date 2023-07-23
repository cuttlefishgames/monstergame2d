using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStateSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _characterName;
    [SerializeField] private TextMeshProUGUI _characterLevel;
    [SerializeField] private Image _portrait;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private EnergyBar _energyBar;
    [SerializeField] private ActionBar _actionBar;
    [SerializeField] private RectTransform _maskRect;
    [SerializeField] private RectTransform _portraitRect;
    [SerializeField] private Animator _animator;

    public HealthBar HealthBar => _healthBar;
    public EnergyBar EnergyBar => _energyBar;
    public ActionBar ActionBar => _actionBar;
    public Entity2D Entity { get; private set; }

    public void Clear()
    {
        Entity = null;
    }

    public void SetState(CharacterSlotStates state)
    {
        if(_animator != null)
        {
            _animator.SetInteger("State", (int)state);
        }
    }

    public void SetEntity(Entity2D entity)
    {
        Entity = entity;

        if(_characterName != null)
        {
            _characterName.text = entity.name;
        }

        if (_characterLevel != null)
        {
            _characterLevel.text = "Lv" + entity.Level.ToString();
        }

        if(_portrait != null)
        {
            _portrait.sprite = entity.Sprite;
        }

        if (HealthBar != null)
        {
            HealthBar.SetEntity(entity);
        }

        if (EnergyBar != null)
        {
            EnergyBar.SetEntity(entity);
        }

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
