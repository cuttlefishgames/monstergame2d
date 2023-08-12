using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Utils;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BattleManager2D : Singleton<BattleManager2D>
{
    [Serializable]
    private class ArrowData
    {
        public RectTransform ArrowRect;
        [HideInInspector] public Entity2D Entity;
        public ArrowData UP;
        public ArrowData Down;
        public ArrowData Left;
        public ArrowData Right;
        public TeamSides Side;

        public void Highlight()
        {
            if (Entity == null)
            {
                return;
            }

            var canvasRect = CanvasRect;
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(Entity.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the ui element
            ArrowRect.anchoredPosition = WorldObject_ScreenPosition;
            ArrowRect.gameObject.SetActive(true);
        }

        public void Downlight()
        {
            ArrowRect.gameObject.SetActive(false);
        }

        public void Clear()
        {
            Entity = null;
        }
    }

    public static bool BattleActive { get; private set; }
    public static bool FillingBars { get; private set; }
    public static bool TurnHolderActed { get; private set; }
    public static float FillLimit => Instance._fillLimit;
    public static int BattlefieldLayer => Instance._battlefieldLayer;
    [SerializeField] private bool _controlEnemiesToo = false;
    public static bool ControlEnemiesToo => Instance._controlEnemiesToo;
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
    [SerializeField] private ArrowData _leftArrow1;
    [SerializeField] private ArrowData _leftArrow2;
    [SerializeField] private ArrowData _leftArrow3;
    [SerializeField] private ArrowData _leftArrow4;
    [SerializeField] private ArrowData _rightArrow1;
    [SerializeField] private ArrowData _rightArrow2;
    [SerializeField] private ArrowData _rightArrow3;
    [SerializeField] private ArrowData _rightArrow4;
    private static List<ArrowData> _arrowsData;
    private static ArrowData _currentArrowData;

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
    private static MovesResources.MoveResource _currentMoveData;
    private static Queue<BattleEvent> BattleEvents;
    private static BattleEvent _currentEvent;

    public static RectTransform CanvasRect { get; private set; }

    private BattleUIControls _uiControls;

    protected override void Awake()
    {
        base.Awake();

        CanvasRect = _battleMenuCanvas.GetComponent<RectTransform>();

        _uiControls = new BattleUIControls();
        _uiControls.BattleUINavigation.MoveUp.performed += ctx => UIMove(Directions.UP);
        _uiControls.BattleUINavigation.MoveDown.performed += ctx => UIMove(Directions.DOWN);
        _uiControls.BattleUINavigation.MoveLeft.performed += ctx => UIMove(Directions.LEFT);
        _uiControls.BattleUINavigation.MoveRight.performed += ctx => UIMove(Directions.RIGHT);
        _uiControls.BattleUINavigation.LeftPress.performed += ctx => UILeftPress();

        _uiControls.BattleUINavigation.RotateUp.performed += ctx => UIRotate(Directions.UP);
        _uiControls.BattleUINavigation.RotateDown.performed += ctx => UIRotate(Directions.DOWN);
        _uiControls.BattleUINavigation.RotateLeft.performed += ctx => UIRotate(Directions.LEFT);
        _uiControls.BattleUINavigation.RotateRight.performed += ctx => UIRotate(Directions.RIGHT);
        _uiControls.BattleUINavigation.PressRight.performed += ctx => UIRightPress();

        _uiControls.BattleUINavigation.Confirm.performed += ctx => UIConfirm();
        _uiControls.BattleUINavigation.Cancel.performed += ctx => UICancel();
        _uiControls.BattleUINavigation.Check.performed += ctx => UICheck();
        _uiControls.BattleUINavigation.Switch.performed += ctx => UISwitch();

        _arrowsData = new List<ArrowData>();
        _arrowsData.Add(_leftArrow1);
        _arrowsData.Add(_leftArrow2);
        _arrowsData.Add(_leftArrow3);
        _arrowsData.Add(_leftArrow4);
        _arrowsData.Add(_rightArrow1);
        _arrowsData.Add(_rightArrow2);
        _arrowsData.Add(_rightArrow3);
        _arrowsData.Add(_rightArrow4);

        //navigation left side
        _arrowsData[0].UP = _arrowsData[2];
        _arrowsData[0].Down = _arrowsData[1];
        _arrowsData[0].Left = _arrowsData[6];
        _arrowsData[0].Right = _arrowsData[6];
        _arrowsData[1].UP = _arrowsData[0];
        _arrowsData[1].Down = _arrowsData[3];
        _arrowsData[1].Left = _arrowsData[5];
        _arrowsData[1].Right = _arrowsData[5];
        _arrowsData[2].UP = _arrowsData[3];
        _arrowsData[2].Down = _arrowsData[0];
        _arrowsData[2].Left = _arrowsData[6];
        _arrowsData[2].Right = _arrowsData[6];
        _arrowsData[3].UP = _arrowsData[1];
        _arrowsData[3].Down = _arrowsData[2];
        _arrowsData[3].Left = _arrowsData[7];
        _arrowsData[3].Right = _arrowsData[7];
        //navigation right side
        _arrowsData[4].UP = _arrowsData[6];
        _arrowsData[4].Down = _arrowsData[5];
        _arrowsData[4].Left = _arrowsData[0];
        _arrowsData[4].Right = _arrowsData[0];
        _arrowsData[5].UP = _arrowsData[4];
        _arrowsData[5].Down = _arrowsData[7];
        _arrowsData[5].Left = _arrowsData[1];
        _arrowsData[5].Right = _arrowsData[1];
        _arrowsData[6].UP = _arrowsData[7];
        _arrowsData[6].Down = _arrowsData[4];
        _arrowsData[6].Left = _arrowsData[2];
        _arrowsData[6].Right = _arrowsData[2];
        _arrowsData[7].UP = _arrowsData[5];
        _arrowsData[7].Down = _arrowsData[6];
        _arrowsData[7].Left = _arrowsData[3];
        _arrowsData[7].Right = _arrowsData[3];

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
            SetupCurrentMove(GetSelectedTargets());
            //if (TurnHolder.Side == TeamSides.LEFT)
            //{
            //    SetupCurrentMove(GetRandomTarget(TeamSides.RIGHT));
            //}
            //else
            //{
            //    SetupCurrentMove(GetSelectedTargets());
            //}
        });

        DisposeBattleEvents();
    }

    private void OnEnable()
    {
        _uiControls.BattleUINavigation.Enable();
    }

    private void OnDisable()
    {
        _uiControls.BattleUINavigation.Disable();
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

        if (BattleState == BattleStates.TARGET_SELECTION)
        {
            //if(Input.GetKeyDown(KeyCode.))
        }
    }
    
    private void CalcDamage(Entity2D caster, Entity2D target, float power, Stats offensiveStat, Stats defensiveStat, DamageTypes damageType)
    {
        //var casterStats = caster.BaseStats
    }

    

    private void UIMove(Directions direction)
    {

    }

    private void UILeftPress()
    {

    }

    private void UIRotate(Directions direction)
    {
        if (BattleState == BattleStates.TARGET_SELECTION)
        {
            if (_currentMoveEvent != null && _currentMoveData != null && TurnHolder != null)
            {
                DownlightArrows();
                ArrowData next = _currentArrowData;
                switch (_currentMoveData.target)
                {
                    case MovesTargets.SELF:
                        //highlight self arrow
                        _arrowsData.Where(a => a.Entity == TurnHolder).FirstOrDefault().Highlight();
                        break;
                    case MovesTargets.ALL:
                        //hightlight all arrows
                        _arrowsData.ForEach(a => a.Highlight());
                        break;
                    case MovesTargets.ALL_MINUS_SELF:
                        //hightlight all allies minus caster arrows
                        _arrowsData.Where(a => a.Entity != TurnHolder).ToList().ForEach(a => a.Highlight());
                        break;
                    case MovesTargets.ALL_ALLIES:
                        //hightlight all allies arrows
                        _arrowsData.Where(a => a.Side == TurnHolder.Side).ToList().ForEach(a => a.Highlight());
                        break;
                    case MovesTargets.ALL_ALLIES_MINUS_SELF:
                        //hightlight all allies minus caster arrows
                        _arrowsData.Where(a => a.Side == TurnHolder.Side && a.Entity != TurnHolder).ToList().ForEach(a => a.Highlight());
                        break;
                    case MovesTargets.ALLY:
                        //hightlight single ally
                        switch (direction)
                        {
                            case Directions.UP:
                                do
                                {
                                    next = next.UP;
                                } while (next.Entity == null);
                                _currentArrowData = next;
                                break;
                            case Directions.DOWN:
                                do
                                {
                                    next = next.Down;
                                } while (next.Entity == null);
                                _currentArrowData = next;
                                break;
                        }
                        _currentArrowData.Highlight();
                        break;
                    case MovesTargets.ENEMY:
                        //hightlight single enemy
                        switch (direction)
                        {
                            case Directions.UP:
                                do
                                {
                                    next = next.UP;
                                } while (next.Entity == null);
                                _currentArrowData = next;
                                break;
                            case Directions.DOWN:
                                do
                                {
                                    next = next.Down;
                                } while (next.Entity == null);
                                _currentArrowData = next;
                                break;
                        }
                        _currentArrowData.Highlight();
                        break;
                    case MovesTargets.ANY:
                        //hightlight a single entity
                        switch (direction)
                        {
                            case Directions.UP:
                                do
                                {
                                    next = next.UP;
                                } while (next.Entity == null);
                                _currentArrowData = next;
                                break;
                            case Directions.DOWN:
                                do
                                {
                                    next = next.Down;
                                } while (next.Entity == null);
                                _currentArrowData = next;
                                break;
                            case Directions.LEFT:
                                do
                                {
                                    next = next.Left;
                                } while (next.Entity == null);
                                _currentArrowData = next;
                                break;
                            case Directions.RIGHT:
                                do
                                {
                                    next = next.Right;
                                } while (next.Entity == null);
                                _currentArrowData = next;
                                break;
                        }
                        _currentArrowData.Highlight();
                        break;
                    case MovesTargets.ALL_ENEMIES:
                        //hightlight all enemies
                        _arrowsData.Where(a => a.Side != TurnHolder.Side).ToList().ForEach(a => a.Highlight());
                        break;
                }
            }
        }
    }

    private void UIRightPress()
    {

    }

    private void UIConfirm()
    {

    }

    private void UICancel()
    {
        switch (BattleState)
        {
            case BattleStates.INACTIVE:
                break;
            case BattleStates.FILLING_BARS:
                break;
            case BattleStates.RESOLVING_EVENTS:
                break;
            case BattleStates.BATTLE_MENU:
                break;
            case BattleStates.MOVES_MENU:
                SetBattleState(BattleStates.BATTLE_MENU);
                break;
            case BattleStates.TARGET_SELECTION:
                SetBattleState(BattleStates.MOVES_MENU);
                break;
        }
    }

    private void UICheck()
    {

    }

    private void UISwitch()
    {

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

                if (!ControlEnemiesToo)
                {
                    //hide menu if it is an enemy
                    if (TurnHolder.Side == TeamSides.RIGHT)
                    {
                        Instance._battleMenuCanvas.SetActive(false);
                        SetBattleState(BattleStates.MOVES_MENU);
                    }
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
                if (!ControlEnemiesToo)
                {
                    if (TurnHolder.Side == TeamSides.RIGHT)
                    {
                        Instance._battleMenuCanvas.SetActive(false);
                        var moves = TurnHolder.KnownMoves.Where(m => m != MovesIDs.NONE).ToList();
                        var random = new System.Random();
                        var move = moves[random.Next(moves.Count)];
                        var moveIndex = TurnHolder.KnownMoves.IndexOf(move);
                        SelectMove(moveIndex);
                    }
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
                //if (TurnHolder.Side == TeamSides.RIGHT)
                //{
                //    Instance._battleMenuCanvas.SetActive(false);
                //    SetupCurrentMove(GetRandomTarget(TeamSides.LEFT));
                //    return;
                //}

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
                            _currentArrowData = _arrowsData.Where(a => a.Entity == target).FirstOrDefault();
                            break;
                        case TeamSides.RIGHT:
                            target = Battlefield2D.GetAllCharacterSlotsOfSide(TeamSides.LEFT).Select(s => s.Entity).FirstOrDefault();
                            _currentArrowData = _arrowsData.Where(a => a.Entity == target).FirstOrDefault();
                            break;
                    }

                    //order.SetTargets(new List<Entity2D> { target });
                    _currentMoveEvent = order;
                    _currentMoveData = moveData;
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

                if (!ControlEnemiesToo)
                {
                    if (TurnHolder.Side == TeamSides.RIGHT)
                    {
                        Instance._battleMenuCanvas.SetActive(false);
                        SetupCurrentMove(GetSelectedTargets());
                    }
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
        _arrowsData.ForEach(a => a.Downlight());
    }

    private static void HighlightArrows(Entity2D caster, Entity2D defaultTarget, MovesTargets targetType)
    {
        DownlightArrows();
        List<ArrowData> arrows = new List<ArrowData>();

        switch (targetType)
        {
            case MovesTargets.SELF:
                arrows.Add(_arrowsData.Where(a => a.Entity == caster).FirstOrDefault());
                break;
            case MovesTargets.ALL:
                arrows = _arrowsData;
                break;
            case MovesTargets.ALL_MINUS_SELF:
                arrows = _arrowsData.Where(a => a.Entity != caster).ToList();
                break;
            case MovesTargets.ALLY:
                arrows.Add(_arrowsData.Where(a => a.Entity == caster).FirstOrDefault());
                break;
            case MovesTargets.ALL_ALLIES:
                if (caster.Side == TeamSides.LEFT)
                {
                    arrows = _arrowsData.Where(a => a.Side == TeamSides.LEFT).ToList();
                }
                else
                {
                    arrows = _arrowsData.Where(a => a.Side == TeamSides.RIGHT).ToList();
                }
                break;
            case MovesTargets.ALL_ALLIES_MINUS_SELF:
                if (caster.Side == TeamSides.LEFT)
                {
                    arrows = _arrowsData.Where(a => a.Side == TeamSides.LEFT && a.Entity != caster).ToList();
                }
                else
                {
                    arrows = _arrowsData.Where(a => a.Side == TeamSides.RIGHT && a.Entity != caster).ToList();
                }
                break;
            case MovesTargets.ENEMY:
                arrows.Add(_arrowsData.Where(a => a.Entity == defaultTarget).FirstOrDefault());
                break;
            case MovesTargets.ANY:
                arrows.Add(_arrowsData.Where(a => a.Entity == defaultTarget).FirstOrDefault());
                break;
            case MovesTargets.ALL_ENEMIES:
                if (caster.Side == TeamSides.LEFT)
                {
                    arrows = _arrowsData.Where(a => a.Side == TeamSides.RIGHT).ToList();
                }
                else
                {
                    arrows = _arrowsData.Where(a => a.Side == TeamSides.LEFT).ToList();
                }
                break;
        }

        arrows.ForEach(a => a.Highlight());
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
                var possibleLeftTargets = _arrowsData.Where(a => a.Side == TeamSides.RIGHT && a.Entity != null).ToList();
                return new List<Entity2D> { possibleLeftTargets[random.Next(possibleLeftTargets.Count)].Entity };
            case TeamSides.RIGHT:
                var possibleRighTargets = _arrowsData.Where(a => a.Side == TeamSides.LEFT && a.Entity != null).ToList();
                return new List<Entity2D> { possibleRighTargets[random.Next(possibleRighTargets.Count)].Entity };
            default:
                return new List<Entity2D>();
        }
    }

    private static List<Entity2D> GetSelectedTargets()
    {
        return _arrowsData.
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

        if (_currentMoveData != null)
        {
            _currentMoveData = null;
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
            _arrowsData[leftSlotIndex].Entity = posInfo.Entity;
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
            _arrowsData[rightSlotIndex + 4].Entity = posInfo.Entity;
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
                var fill = Mathf.CeilToInt(_fillSpeed * Time.deltaTime * (ent.BaseStats.BaseSpeed + _speedDamp));
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
