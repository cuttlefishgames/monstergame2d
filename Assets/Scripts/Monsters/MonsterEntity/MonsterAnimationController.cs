using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public enum MonsterAnimationStates 
    { 
        IDLE = 0,
        PAIN = 10,
        PAIN_LOOP = 11,
        HOP_IN = 20,
        HOP_OUT = 30,
        MELEE_ATTACK = 40,
        MELEE_ATTACK_LOOP = 41,
        CAST_ATTACK = 50,
        CAST_ATTACK_LOOP = 51,
        FAINT = 60,
        SPAWN = 70,
        IDLE_INJURED = 80,
        DISABLED = 90,
        SPECIAL_MOVE = 100,
        VICTORY_POSE = 110,
        START_WALK = 119,
        WALK = 120,
        START_RUN = 129,
        RUN = 130
    }

    public static class AnimationBlendDurations
    {
        public const float PAIN_ANIMATION_BLEND_DURATION = 0.1f;
    }

    public class MonsterAnimationController : MonsterComponent
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Material _textureMaterial;
        //[SerializeField] private Animation _animation;

        public MonsterAnimationStates State { get; private set; } = MonsterAnimationStates.IDLE;

        //parameters names
        public static readonly string STATE_PARAMETER = "State";
        public static readonly string PAIN_LOOP_PARAMETER = "PainLoop";
        public static readonly string MELEE_LOOP_PARAMETER = "MeleeLoop";
        public static readonly string CAST_LOOP_PARAMETER = "CastLoop";

        //animation names
        public static readonly string IDLE_ANIMATION_NAME = "Idle";
        public static readonly string PAIN_ANIMATION_NAME = "Pain";
        public static readonly string HOP_IN_ANIMATION_NAME = "HopIn";
        public static readonly string HOP_OUT_ANIMATION_NAME = "HopOut";
        public static readonly string MELEE_ATTACK_ANIMATION_NAME = "MeleeAttack";
        //public static readonly string MELEE_ATTACK_LOOP_ANIMATION_NAME = "MeleeLoop";
        public static readonly string CAST_ATTACK_ANIMATION_NAME = "CastAttack";
        public static readonly string CAST_ATTACK_LOOP_ANIMATION_NAME = "CastLoop";
        public static readonly string FAINT_ANIMATION_NAME = "Faint";
        public static readonly string SPAWN_ANIMATION_NAME = "Spawn";
        public static readonly string IDLE_INJURED_ANIMATION_NAME = "IdleInjured";
        public static readonly string DISABLED_ANIMATION_NAME = "Disabled";
        public static readonly string SPECIAL_MOVE_ANIMATION_NAME = "SpecialMove";
        public static readonly string VICTORY_POSE_ANIMATION_NAME = "VictoryPose";
        public static readonly string START_WALK_ANIMATION_NAME = "StartWalk";
        public static readonly string WALK_ANIMATION_NAME = "Walk";
        public static readonly string START_RUN_ANIMATION_NAME = "StartRun";
        public static readonly string RUN_ANIMATION_NAME = "Run";

        private static Dictionary<string, bool> AnimationNamesReferece
        {
            get
            {
                var animationNamesReference = new Dictionary<string, bool>();

                animationNamesReference.Add(IDLE_ANIMATION_NAME, true);
                animationNamesReference.Add(PAIN_ANIMATION_NAME, true);
                animationNamesReference.Add(HOP_IN_ANIMATION_NAME, true);
                animationNamesReference.Add(HOP_OUT_ANIMATION_NAME, true);
                animationNamesReference.Add(MELEE_ATTACK_ANIMATION_NAME, true);
                //animationNamesReference.Add(MELEE_ATTACK_LOOP_ANIMATION_NAME, true);
                animationNamesReference.Add(CAST_ATTACK_ANIMATION_NAME, true);
                animationNamesReference.Add(CAST_ATTACK_LOOP_ANIMATION_NAME, true);
                animationNamesReference.Add(FAINT_ANIMATION_NAME, true);
                animationNamesReference.Add(SPAWN_ANIMATION_NAME, true);
                animationNamesReference.Add(IDLE_INJURED_ANIMATION_NAME, true);
                animationNamesReference.Add(DISABLED_ANIMATION_NAME, true);
                animationNamesReference.Add(SPECIAL_MOVE_ANIMATION_NAME, true);
                animationNamesReference.Add(VICTORY_POSE_ANIMATION_NAME, true);
                animationNamesReference.Add(START_WALK_ANIMATION_NAME, true);
                animationNamesReference.Add(WALK_ANIMATION_NAME, true);
                animationNamesReference.Add(START_RUN_ANIMATION_NAME, true);
                animationNamesReference.Add(RUN_ANIMATION_NAME, true);

                return animationNamesReference;
            }
        }

        private Dictionary<string, AnimationClip> _animationClips;
        public Dictionary<string, AnimationClip> AnimationsClips { get => _animationClips; }

        //private void Awake()
        //{
        //    if(_animator == null)
        //    {
        //        var animator = GetComponent<Animator>();
        //        if(animator != null)
        //        {
        //            _animator = animator;
        //        }
        //    }
        //}

        //private void OnValidate()
        //{
        //    var namesReference = AnimationNamesReferece;
        //    foreach(AnimationState animState in _animation)
        //    {
        //        if (!namesReference.ContainsKey(animState.name))
        //        {
        //            Debug.Log("Animation name '" + animState.name + "' does not match for " + ParentMonster.GetGameObject().name);
        //        }
        //    }
        //}

        public override void AttachToMonster(IMonster parent)
        {
            base.AttachToMonster(parent);

            _animator = GetComponent<Animator>();
            if(_animator == null)
            {
                Debug.Log("Failed to get animator for animator controller of " + ParentMonster.GetGameObject().name);
            }

            //_animation = GetComponent<Animation>();
            //if (_animation == null)
            //{
            //    Debug.Log("Failed to get animation for animator controller of " + ParentMonster.GetGameObject().name);
            //}
        }

        //public AnimationClip GetAnimation(string animationName)
        //{
        //    if (_animation.GetClip(animationName) != null)
        //    {
        //        return AnimationsClips[animationName];
        //    }

        //    Debug.LogError("Animation '" + animationName + "' not found for " + ParentMonster.GetGameObject().name);
        //    return null;
        //}

        //public void SetTextureOffset(int offset)
        //{
        //    SetTextureOffset(new Vector2(offset, offset));
        //}

        //public void SetTextureOffset(Vector2 offset)
        //{
        //    _textureMaterial.mainTextureOffset = offset;
        //}

        public void SetState(string state, float fadeLength = 0.25f)
        {
            //_animation.CrossFade(state, fadeLength);
            Debug.Log(state);
            _animator.CrossFade(state, fadeLength);
        }

        public void SetState(MonsterAnimationStates state, float fadeLength = 0.25f)
        {
            if (State == state)
                return;

            //_animator.SetInteger(STATE_PARAMETER, (int)state);
            State = state;

            CancelInvoke("SetWalkLoop");
            CancelInvoke("SetRunLoop");

            float transitionDuration = fadeLength;

            string animation = "";

            switch (state)
            {
                default:
                    animation = IDLE_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.IDLE:
                    animation = IDLE_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.IDLE_INJURED:
                    animation = IDLE_INJURED_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.HOP_IN:
                    animation = HOP_IN_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.HOP_OUT:
                    animation = HOP_OUT_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.MELEE_ATTACK:
                    animation = MELEE_ATTACK_ANIMATION_NAME;
                    break;
                //case MonsterAnimationStates.MELEE_ATTACK_LOOP:
                //    animation = MELEE_ATTACK_LOOP_ANIMATION_NAME;
                //    break;
                case MonsterAnimationStates.CAST_ATTACK:
                    animation = CAST_ATTACK_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.CAST_ATTACK_LOOP:
                    animation = CAST_ATTACK_LOOP_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.PAIN:
                    animation = PAIN_ANIMATION_NAME;
                    transitionDuration = AnimationBlendDurations.PAIN_ANIMATION_BLEND_DURATION;
                    break;
                case MonsterAnimationStates.PAIN_LOOP:
                    animation = PAIN_LOOP_PARAMETER;
                    transitionDuration = AnimationBlendDurations.PAIN_ANIMATION_BLEND_DURATION;
                    break;
                case MonsterAnimationStates.FAINT:
                    animation = FAINT_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.SPECIAL_MOVE:
                    animation = SPECIAL_MOVE_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.SPAWN:
                    animation = SPAWN_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.VICTORY_POSE:
                    animation = VICTORY_POSE_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.DISABLED:
                    animation = DISABLED_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.START_WALK:
                    animation = START_WALK_ANIMATION_NAME;
                    Invoke("SetWalkLoop", .5f);
                    break;
                case MonsterAnimationStates.WALK:
                    animation = WALK_ANIMATION_NAME;
                    break;
                case MonsterAnimationStates.START_RUN:
                    animation = START_RUN_ANIMATION_NAME;
                    Invoke("SetRunLoop", .5f);
                    break;
                case MonsterAnimationStates.RUN:
                    animation = RUN_ANIMATION_NAME;
                    break;
            }

            SetState(animation, transitionDuration);
        }

        private void SetWalkLoop()
        {
            SetState(MonsterAnimationStates.WALK);
        }

        private void SetRunLoop()
        {
            SetState(MonsterAnimationStates.RUN);
        }

        public void SetLoop(string parameter, bool loop)
        {
            //ClearLoops();
            //_animator.SetBool(parameter, loop);
        }

        private void ClearLoops()
        {
            //_animator.SetBool(PAIN_LOOP_PARAMETER, false);
            //_animator.SetBool(MELEE_LOOP_PARAMETER, false);
            //_animator.SetBool(CAST_LOOP_PARAMETER, false);
        }
    }
}