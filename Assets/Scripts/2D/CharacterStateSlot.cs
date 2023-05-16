using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateSlot : MonoBehaviour
{
    [SerializeField] private ActionBar2D _actionBar;
    public ActionBar2D ActionBar => _actionBar;
}
