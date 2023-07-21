using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Utils;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    public List<CharacterStateSlot> LeftSideCharacterSlots;
    public List<CharacterStateSlot> RightSideCharacterSlots;

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
    [SerializeField] private TextMeshProUGUI _move5Text;
    [SerializeField] private TextMeshProUGUI _move6Text;
    [SerializeField] private Button _move1Button;
    [SerializeField] private Button _move2Button;
    [SerializeField] private Button _move3Button;
    [SerializeField] private Button _move4Button;
    [SerializeField] private Button _move5Button;
    [SerializeField] private Button _move6Button;
    [SerializeField] private Button _backFromMovesButton;

    private Coroutine _fillBars;
    private List<CharacterStateSlot> _leftSlots;
    private List<CharacterStateSlot> _rightSlots;

    public Entity2D TurnHolder { get; private set; }
    public MovesIDs CurrentSelectedMove { get; private set; }

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
        _move5Button.onClick.RemoveAllListeners();
        _move6Button.onClick.RemoveAllListeners();
        _move1Button.onClick.AddListener(() => { SelectMove(0); });
        _move2Button.onClick.AddListener(() => { SelectMove(1); });
        _move3Button.onClick.AddListener(() => { SelectMove(2); });
        _move4Button.onClick.AddListener(() => { SelectMove(3); });
        _move5Button.onClick.AddListener(() => { SelectMove(4); });
        _move6Button.onClick.AddListener(() => { SelectMove(5); });

        _backFromMovesButton.onClick.RemoveAllListeners();
        _backFromMovesButton.onClick.AddListener(BackFromMoves);
    }

    private void SelectMove(int moveSlot)
    {
        var move = TurnHolder.KnownMoves[moveSlot];
        if (move == MovesIDs.NONE)
            return;

        CurrentSelectedMove = move;

        var moveData = MonstersManager.MovesResources.Resources.Where(m => m.moveID == CurrentSelectedMove).FirstOrDefault();
        if(moveData != null)
        {
            var moveObjectInstance = Instantiate(moveData.movePrefab);
            var order = moveObjectInstance.GetComponent<BattleOrder2D>();
            order.SetCaster(TurnHolder);
            var target = Battlefield2D.GetAllCharacterSlotsOfSide(TeamSides.RIGHT).Select(s => s.Entity).FirstOrDefault();
            order.SetTargets(new List<Entity2D> { target });
            order.Execute();
        }
    }

    private void BackFromMoves()
    {
        _movesPanel.gameObject.SetActive(false);
        _battleMenuPanel.gameObject.SetActive(true);
        CurrentSelectedMove = MovesIDs.NONE;
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
        int leftSlotIndex = 0;
        int rightSlotIndex = 0;

        foreach (var posInfo in leftSidePositions)
        {
            if (posInfo.Entity as UnityEngine.Object == null)
                continue;

            var sorter = posInfo.Entity.Root.GetComponentInChildren<SortingHelper>();
            if (sorter != null)
            {
                sorter.Group.sortingLayerName = LayerMask.LayerToName(Instance._battlefieldLayer);
            }

            //var slotObj = Instantiate(Instance._characterStateSlotPrefab);
            //var slot = slotObj.GetComponent<CharacterStateSlot>();
            //slot.ActionBar.SetEntity(posInfo.Entity);
            //slot.transform.SetParent(Instance._leftSideSlotsContent);
            //slot.transform.localScale = Vector3.one;
            //Instance._leftSlots.Add(slot);
            var slot = Instance.LeftSideCharacterSlots[leftSlotIndex];
            slot.SetEntity(posInfo.Entity);
            slot.Show();
            leftSlotIndex++;

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

            //var slotObj = Instantiate(Instance._characterStateSlotPrefab);
            //var slot = slotObj.GetComponent<CharacterStateSlot>();
            //slot.ActionBar.SetEntity(posInfo.Entity);
            //slot.ActionBar.InvetValue();
            //slot.transform.SetParent(Instance._rightSideSlotsContent);
            //slot.transform.localScale = Vector3.one;
            //Instance._rightSlots.Add(slot);
            var slot = Instance.RightSideCharacterSlots[rightSlotIndex];
            slot.SetEntity(posInfo.Entity);
            slot.Show();
            rightSlotIndex++;
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
                    nextTurnHolder.ResetActionPoints();
                    break;
                }
            }

            yield return Wait.ForEndOfFrame;
        }

        TurnHolder = nextTurnHolder;
        UpdateInterface(TurnHolder);
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

        _move1Text.text = turnHolder.KnownMoves[0] != MovesIDs.NONE ? turnHolder.KnownMoves[0].ToString() : "--";
        _move2Text.text = turnHolder.KnownMoves[1] != MovesIDs.NONE ? turnHolder.KnownMoves[1].ToString() : "--";
        _move3Text.text = turnHolder.KnownMoves[2] != MovesIDs.NONE ? turnHolder.KnownMoves[2].ToString() : "--";
        _move4Text.text = turnHolder.KnownMoves[3] != MovesIDs.NONE ? turnHolder.KnownMoves[3].ToString() : "--";
        _move5Text.text = turnHolder.KnownMoves[4] != MovesIDs.NONE ? turnHolder.KnownMoves[4].ToString() : "--";
        _move6Text.text = turnHolder.KnownMoves[5] != MovesIDs.NONE ? turnHolder.KnownMoves[5].ToString() : "--";
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
