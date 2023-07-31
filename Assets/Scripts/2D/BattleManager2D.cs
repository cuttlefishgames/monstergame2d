using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Utils;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class BattleManager2D : Singleton<BattleManager2D>
{
    private class ArrowData
    {
        public RectTransform ArrowRect;
        public Entity2D Entity;
    }

    public static bool BattleActive { get; private set; }
    public static bool FillingBars { get; private set; }
    public static bool TurnHolderActed { get; private set; }
    public static float FillLimit => Instance._fillLimit;
    public static int BattlefieldLayer => Instance._battlefieldLayer;
    [SerializeField] private int _battlefieldLayer;
    [SerializeField] private int _fillLimit = 2000;
    [SerializeField] private int _speedDamp = 0;
    [SerializeField] private float _fillSpeed = 10;
    [SerializeField] private float _deflayBeforeFillBars = 0.5f;
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
    [SerializeField] private GameObject _targetSelectionPanel;
    [SerializeField] private Button _backFromTargetSelectionMoveBtn;
    [SerializeField] private Button _confirmTargetOfMoveBtn;
    [SerializeField] private TextMeshProUGUI _moveName;
    [SerializeField] private TextMeshProUGUI _moveDescription;
    [SerializeField] private TextMeshProUGUI _movePower;
    [SerializeField] private TextMeshProUGUI _moveCost;
    [SerializeField] private TextMeshProUGUI _moveCategory;
    [SerializeField] private TextMeshProUGUI _moveElement;

    //target arrows
    [SerializeField] private List<RectTransform> _leftSideArrows;
    [SerializeField] private List<RectTransform> _rightSideArrows;
    private static List<ArrowData> _leftArrowsData = new List<ArrowData>();
    private static List<ArrowData> _rightArrowsData = new List<ArrowData>();

    private Coroutine _fillBars;
    private List<CharacterStateSlot> _leftSlots;
    private List<CharacterStateSlot> _rightSlots;

    //event system
    private EventSystem _eventSysten;
    private GameObject _lastMoveBtn;

    public static BattleStates BattleState { get; private set; } = BattleStates.INACTIVE;
    public static Entity2D TurnHolder { get; private set; }
    public static MovesIDs CurrentSelectedMove { get; private set; }
    private static BattleEvent _currentMoveEvent;

    private static Queue<BattleEvent> BattleEvents;
    private static BattleEvent _currentEvent;

    protected override void Awake()
    {
        base.Awake();

        _leftArrowsData = new List<ArrowData>();
        foreach (var arrow in _leftSideArrows)
        {
            _leftArrowsData.Add(new ArrowData() { ArrowRect = arrow });
        }

        _rightArrowsData = new List<ArrowData>();
        foreach (var arrow in _rightSideArrows)
        {
            _rightArrowsData.Add(new ArrowData() { ArrowRect = arrow });
        }

        _lastMoveBtn = _move1Button.gameObject;

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
        _move1Button.onClick.AddListener(() => { _lastMoveBtn = _move1Button.gameObject; SelectMove(0); });
        _move2Button.onClick.AddListener(() => { _lastMoveBtn = _move2Button.gameObject; SelectMove(1); });
        _move3Button.onClick.AddListener(() => { _lastMoveBtn = _move3Button.gameObject; SelectMove(2); });
        _move4Button.onClick.AddListener(() => { _lastMoveBtn = _move4Button.gameObject; SelectMove(3); });
        _move5Button.onClick.AddListener(() => { _lastMoveBtn = _move5Button.gameObject; SelectMove(4); });
        _move6Button.onClick.AddListener(() => { _lastMoveBtn = _move6Button.gameObject; SelectMove(5); });

        _backFromMovesButton.onClick.RemoveAllListeners();
        _backFromMovesButton.onClick.AddListener(BackFromMoves);

        //move target selection
        _backFromTargetSelectionMoveBtn.onClick.RemoveAllListeners();
        _backFromTargetSelectionMoveBtn.onClick.AddListener(() => 
        {
            SetBattleState(BattleStates.MOVES_MENU);
        });

        _confirmTargetOfMoveBtn.onClick.RemoveAllListeners();
        _confirmTargetOfMoveBtn.onClick.AddListener(() =>
        {
            if (TurnHolder.Side == TeamSides.LEFT)
            {
                SetupCurrentMove(GetRandomTarget(TeamSides.RIGHT));
            }
            else
            {
                SetupCurrentMove(GetSelectedTargets());
            }
        });

        DisposeBattleEvents();
    }

    private void Update()
    {
        if (!BattleActive)
        {
            return;
        }

        if (_currentEvent != null && _currentEvent.Resolved)
        {
            _currentEvent.Dispose();
            _currentEvent = null;
        }

        if (BattleEvents.Count > 0 && _currentEvent == null)
        {
            _currentEvent = BattleEvents.Dequeue();
            if (_currentEvent.Prepare())
            {
                _currentEvent.Resolve();
            }
        }

        if (TurnHolder != null && _currentEvent == null && TurnHolderActed)
        {
            //end turn
            SetBattleState(BattleStates.FILLING_BARS);
        }
    }

    public static void SetBattleState(BattleStates state)
    {
        BattleState = state;

        switch (BattleState) 
        {
            case BattleStates.INACTIVE:

                Instance._battleMenuCanvas.SetActive(false);
                break;

            case BattleStates.FILLING_BARS:

                Instance._battleMenuCanvas.SetActive(false);
                var allSlots = Instance.LeftSideCharacterSlots.Concat(Instance.RightSideCharacterSlots).ToList();
                allSlots.ForEach(s => s.SetState(CharacterSlotStates.IDLE));
                FillBars();
                break;

            case BattleStates.BATTLE_MENU:

                var slots = Instance.LeftSideCharacterSlots.Concat(Instance.RightSideCharacterSlots).ToList();
                var slot = slots.Where(slot => slot.Entity == TurnHolder).FirstOrDefault();
                if (slot != null)
                {
                    slot.SetState(CharacterSlotStates.HIGHLIGHT);
                }           

                Instance._battleMenuCanvas.SetActive(true);
                Instance._battleMenuPanel.SetActive(true);
                Instance._movesPanel.SetActive(false);
                Instance._targetSelectionPanel.SetActive(false);
                Instance._eventSysten.SetSelectedGameObject(null);
                Instance._eventSysten.SetSelectedGameObject(Instance._movesButton.gameObject);

                //hide menu if it is an enemy
                if (TurnHolder.Side == TeamSides.RIGHT)
                {
                    Instance._battleMenuCanvas.SetActive(false);
                    SetBattleState(BattleStates.MOVES_MENU);
                }

                break;
            case BattleStates.MOVES_MENU:

                DownlightArrows();
                DisposeCurrentBattleEvent();
                CurrentSelectedMove = MovesIDs.NONE;

                Instance._battleMenuCanvas.SetActive(true);
                Instance._battleMenuPanel.SetActive(false);
                Instance._movesPanel.SetActive(true);
                Instance._targetSelectionPanel.SetActive(false);
                Instance._eventSysten.SetSelectedGameObject(null);
                Instance._eventSysten.SetSelectedGameObject(Instance._lastMoveBtn.gameObject);

                Instance._move1Text.text = TurnHolder.KnownMoves[0] != MovesIDs.NONE ? TurnHolder.KnownMoves[0].ToString() : "--";
                Instance._move2Text.text = TurnHolder.KnownMoves[1] != MovesIDs.NONE ? TurnHolder.KnownMoves[1].ToString() : "--";
                Instance._move3Text.text = TurnHolder.KnownMoves[2] != MovesIDs.NONE ? TurnHolder.KnownMoves[2].ToString() : "--";
                Instance._move4Text.text = TurnHolder.KnownMoves[3] != MovesIDs.NONE ? TurnHolder.KnownMoves[3].ToString() : "--";
                Instance._move5Text.text = TurnHolder.KnownMoves[4] != MovesIDs.NONE ? TurnHolder.KnownMoves[4].ToString() : "--";
                Instance._move6Text.text = TurnHolder.KnownMoves[5] != MovesIDs.NONE ? TurnHolder.KnownMoves[5].ToString() : "--";

                //hide menu if it is an enemy
                if (TurnHolder.Side == TeamSides.RIGHT)
                {
                    Instance._battleMenuCanvas.SetActive(false);
                    var moves = TurnHolder.KnownMoves.Where(m => m != MovesIDs.NONE).ToList();
                    var random = new System.Random();
                    var move = moves[random.Next(moves.Count)];
                    var moveIndex = TurnHolder.KnownMoves.IndexOf(move);
                    SelectMove(moveIndex);
                }

                break;
            case BattleStates.TARGET_SELECTION:

                Instance._battleMenuCanvas.SetActive(true);
                Instance._battleMenuPanel.SetActive(false);
                Instance._movesPanel.SetActive(false);
                Instance._targetSelectionPanel.SetActive(true);
                Instance._eventSysten.SetSelectedGameObject(null);
                Instance._eventSysten.SetSelectedGameObject(Instance._confirmTargetOfMoveBtn.gameObject);
                Instance._confirmTargetOfMoveBtn.Select();
                Instance._confirmTargetOfMoveBtn.OnSelect(null);

                //hide menu if it is an enemy
                if (TurnHolder.Side == TeamSides.RIGHT)
                {
                    Instance._battleMenuCanvas.SetActive(false);
                    SetupCurrentMove(GetRandomTarget(TeamSides.LEFT));
                    return;
                }

                var moveData = MonstersManager.MovesResources.Resources.Where(m => m.moveID == CurrentSelectedMove).FirstOrDefault();

                if (moveData != null)
                {
                    var moveObjectInstance = Instantiate(moveData.movePrefab);
                    var order = moveObjectInstance.GetComponent<BattleOrder2D>();
                    order.SetCaster(TurnHolder);
                    Entity2D target = null;
                    switch (TurnHolder.Side)
                    {
                        case TeamSides.LEFT:
                            target = Battlefield2D.GetAllCharacterSlotsOfSide(TeamSides.RIGHT).Select(s => s.Entity).FirstOrDefault();
                            break;
                        case TeamSides.RIGHT:
                            target = Battlefield2D.GetAllCharacterSlotsOfSide(TeamSides.LEFT).Select(s => s.Entity).FirstOrDefault();
                            break;
                    }

                    //order.SetTargets(new List<Entity2D> { target });
                    _currentMoveEvent = order;
                    //order.Execute();
                    //BattleEvents.Enqueue(order);
                    //}

                    Instance._moveName.text = moveData.moveName;
                    Instance._moveDescription.text = moveData.moveDescription;
                    Instance._movePower.text = "Power: 100";
                    Instance._moveCost.text = "Energy: 100";
                    Instance._moveCategory.text = "Cat.: Magical";
                    Instance._moveElement.text = "Ele.: Pyro";

                    HighlightArrows(TurnHolder, target, moveData.target);
                }

                break;
            case BattleStates.RESOLVING_EVENTS:
                DownlightArrows();
                Instance._battleMenuCanvas.SetActive(false);
                break;
        }
    }

    public static void SetBattleActive(bool active)
    {
        BattleActive = active;
    }

    private static void DownlightArrows()
    {
        _leftArrowsData.ForEach(a => a.ArrowRect.gameObject.SetActive(false));
        _rightArrowsData.ForEach(a => a.ArrowRect.gameObject.SetActive(false));
    }

    private static void HighlightArrows(Entity2D caster, Entity2D defaultTarget, MovesTargets targetType)
    {
        DownlightArrows();
        List<ArrowData> arrows = new List<ArrowData>();
        var allArrows = _leftArrowsData.Concat(_rightArrowsData).ToList();

        switch (targetType)
        {
            case MovesTargets.SELF:
                arrows.Add(allArrows.Where(a => a.Entity == caster).FirstOrDefault());
                break;
            case MovesTargets.ALL:
                arrows = allArrows;
                break;
            case MovesTargets.ALL_MINUS_SELF:
                arrows = allArrows.Where(a => a.Entity != caster).ToList();
                break;
            case MovesTargets.ALL_ALLIES:
                if (caster.Side == TeamSides.LEFT)
                {
                    arrows = _leftArrowsData;
                }
                else
                {
                    arrows = _rightArrowsData;
                }
                break;
            case MovesTargets.ALL_ALLIES_MINUS_SELF:
                if (caster.Side == TeamSides.LEFT)
                {
                    arrows = _leftArrowsData.Where(a => a.Entity != caster).ToList();
                }
                else
                {
                    arrows = _rightArrowsData.Where(a => a.Entity != caster).ToList();
                }
                break;
            case MovesTargets.ENEMY:
                arrows.Add(allArrows.Where(a => a.Entity = defaultTarget).FirstOrDefault());
                break;
            case MovesTargets.ALL_ENEMIES:
                if (caster.Side == TeamSides.LEFT)
                {
                    arrows = _rightArrowsData;
                }
                else
                {
                    arrows = _leftArrowsData;
                }
                break;
        }

        var canvasRect = Instance._battleMenuCanvas.GetComponent<RectTransform>();
        foreach (var arrow in arrows)
        {
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(arrow.Entity.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the ui element
            arrow.ArrowRect.anchoredPosition = WorldObject_ScreenPosition;
            arrow.ArrowRect.gameObject.SetActive(true);
        }
    }

    private static void DisposeBattleEvents()
    {
        if (BattleEvents != null)
        {
            while (BattleEvents.Count > 0)
            {
                var disposedEvent = BattleEvents.Dequeue();
                disposedEvent.Dispose();
            }

            BattleEvents.Clear();
        }

        BattleEvents = new Queue<BattleEvent>();
    }

    private static List<Entity2D> GetRandomTarget(TeamSides side)
    {
        var random = new System.Random();

        switch (side)
        {
            case TeamSides.LEFT:
                var possibleLeftTargets = _leftArrowsData.Where(a => a.Entity != null).ToList();
                return new List<Entity2D> { possibleLeftTargets[random.Next(possibleLeftTargets.Count)].Entity };
            case TeamSides.RIGHT:
                var possibleRighTargets = _rightArrowsData.Where(a => a.Entity != null).ToList();
                return new List<Entity2D> { possibleRighTargets[random.Next(possibleRighTargets.Count)].Entity };
            default:
                return new List<Entity2D>();
        }
    }

    private static List<Entity2D> GetSelectedTargets()
    {
        return _leftArrowsData.Concat(_rightArrowsData).
            Where(a => a.ArrowRect.gameObject.activeSelf).Select(a => a.Entity).ToList();
    }

    private static void SetupCurrentMove(List<Entity2D> targets)
    {
        TurnHolderActed = true;
        _currentMoveEvent.SetTargets(targets);
        BattleEvents.Enqueue(_currentMoveEvent);
        SetBattleState(BattleStates.RESOLVING_EVENTS);
    }

    private static void DisposeCurrentBattleEvent()
    {
        if (_currentMoveEvent != null)
        {
            _currentMoveEvent.Discard();
            _currentMoveEvent.Dispose();
        }
    }

    private static void SelectMove(int moveSlot)
    {
        var move = TurnHolder.KnownMoves[moveSlot];
        if (move == MovesIDs.NONE)
            return;

        CurrentSelectedMove = move;
        SetBattleState(BattleStates.TARGET_SELECTION);
    }

    private void BackFromMoves()
    {
        SetBattleState(BattleStates.BATTLE_MENU);
    }

    private void Moves()
    {
        SetBattleState(BattleStates.MOVES_MENU);
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
        DisposeBattleEvents();
        Instance._eventSysten = EventSystem.current;

        var leftSidePositions = Battlefield2D.GetAllCharacterSlotsOfSide(TeamSides.LEFT);
        var rightSidePositions = Battlefield2D.GetAllCharacterSlotsOfSide(TeamSides.RIGHT);
        int leftSlotIndex = 0;
        int rightSlotIndex = 0;

        foreach (var posInfo in leftSidePositions)
        {
            if (posInfo.Entity as UnityEngine.Object == null)
                continue;

            var slot = Instance.LeftSideCharacterSlots[leftSlotIndex];
            slot.SetEntity(posInfo.Entity);
            slot.SetState(CharacterSlotStates.IDLE);
            slot.Show();
            _leftArrowsData[leftSlotIndex].Entity = posInfo.Entity;
            leftSlotIndex++;
        }

        foreach (var posInfo in rightSidePositions)
        {
            if (posInfo.Entity as UnityEngine.Object == null)
                continue;

            //var sorter = posInfo.Entity.Root.GetComponentInChildren<SortingHelper>();
            //if (sorter != null)
            //{
            //    sorter.Group.sortingLayerName = LayerMask.LayerToName(BattlefieldLayer);
            //}

            //var slotObj = Instantiate(Instance._characterStateSlotPrefab);
            //var slot = slotObj.GetComponent<CharacterStateSlot>();
            //slot.ActionBar.SetEntity(posInfo.Entity);
            //slot.ActionBar.InvetValue();
            //slot.transform.SetParent(Instance._rightSideSlotsContent);
            //slot.transform.localScale = Vector3.one;
            //Instance._rightSlots.Add(slot);

            var slot = Instance.RightSideCharacterSlots[rightSlotIndex];
            slot.SetEntity(posInfo.Entity);
            slot.SetState(CharacterSlotStates.IDLE);
            slot.Show();
            _rightArrowsData[rightSlotIndex].Entity = posInfo.Entity;
            rightSlotIndex++;
        }

        Instance._canvas.SetActive(true);
        SetBattleState(BattleStates.INACTIVE);
    }

    public static void FillBars()
    {
        if (Instance._fillBars != null)
        {
            Instance.StopCoroutine(Instance._fillBars);
        }

        Instance._fillBars = Instance.StartCoroutine(Instance.FillBarsCoroutine());
        FillingBars = true;
        TurnHolder = null;
        TurnHolderActed = false;
    }

    IEnumerator FillBarsCoroutine()
    {
        yield return new WaitForSeconds(_deflayBeforeFillBars);

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
        FillingBars = false;
        SetBattleState(BattleStates.BATTLE_MENU);
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
