using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    Map m_Target;

    public override void OnInspectorGUI()
    {
        //ボタンを表示
        if (GUILayout.Button("Open MapEditorSubWindow"))
        {
            MapEditorSubWindow SubWindow = EditorWindow.GetWindow(typeof(MapEditorSubWindow)) as MapEditorSubWindow;
          
            bool isNewWindow = false;

            if (m_Target != target && (SubWindow.m_TargetMap && SubWindow.m_TargetMap != target))
            {
                int selsect = EditorUtility.DisplayDialogComplex(SubWindow.m_TargetMap.gameObject.name, "保存しますか？", "はい", "いいえ", "キャンセル");

                switch(selsect)
                {
                    case 0:

                        isNewWindow = true;

                        break;

                    case 1:

                        isNewWindow = true;

                        break;

                    case 2:

                        break;
                }
            }

            if(isNewWindow || !SubWindow.m_TargetMap)
            {
                MapEditorMainWindow MainWindow = EditorWindow.GetWindow(typeof(MapEditorMainWindow)) as MapEditorMainWindow;

                m_Target = target as Map;

                SubWindow.m_TargetMap = m_Target;
                SubWindow.m_MainWindow = MainWindow;

                MainWindow.m_TargetMap = m_Target;
                MainWindow.m_SubWindow = SubWindow;
                MainWindow.Init();
            }
        }
    }
}
