using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tower))]
public class TowerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Tower towerObj = target as Tower;
        if (GUILayout.Button("Build Tower")) {
            towerObj.BuildTower();
        }
        if (GUILayout.Button("Reset Tower")) {
            towerObj.ResetTower();
        }
        GUI.enabled = Application.isPlaying;
        if (GUILayout.Button("StartGame")) {
            towerObj.StartGame();
        }
    }
}
