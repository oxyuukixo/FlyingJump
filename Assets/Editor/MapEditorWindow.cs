using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public class MapEditorWindow : EditorWindow
{
    //編集するマップ
    public Map m_TargetMap;

    public MapEditorStateWindow m_TargetMESWindow;

    Vector2 m_ScrollPos = Vector2.zero;

    void OnGUI()
    {
        m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUI.skin.box);
        {
            Rect area = GUILayoutUtility.GetRect(m_TargetMESWindow.m_NewSizeX * m_TargetMESWindow.m_NewNumX, m_TargetMESWindow.m_NewSizeY * m_TargetMESWindow.m_NewNumY);

            for (int x = 0; x <= m_TargetMESWindow.m_NewNumX; x++)
            {
                Handles.DrawLine(new Vector2(area.x + m_TargetMESWindow.m_NewSizeX * x, area.y), new Vector2(area.x + m_TargetMESWindow.m_NewSizeX * x, area.y + m_TargetMESWindow.m_NewSizeY * m_TargetMESWindow.m_NewNumY));
            }

            for (int y = 0; y <= m_TargetMESWindow.m_NewNumY; y++)
            {
                Handles.DrawLine(new Vector2(area.x, area.y + m_TargetMESWindow.m_NewSizeY * y), new Vector2(area.x + m_TargetMESWindow.m_NewSizeX * m_TargetMESWindow.m_NewNumX, area.y + m_TargetMESWindow.m_NewSizeY * y));
            }
        }
        EditorGUILayout.EndScrollView();

        //for (int y = 0; y < m_TargetMESWindow.m_NewNumY; y++)
        //{
        //    for (int x = 0; x < m_TargetMESWindow.m_NewNumX; x++)
        //    {
        //        if (m_TargetMESWindow.m_NewMapChipNum[y, x] == -1)
        //        {
        //            continue;
        //        }
        //    }
        //}


        //Event e = Event.current;
        //if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
        //{
        //    if (e.button == 0)
        //    {
        //        int posX = (int)e.mousePosition.x / m_TargetMESWindow.m_NewSizeX;
        //        int posY = (int)e.mousePosition.y / m_TargetMESWindow.m_NewSizeY;

        //        if (posX < m_TargetMESWindow.m_NewNumX && posY < m_TargetMESWindow.m_NewNumY)
        //        {
        //            m_TargetMESWindow.m_NewMapChipNum[posY,posX] = m_TargetMESWindow.GetSelectChipNum();
        //        }
        //    }

        //    //if(e.button == 1)
        //    //{

        //    //}
        //}
    }
}

public class MapEditorStateWindow : EditorWindow
{
    //編集するマップのマネージャー
    public Map m_TargetMap;

    public int[,] m_NewMapChipNum;

    public int m_NewNumX = 0;
    public int m_NewNumY = 0;

    public int m_NewSizeX = 0;
    public int m_NewSizeY = 0;

    private int selectedGridIndex = 0;

    //変更を加えたのかのフラグ
    private bool m_IsChange = false;

    Vector2 m_ScrollPos = Vector2.zero;

    void Init()
    {
        
    }

    void OnSelectionChange()
    {
        foreach (GameObject go in Selection.gameObjects)
        {

        }

        Debug.Log("aa");
    }

    void OnDestroy()
    {
       int state = EditorUtility.DisplayDialogComplex
            ("MapEditor",
            "保存しますか？",
            "はい",
            "いいえ",
            "キャンセル");

        switch (state)
        {
            case 0:

                break;

            case 1:

                break;

            case 2:

               

                break;
        }

    }

    void OnGUI()
    {

        //変更の確認をする
        EditorGUI.BeginChangeCheck();
        {
            //縦に整列
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    m_NewNumX = EditorGUILayout.IntField("NumX", m_NewNumX);

                    m_NewNumY = EditorGUILayout.IntField("NumY", m_NewNumY);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginHorizontal();
                {
                    m_NewSizeX = EditorGUILayout.IntField("SizeX", m_NewSizeX);

                    m_NewSizeY = EditorGUILayout.IntField("SizeY", m_NewSizeY);
                }
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndVertical();

        }
        //ここまでで値が変更されていたらマップに反映する
        if (EditorGUI.EndChangeCheck())
        {
            //if (m_NewNumY != 0 && m_NewNumX != 0)
            //{
            //    int[,] newMap = new int[m_NewNumY, m_NewNumX];

            //    SetArryInit(newMap, -1);

            //    for (int y = 0; y < m_NewMapChipNum.GetLength(0); y++)
            //    {
            //        for (int x = 0; x < m_NewMapChipNum.GetLength(1); x++)
            //        {
            //            newMap[y, x] = m_NewMapChipNum[y, x];
            //        }
            //    }

            //    m_NewMapChipNum = newMap;
            //}
        }

        if (m_TargetMap.m_MapChipObject.Count > 0)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {

                EditorGUILayout.LabelField(m_TargetMap.m_MapChipObject[selectedGridIndex].name);

                if (GUILayout.Button("Delete"))
                {
                    m_TargetMap.m_MapChipObject.RemoveAt(selectedGridIndex);

                    selectedGridIndex = 0;
                }
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            if (m_TargetMap.m_MapChipObject.Count > 0)
            {
                Texture[] texture = new Texture[m_TargetMap.m_MapChipObject.Count];

                for (int i = 0; i < m_TargetMap.m_MapChipObject.Count; i++)
                {
                    texture[i] = m_TargetMap.m_MapChipObject[i].GetComponent<SpriteRenderer>().sprite.texture;
                }

                int Xnum = Screen.width / 100;

                if (Xnum <= 0)
                {
                    Xnum = 1;
                }

                m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUI.skin.box);
                {
                    selectedGridIndex = GUILayout.SelectionGrid(selectedGridIndex, texture, Xnum, GUILayout.Width(Xnum * 100), GUILayout.Height((m_TargetMap.m_MapChipObject.Count / Xnum + 1) * 100));
                }
                EditorGUILayout.EndScrollView();
            }

            var e = Event.current;

            //ドロップするボックスの表示
            var dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Add GameObject\nDrag & Drop");

            int id = GUIUtility.GetControlID(FocusType.Passive);
            switch (e.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:

                    if (!dropArea.Contains(e.mousePosition)) break;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    DragAndDrop.activeControlID = id;

                    if (e.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (var draggedObject in DragAndDrop.objectReferences)
                        {
                            GameObject obj = draggedObject as GameObject;

                            if (obj)
                            {
                                Debug.Log("Drag Object:" + AssetDatabase.GetAssetPath(draggedObject));
                                m_TargetMap.m_MapChipObject.Add(obj);
                            }

                        }
                        DragAndDrop.activeControlID = 0;
                    }

                    Event.current.Use();

                    break;
            }

            //適用ボタン
            if (GUILayout.Button("Apply"))
            {
                m_TargetMap.m_MapChipObject.Clear();
            }
        }
        EditorGUILayout.EndVertical();
    }

    public int GetSelectChipNum()
    {
        return selectedGridIndex;
    }

    public void SetArryInit(int[,] InitIns,int InitNum)
    {
        for(int y = 0; y < InitIns.GetLength(0);y++)
        {
            for(int x = 0; x <InitIns.GetLength(1);x++)
            {
                InitIns[y,x] = InitNum;
            }
        }
    }
}


