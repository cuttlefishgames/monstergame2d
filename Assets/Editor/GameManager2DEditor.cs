using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager2D))]
public class GameManager2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager2D gameManager = (GameManager2D)target;

        if(GUILayout.Button("Assemble Battlefield"))
        {
            gameManager.TestBattlefied();
        }
    }
}
