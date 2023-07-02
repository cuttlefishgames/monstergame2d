using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionBar2D : MonoBehaviour
{
    public Entity2D Entity { get; private set; }
    [SerializeField] private Image _fill;
    [SerializeField] private TextMeshProUGUI _value;

    public void Mirror()
    {
        _fill.fillOrigin = (int)Image.OriginHorizontal.Right;
        var rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.pivot = new Vector2(1, 0.5f);
        rectTransform.anchoredPosition = new Vector2(220, 0); 
        //_value.transform.localScale = new Vector3(
        //        -1 * Mathf.Abs(_value.transform.localScale.x),
        //        _value.transform.localScale.y,
        //        _value.transform.localScale.z);
    }

    public void SetEntity(Entity2D entity)
    {
        Entity = entity;
        Entity.Events.ActionPointsUpdated += OnActionPointsUpdatedListener;
        UpdateFill(Entity.ActionPoints);
    }

    private void OnActionPointsUpdatedListener(int actionPoints)
    {
        UpdateFill(actionPoints);
    }

    public void UpdateFill(int actionPoints)
    {
        var fill = actionPoints / (float)BattleManager2D.FillLimit;
        _fill.fillAmount = fill;
        _value.text = Mathf.FloorToInt(fill * 100f).ToString()+"%";
    }

    public void Clear()
    {
        if(Entity != null)
        {
            Entity.Events.ActionPointsUpdated -= OnActionPointsUpdatedListener;
            UpdateFill(0);
        }
    }

    public void InvetValue()
    {
        _value.transform.localScale = 
            new Vector3(_value.transform.localScale.x * -1, _value.transform.localScale.y, _value.transform.localScale.z);
    }
}
