using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BattleOrder2DTester))]
public class BattleOrder2DTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BattleOrder2DTester gameManager = (BattleOrder2DTester)target;

        if (GUILayout.Button("Test Order"))
        {
            gameManager.StartOrder();
        }
    }
}
