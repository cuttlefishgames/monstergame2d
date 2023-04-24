using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public enum AnimationPoints { CAST = 0, FEET = 100, UPPER_CENTER = 200, HEAD = 300, CENTER = 400, FRONT = 500, BACK = 600 }

    public class MonsterAnimationSpecialPoints : MonsterComponent
    {
        [SerializeField] private Transform _castPoint;
        [SerializeField] private Transform _feetPoint;
        [SerializeField] private Transform _upperCenter;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _center;
        [SerializeField] private Transform _front;
        [SerializeField] private Transform _back;

        public Transform CastPoint { get { return _castPoint; } }
        public Transform FeetPoint { get { return _feetPoint; } }
        public Transform UpperCenter { get { return _upperCenter; } }
        public Transform Head { get { return _head; } }
        public Transform Center { get { return _center; } }
        public Transform Front { get { return _front; } }
        public Transform Back { get { return _back; } }

        public Transform GetPoint(AnimationPoints point)
        {
            switch (point)
            {
                default:
                    return Center;
                case AnimationPoints.CAST:
                    return CastPoint;
                case AnimationPoints.FEET:
                    return FeetPoint;
                case AnimationPoints.UPPER_CENTER:
                    return UpperCenter;
                case AnimationPoints.HEAD:
                    return Head;
                case AnimationPoints.CENTER:
                    return Center;
                case AnimationPoints.FRONT:
                    return Front;
                case AnimationPoints.BACK:
                    return Back;
            }
        }
    }
}