using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Utils;
using UnityEngine.UI;

public class BattleManager2D : Singleton<BattleManager2D>
{
    public static float FillLimit => Instance._fillLimit;
    [SerializeField] private int _fillLimit = 2000;
    [SerializeField] private int _speedDamp = 0;
    [SerializeField] private float _fillSpeed = 1;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _characterStateSlotPrefab;
    [SerializeField] private Transform _leftSideSlotsContent;
    [SerializeField] private Transform _rightSideSlotsContent;
    private Coroutine _fillBars;
    private List<CharacterStateSlot> _leftSlots;
    private List<CharacterStateSlot> _rightSlots;

    protected override void Awake()
    {
        base.Awake();
        _leftSlots = new List<CharacterStateSlot>();
        _rightSlots = new List<CharacterStateSlot>();
    }

    public static void Show()
    {
        var leftSidePositions = Battlefield2D.GetAllCharacterSlotsOfSide(TeamSides.LEFT);
        var rightSidePositions = Battlefield2D.GetAllCharacterSlotsOfSide(TeamSides.RIGHT);

        foreach(var posInfo in leftSidePositions)
        {
            var slotObj = Instantiate(Instance._characterStateSlotPrefab);
            var slot = slotObj.GetComponent<CharacterStateSlot>();
            slot.ActionBar.SetEntity(posInfo.Entity);
            slot.transform.SetParent(Instance._leftSideSlotsContent);
            slot.transform.localScale = Vector3.one;
            Instance._leftSlots.Add(slot);
        }

        foreach (var posInfo in rightSidePositions)
        {
            var slotObj = Instantiate(Instance._characterStateSlotPrefab);
            var slot = slotObj.GetComponent<CharacterStateSlot>();
            slot.ActionBar.SetEntity(posInfo.Entity);
            slot.ActionBar.InvetValue();
            slot.transform.SetParent(Instance._rightSideSlotsContent);
            slot.transform.localScale = Vector3.one;
            Instance._rightSlots.Add(slot);
        }

        Instance._canvas.SetActive(true);
    }

    public static void FillBars()
    {
        if(Instance._fillBars != null)
        {
            Instance.StopCoroutine(Instance._fillBars);
        }

        Instance._fillBars = Instance.StartCoroutine(Instance.FillBarsCoroutine());
    }

    IEnumerator FillBarsCoroutine()
    {
        var allEntites = Battlefield2D.GetAllCharactersAlive();
        var hasFilled = false;
        Entity2D nextTurnHolder = null;
        while (!hasFilled)
        {
            foreach(var ent in allEntites)
            {
                var fill = Mathf.CeilToInt(_speedDamp + (_fillSpeed * Time.deltaTime * ent.BaseStats.Speed));
                ent.AddActionPoints(fill);
                if (ent.ActionPoints >= _fillLimit)
                {
                    //hasFilled = true;
                    nextTurnHolder = ent;
                    ent.ResetActionPoints();
                }
            }

            yield return Wait.ForEndOfFrame;
        }
    }
}
