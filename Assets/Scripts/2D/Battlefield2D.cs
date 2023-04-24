using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Utils;
using System.Linq;

public class Battlefield2D : Singleton<Battlefield2D>
{
    public static readonly string LAYER_NAME = "Battlefield";

    //animation names
    public static readonly string HIDE = "Hide";
    private static readonly string HIDDEN = "Hidden";
    public static readonly string SHOW = "Show";
    private static readonly string SHOWING = "Showing";

    public class EntityBattlefieldInfo
    {
        public Entity2D Entity = null;
        public BattlefieldCharacterPositions PositionTag;
        public Transform Transform;

        public bool Available => Entity as UnityEngine.Object == null;

        public void Clear()
        {
            Entity = null;
        }
    }

    public static bool IsShowing { get; private set; } = false;
    public static Dictionary<BattlefieldCharacterPositions, Transform> CharacterPositions { get; private set; }
    public static Dictionary<BattlefieldPositions, Transform> Positions { get; private set; }
    public static Camera BattlefieldCamera => Instance._battlefieldcamera;
    [SerializeField] private Camera _battlefieldcamera;
    [SerializeField] private Animator _animator;
    //[SerializeField] private Animation _animation;
    [SerializeField] private float _animationCrossFadeTime = 0.2f;
    [SerializeField] private Transform _leftTop;
    [SerializeField] private Transform _leftMiddle;
    [SerializeField] private Transform _leftBottom;
    [SerializeField] private Transform _rightTop;
    [SerializeField] private Transform _rightMiddle;
    [SerializeField] private Transform _rightBototm;
    [SerializeField] private Transform _middle;
    [SerializeField] private Transform _middleTop;
    [SerializeField] private Transform _middleBottom;
    [SerializeField] private Transform _frontOfMiddleLeft;
    [SerializeField] private Transform _frontOfMiddleRight;
    [SerializeField] private List<BattlefieldCharacterPositions> _addToTeamPriorityLeft =
        new List<BattlefieldCharacterPositions> { BattlefieldCharacterPositions .LEFT_MIDDLE, BattlefieldCharacterPositions.LEFT_TOP, BattlefieldCharacterPositions.LEFT_BOTTOM };
    [SerializeField] private List<BattlefieldCharacterPositions> _addToTeamPriorityRight =
        new List<BattlefieldCharacterPositions> { BattlefieldCharacterPositions.RIGHT_MIDDLE, BattlefieldCharacterPositions.RIGHT_TOP, BattlefieldCharacterPositions.RIGHT_BOTTOM };
    private List<EntityBattlefieldInfo> _battlefieldInfo;
    private List<EntityBattlefieldInfo> _leftSideInfo;
    private List<EntityBattlefieldInfo> _rightSideInfo;

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
        _animator.keepAnimatorControllerStateOnDisable = true;
        //_animation = GetComponent<Animation>();

        //character positions
        CharacterPositions = new Dictionary<BattlefieldCharacterPositions, Transform>();
        CharacterPositions.Add(BattlefieldCharacterPositions.LEFT_TOP, Instance._leftTop);
        CharacterPositions.Add(BattlefieldCharacterPositions.LEFT_MIDDLE, Instance._leftMiddle);
        CharacterPositions.Add(BattlefieldCharacterPositions.LEFT_BOTTOM, Instance._leftBottom);
        CharacterPositions.Add(BattlefieldCharacterPositions.RIGHT_TOP, Instance._rightTop);
        CharacterPositions.Add(BattlefieldCharacterPositions.RIGHT_MIDDLE, Instance._rightMiddle);
        CharacterPositions.Add(BattlefieldCharacterPositions.RIGHT_BOTTOM, Instance._rightBototm);

        //positions
        Positions = new Dictionary<BattlefieldPositions, Transform>();
        Positions.Add(BattlefieldPositions.MIDDLE, Instance._middle);
        Positions.Add(BattlefieldPositions.MIDDLE_TOP, Instance._middleTop);
        Positions.Add(BattlefieldPositions.MIDDLE_BOTTOM, Instance._middleBottom);
        Positions.Add(BattlefieldPositions.FRONT_MIDDLE_LEFT, Instance._frontOfMiddleLeft);
        Positions.Add(BattlefieldPositions.FRONT_MIDDLE_RIGHT, Instance._frontOfMiddleRight);

        _battlefieldInfo = new List<EntityBattlefieldInfo>();
        _battlefieldInfo.Add(new EntityBattlefieldInfo() { Entity = null, PositionTag = BattlefieldCharacterPositions.LEFT_TOP });
        _battlefieldInfo.Add(new EntityBattlefieldInfo() { Entity = null, PositionTag = BattlefieldCharacterPositions.LEFT_MIDDLE });
        _battlefieldInfo.Add(new EntityBattlefieldInfo() { Entity = null, PositionTag = BattlefieldCharacterPositions.LEFT_BOTTOM });
        _battlefieldInfo.Add(new EntityBattlefieldInfo() { Entity = null, PositionTag = BattlefieldCharacterPositions.RIGHT_TOP });
        _battlefieldInfo.Add(new EntityBattlefieldInfo() { Entity = null, PositionTag = BattlefieldCharacterPositions.RIGHT_MIDDLE });
        _battlefieldInfo.Add(new EntityBattlefieldInfo() { Entity = null, PositionTag = BattlefieldCharacterPositions.RIGHT_BOTTOM });

        _leftSideInfo = new List<EntityBattlefieldInfo>();
        _rightSideInfo = new List<EntityBattlefieldInfo>();
        _leftSideInfo.Add(GetCharacterSlotByTag(BattlefieldCharacterPositions.LEFT_TOP));
        _leftSideInfo.Add(GetCharacterSlotByTag(BattlefieldCharacterPositions.LEFT_MIDDLE));
        _leftSideInfo.Add(GetCharacterSlotByTag(BattlefieldCharacterPositions.LEFT_BOTTOM));
        _rightSideInfo.Add(GetCharacterSlotByTag(BattlefieldCharacterPositions.RIGHT_TOP));
        _rightSideInfo.Add(GetCharacterSlotByTag(BattlefieldCharacterPositions.RIGHT_MIDDLE));
        _rightSideInfo.Add(GetCharacterSlotByTag(BattlefieldCharacterPositions.RIGHT_BOTTOM));
    }

    public static void Show()
    {
        IsShowing = true;
        SetAnimation(SHOW);
    }

    public static void Hide()
    {
        IsShowing = true;
        SetAnimation(HIDE);
    }

    private static void SetAnimation(string animation)
    {
        if(animation == HIDE)
        {
            Instance._animator.CrossFade(HIDE, Instance._animationCrossFadeTime);
            //Instance._animation.CrossFadeQueued(HIDE, Instance._animationCrossFadeTime);
            //Instance._animation.CrossFadeQueued(HIDDEN, Instance._animationCrossFadeTime);
        }
        else if(animation == SHOW)
        {
            Instance._animator.CrossFade(SHOW, 0);
            //Instance._animation.CrossFadeQueued(SHOW, Instance._animationCrossFadeTime);
            //Instance._animation.CrossFadeQueued(SHOWING, Instance._animationCrossFadeTime);
        }
        else
        {
            //Instance._animation.CrossFade(animation, Instance._animationCrossFadeTime);
            Instance._animator.CrossFade(animation, Instance._animationCrossFadeTime);
        }
    }

    public static EntityBattlefieldInfo AddEntity(Entity2D entity, TeamSides side)
    {
        var pos = GetNextFreeCharacterSlotOfSide(side);
        if(pos == null)
        {
            Debug.LogError("No free position found on " + side.ToString());
            return null;
        }

        pos.Entity = entity;
        return pos;
    }

    public static Transform GetPositionByTag(BattlefieldPositions tag)
    {
        return Positions[tag];
    }

    public static EntityBattlefieldInfo GetNextFreeCharacterSlotOfSide(TeamSides side)
    {
        if(side == TeamSides.LEFT)
        {
            foreach (var posTag in Instance._addToTeamPriorityLeft)
            {
                var pos = GetCharacterSlotByTag(posTag);
                if (pos.Available)
                    return pos;
            }
        }
        else
        {
            foreach (var posTag in Instance._addToTeamPriorityRight)
            {
                var pos = GetCharacterSlotByTag(posTag);
                if (pos.Available)
                    return pos;
            }
        }

        return null;
    }

    public static EntityBattlefieldInfo GetCharacterSlotByTag(BattlefieldCharacterPositions tag)
    {
        return Instance._battlefieldInfo.Where(p => p.PositionTag == tag).FirstOrDefault();
    }

    public static List<EntityBattlefieldInfo> GetAllCharacterSlotsOfSide(TeamSides side)
    {
        switch (side)
        {
            case TeamSides.LEFT:
                return Instance._leftSideInfo;
            case TeamSides.RIGHT:
                return Instance._rightSideInfo;
            default:
                return Instance._leftSideInfo;
        }
    }

    public static Transform GetCharacterPositionByTag(BattlefieldCharacterPositions tag)
    {
        return CharacterPositions[tag];
    }
}
