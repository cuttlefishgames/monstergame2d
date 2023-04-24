using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    public class MonsterComponent : MonoBehaviour
    {
        public IMonster ParentMonster { get; protected set; }

        public virtual void AttachToMonster(IMonster parent)
        {
            ParentMonster = parent;
        }
    }
}