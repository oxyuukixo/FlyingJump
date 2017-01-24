using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //ボタンを表示
        if (GUILayout.Button("Open MapEditorWindow"))
        {
            MapEditorWindow MEWindow = EditorWindow.GetWindow(typeof(MapEditorWindow)) as MapEditorWindow;
            MapEditorStateWindow MESWindow = EditorWindow.GetWindow(typeof(MapEditorStateWindow)) as MapEditorStateWindow;

            MEWindow.m_TargetMap = target as Map;
            MEWindow.m_TargetMESWindow = MESWindow;

            MESWindow.m_TargetMap = target as Map;

            //if (MEWindow.m_TargetMap.m_MapChipNum.GetLength(0) < 0)
            //{
            //    MEWindow.m_TargetMap.m_MapChipNum = new int[MEWindow.m_TargetMap.m_NumY, MEWindow.m_TargetMap.m_NumX];

            //    MESWindow.SetArryInit(MEWindow.m_TargetMap.m_MapChipNum, -1);
            //}

            //MEWindow.m_TargetMap.m_MapChipNum.CopyTo(MESWindow.m_NewMapChipNum, 0);
        }
    }
}
