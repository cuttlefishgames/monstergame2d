using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Utils;
using UnityEngine.UI;
using TMPro;

public class BattleManager2D : Singleton<BattleManager2D>
{
    public static float FillLimit => Instance._fillLimit;
    public static int BattlefieldLayer => Instance._battlefieldLayer;
    [SerializeField] private int _battlefieldLayer;
    [SerializeField] private int _fillLimit = 2000;
    [SerializeField] private int _speedDamp = 0;
    [SerializeField] private float _fillSpeed = 10;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _characterStateSlotPrefab;
    [SerializeField] private Transform _leftSideSlotsContent;
    [SerializeField] private Transform _rightSideSlotsContent;

    //battle menu
    [SerializeField] private GameObject _battleMenuCanvas;
    [SerializeField] private GameObject _battleMenuPanel;
    [SerializeField] private Button _movesButton;
    [SerializeField] private Button _restButton;
    [SerializeField] private Button _itemsButton;
    [SerializeField] private Button _runButton;
    [SerializeField] private GameObject _movesPanel;
    [SerializeField] private TextMeshProUGUI _move1Text;
    [SerializeField] private TextMeshProUGUI _move2Text;
    [SerializeField] private TextMeshProUGUI _move3Text;
    [SerializeField] private TextMeshProUGUI _move4Text;
    [SerializeField] private Button _move1Button;
    [SerializeField] private Button _move2Button;
    [SerializeField] private Button _move3Button;
    [SerializeField] private Button _move4Button;
    [SerializeField] private Button _backFromMovesButton;

    private Coroutine _fillBars;
    private List<CharacterStateSlot> _leftSlots;
    private List<CharacterStateSlot> _rightSlots;

    protected override void Awake()
    {
        base.Awake();
        _leftSlots = new List<CharacterStateSlot>();
        _rightSlots = new List<CharacterStateSlot>();

        _movesButton.onClick.RemoveAllListeners();
        _movesButton.onClick.AddListener(Moves);

        _restButton.onClick.RemoveAllListeners();
        _restButton.onClick.AddListener(Rest);

        _itemsButton.onClick.RemoveAllListeners();
        _itemsButton.onClick.AddListener(Items);

        _runButton.onClick.RemoveAllListeners();
        _runButton.onClick.AddListener(Run);

        _move1Button.onClick.RemoveAllListeners();
        _move2Button.onClick.RemoveAllListeners();
        _move3Button.onClick.RemoveAllListeners();
        _move4Button.onClick.RemoveAllListeners();
        _move1Button.onClick.AddListener(() => { SelectMove(0); });
        _move2Button.onClick.AddListener(() => { SelectMove(1); });
        _move3Button.onClick.AddListener(() => { SelectMove(2); });
        _move4Button.onClick.AddListener(() => { SelectMove(3); });

        _backFromMovesButton.onClick.RemoveAllListeners();
        _backFromMovesButton.onClick.AddListener(BackFromMoves);
    }

    private void SelectMove(int moveSlot)
    {

    }

    private void BackFromMoves()
    {
        _movesPanel.gameObject.SetActive(false);
        _battleMenuPanel.gameObject.SetActive(true);
    }

    private void Moves()
    {
        _movesPanel.gameObject.SetActive(true);
        _battleMenuPanel.gameObject.SetActive(false);
    }

    private void Rest()
    {

    }

    private void Items()
    {

    }

    private void Run()
    {

    }

    public static void Show()
    {
        var leftSidePositions = Battlefield2D.GetAllCharacterSlotsOfSide(TeamSides.LEFT);
        var rightSidePositions = Battlefield2D.GetAllCharacterSlotsOfSide(TeamSides.RIGHT);

        foreach (var posInfo in leftSidePositions)
        {
            if (posInfo.Entity as UnityEngine.Object == null)
                continue;

            var sorter = posInfo.Entity.Root.GetComponentInChildren<SortingHelper>();
            if (sorter != null)
            {
                sorter.Group.sortingLayerName = LayerMask.LayerToName(Instance._battlefieldLayer);
            }

            var slotObj = Instantiate(Instance._characterStateSlotPrefab);
            var slot = slotObj.GetComponent<CharacterStateSlot>();
            slot.ActionBar.SetEntity(posInfo.Entity);
            slot.transform.SetParent(Instance._leftSideSlotsContent);
            slot.transform.localScale = Vector3.one;
            Instance._leftSlots.Add(slot);
        }

        foreach (var posInfo in rightSidePositions)
        {
            if (posInfo.Entity as UnityEngine.Object == null)
                continue;

            var sorter = posInfo.Entity.Root.GetComponentInChildren<SortingHelper>();
            if (sorter != null)
            {
                sorter.Group.sortingLayerName = LayerMask.LayerToName(Instance._battlefieldLayer);
            }

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
        if (Instance._fillBars != null)
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
            foreach (var ent in allEntites)
            {
                var fill = Mathf.CeilToInt(_fillSpeed * Time.deltaTime * (ent.BaseStats.Speed + _speedDamp));
                ent.AddActionPoints(fill);
                if (ent.ActionPoints >= _fillLimit)
                {
                    hasFilled = true;
                    nextTurnHolder = ent;
                    ent.ResetActionPoints();
                }
            }

            yield return Wait.ForEndOfFrame;
        }

        UpdateInterface(nextTurnHolder);
    }

    private void UpdateInterface(Entity2D turnHolder)
    {
        if (turnHolder.Side == TeamSides.RIGHT)
        {
            _battleMenuCanvas.SetActive(false);
            return;
        }

        _battleMenuCanvas.SetActive(true);
        _battleMenuPanel.SetActive(true);
        _movesPanel.SetActive(false);

        _move1Text.text = turnHolder.KnownMoves[0] != global::Moves.NONE ? turnHolder.KnownMoves[0].ToString() : "--";
        _move2Text.text = turnHolder.KnownMoves[1] != global::Moves.NONE ? turnHolder.KnownMoves[1].ToString() : "--";
        _move3Text.text = turnHolder.KnownMoves[2] != global::Moves.NONE ? turnHolder.KnownMoves[2].ToString() : "--";
        _move4Text.text = turnHolder.KnownMoves[3] != global::Moves.NONE ? turnHolder.KnownMoves[3].ToString() : "--";
    }

    public static void SetGameLayerRecursive(GameObject _go, int _layer)
    {
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = _layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, _layer);
        }
    }
}
