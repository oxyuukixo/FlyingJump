using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class MapEditorMainWindow : EditorWindow
{
    //編集するマップのマネージャー
    public Map m_TargetMap;

    public MapEditorSubWindow m_SubWindow;

    public List<IntList> m_NewMapChipNum;

    public int m_NewNumX = 0;
    public int m_NewNumY = 0;

    public int m_NewSizeX = 0;
    public int m_NewSizeY = 0;

    public bool m_IsEdit = false;

    private int m_SelectedGridIndex = 0;

    Vector2 m_ScrollPos = Vector2.zero;

    // 現在編集中のコントロール名
    string m_EditControlName;

    // 現在編集中のコントロールに表示している値
    object m_EditControlValue;

    public void Init()
    {
        bool isMapObj = false;
        bool isEnemyObj = false;
        bool isCollisionObj = false;

        foreach(Transform child in m_TargetMap.transform)
        {
            if(child.name == "MapChip")
            {
                isMapObj = true;
                continue;
            }
            if (child.name == "Enemy")
            {
                isEnemyObj = true;
                continue;
            }
            if (child.name == "Collision")
            {
                isCollisionObj = true;
                continue;
            }
        }

        if(!isMapObj)
        {
            GameObject obj = new GameObject();
            obj.name = "MapChip";
            obj.transform.position = m_TargetMap.transform.position;
            obj.transform.SetParent(m_TargetMap.transform);
        }

        if(!isEnemyObj)
        {
            GameObject obj = new GameObject();
            obj.name = "Enemy";
            obj.transform.position = m_TargetMap.transform.position;
            obj.transform.SetParent(m_TargetMap.transform);
        }

        if (!isCollisionObj)
        {
            GameObject obj = new GameObject();
            obj.name = "Collision";
            obj.transform.position = m_TargetMap.transform.position;
            obj.transform.SetParent(m_TargetMap.transform);
        }

        m_NewNumX = m_TargetMap.m_NumX;
        m_NewNumY = m_TargetMap.m_NumY;

        m_NewSizeX = m_TargetMap.m_SizeX;
        m_NewSizeY = m_TargetMap.m_SizeY;

        m_NewMapChipNum = new List<IntList>();

        if (m_TargetMap.m_MapChipNum.Count > 0)
        {
            for (int y = 0; y < m_TargetMap.m_MapChipNum.Count; y++)
            {
                m_NewMapChipNum.Add(new IntList(new List<int>()));

                for (int x = 0; x < m_TargetMap.m_MapChipNum[y].List.Count; x++)
                {
                    m_NewMapChipNum[y].List.Add(m_TargetMap.m_MapChipNum[y].List[x]);
                }
            }
        }
        else
        {
            for (int y = 0; y < m_NewNumY; y++)
            {
                m_NewMapChipNum.Add(new IntList(new List<int>()));

                for (int x = 0; x < m_NewNumX; x++)
                {
                    m_NewMapChipNum[y].List.Add(-1);
                }
            }
        }

    }

    void OnDestroy()
    {
        if(m_IsEdit)
        {
            if (EditorUtility.DisplayDialog("マップの保存", "保存しますか？", "はい", "いいえ"))
            {
                Apply();
            }
        }

        if (m_SubWindow)
        {
            m_SubWindow.m_MainWindow = null;
            m_SubWindow.Close();
        }
    }

    void OnGUI()
    {
        if (!m_TargetMap)
        {
            EditorGUILayout.LabelField("編集するマップが選択されていません");
            return;
        }

        //縦に整列
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            EditorGUILayout.BeginHorizontal();
            {
                //NumX
                {
                    // 現在編集中のエイリアスデータが保存されているか
                    bool isEdit = m_EditControlName == "NumX";

                    // 表示すべきデータがあればそちらに切り替える
                    int dispValue = m_NewNumX;
                    if (isEdit)
                    {
                        dispValue = (int)m_EditControlValue;
                    }

                    // フィールド名を指定してデータを表示して戻り値を得る
                    GUI.SetNextControlName("NumX");
                    int newNumX = EditorGUILayout.IntField("NumX", dispValue);

                    // 現在フォーカスされているか
                    bool isFocuse = GUI.GetNameOfFocusedControl() == "NumX";

                    // オリジナルの値と変わっているか比較判定
                    bool isChange = m_NewNumX != newNumX;

                    // エンターキー入力判定
                    bool isEnter = (Event.current.isKey && (Event.current.keyCode == KeyCode.Return));

                    // フォーカスされていてエンターキー入力されたよ！
                    bool isFocusInputEnter = isChange && isFocuse && isEnter;

                    // フォーカスロストしてるけどバッファにはこのコントロールが編集中だったって記録されてるよ！
                    bool isLostFocusEnter = isChange && (isFocuse == false) && isEdit;

                    // 入力エイリアスを更新するよ
                    if (isFocuse && (dispValue != newNumX))
                    {
                        m_EditControlName = "NumX";
                        m_EditControlValue = newNumX;
                    }

                    // エンターアプライまたはロストフォーカスアプライするよ
                    if (isFocusInputEnter || isLostFocusEnter)
                    {
                        // 入力エイリアスをクリア
                        m_EditControlName = null;
                        m_EditControlValue = null;

                        // いつも通りの更新
                        Undo.RecordObject(this, string.Empty);
                        m_NewNumX = newNumX;
                        EditorUtility.SetDirty(this);

                        m_IsEdit = true;

                        SetMapSize();
                    }
                }

                //NumY
                {
                    // 現在編集中のエイリアスデータが保存されているか
                    bool isEdit = m_EditControlName == "NumY";

                    // 表示すべきデータがあればそちらに切り替える
                    int dispValue = m_NewNumY;
                    if (isEdit)
                    {
                        dispValue = (int)m_EditControlValue;
                    }

                    // フィールド名を指定してデータを表示して戻り値を得る
                    GUI.SetNextControlName("NumY");
                    int newNumY = EditorGUILayout.IntField("NumY", dispValue);

                    // 現在フォーカスされているか
                    bool isFocuse = GUI.GetNameOfFocusedControl() == "NumY";

                    // オリジナルの値と変わっているか比較判定
                    bool isChange = m_NewNumY != newNumY;

                    // エンターキー入力判定
                    bool isEnter = (Event.current.isKey && (Event.current.keyCode == KeyCode.Return));

                    // フォーカスされていてエンターキー入力されたよ！
                    bool isFocusInputEnter = isChange && isFocuse && isEnter;

                    // フォーカスロストしてるけどバッファにはこのコントロールが編集中だったって記録されてるよ！
                    bool isLostFocusEnter = isChange && (isFocuse == false) && isEdit;

                    // 入力エイリアスを更新するよ
                    if (isFocuse && (dispValue != newNumY))
                    {
                        m_EditControlName = "NumY";
                        m_EditControlValue = newNumY;
                    }

                    // エンターアプライまたはロストフォーカスアプライするよ
                    if (isFocusInputEnter || isLostFocusEnter)
                    {
                        // 入力エイリアスをクリア
                        m_EditControlName = null;
                        m_EditControlValue = null;

                        // いつも通りの更新
                        Undo.RecordObject(this, string.Empty);
                        m_NewNumY = newNumY;
                        EditorUtility.SetDirty(this);

                        m_IsEdit = true;

                        SetMapSize();
                    }
                }
            }
            EditorGUILayout.EndVertical();

            //変更の確認をする
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    m_NewSizeX = EditorGUILayout.IntField("SizeX", m_NewSizeX);

                    m_NewSizeY = EditorGUILayout.IntField("SizeY", m_NewSizeY);
                }
                EditorGUILayout.EndVertical();
            }
            //ここまでで値が変更されていたらマップに反映する
            if (EditorGUI.EndChangeCheck())
            {
                m_IsEdit = true;

                SetMapSize();
            }
        }
        EditorGUILayout.EndVertical();

        if (m_TargetMap.m_MapChipObject.Count > 0)
        {
            for (int i = 0; i < m_TargetMap.m_MapChipObject.Count; i++)
            {
                if(!m_TargetMap.m_MapChipObject[i])
                {
                    DeleteMapChip(i);
                }
            }

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField(m_TargetMap.m_MapChipObject[m_SelectedGridIndex].name);

                if (GUILayout.Button("Delete"))
                {
                    DeleteMapChip(m_SelectedGridIndex);
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

                int numX = Screen.width / 100 + 1;

                if (numX <= 0)
                {
                    numX = 1;
                }

                m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUI.skin.box);
                {
                    int newIndex = GUILayout.SelectionGrid(m_SelectedGridIndex, texture, numX, GUILayout.Width(numX * 100), GUILayout.Height((m_TargetMap.m_MapChipObject.Count / numX + 1) * 100));

                    if (m_SelectedGridIndex != newIndex)
                    {
                        GUI.FocusControl("");
                    }

                    m_SelectedGridIndex = newIndex;
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
                            if (draggedObject.GetType() == typeof(GameObject))
                            {
                                bool IsSame = false;

                                for (int i = 0; i < m_TargetMap.m_MapChipObject.Count; i++)
                                {
                                    if(m_TargetMap.m_MapChipObject[i] ==  draggedObject)
                                    {
                                        IsSame = true;
                                        break;
                                    }
                                }

                                if(!IsSame)
                                {
                                    m_TargetMap.m_MapChipObject.Add(draggedObject as GameObject);
                                }
                            }

                            if(draggedObject.GetType() == typeof(Sprite))
                            {
                                GameObject newobj = new GameObject();

                                newobj.name = draggedObject.name;
                                newobj.AddComponent<SpriteRenderer>();
                                newobj.GetComponent<SpriteRenderer>().sprite = draggedObject as Sprite;
                                newobj.GetComponent<SpriteRenderer>().sortingLayerName = "Block";

                                GameObject prefab = PrefabUtility.CreatePrefab("Assets/Resources/Prefabs/MapChip/" + newobj.name + ".prefab", newobj);
                                UnityEditor.AssetDatabase.SaveAssets();

                                m_TargetMap.m_MapChipObject.Add(prefab);

                                GameObject.DestroyImmediate(newobj);
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
                Apply();
            }
        }
        EditorGUILayout.EndVertical();
    }

    public void DeleteMapChip(int ChipNum)
    {
        if (ChipNum >= 0 && ChipNum < m_TargetMap.m_MapChipObject.Count)

            for (int y = 0; y < m_NewNumY; y++)
            {
                for (int x = 0; x < m_NewNumX; x++)
                {
                    if (m_NewMapChipNum[y].List[x] > ChipNum)
                    {
                        m_NewMapChipNum[y].List[x] -= 1;
                    }
                    else if (m_NewMapChipNum[y].List[x] == ChipNum)
                    {
                        m_NewMapChipNum[y].List[x] = -1;
                    }

                }
            }

        m_TargetMap.m_MapChipObject.RemoveAt(ChipNum);

        m_SelectedGridIndex = 0;

        m_SubWindow.Repaint();
    }

    //=============================================================================
    //
    // Purpose : フォーカスが失われたときの処理．
    //
    //=============================================================================
    void OnLostFocus()
    {
        // 入力エイリアスをクリア
        m_EditControlName = null;
        m_EditControlValue = null;
    }

    //=============================================================================
    //
    // Purpose : 変更を保存するときの処理．
    //
    //=============================================================================
    void SetMapSize()
    {
        List<IntList> newMap = new List<IntList>();

        for (int y = 0; y < m_NewNumY; y++)
        {
            newMap.Add(new IntList(new List<int>()));

            for (int x = 0; x < m_NewNumX; x++)
            {
                newMap[y].List.Add(-1);
            }
        }

        for (int y = 0; y < m_NewMapChipNum.Count && y < m_NewNumY; y++)
        {
            for (int x = 0; x < m_NewMapChipNum[y].List.Count && x < m_NewNumX; x++)
            {
                newMap[y].List[x] = m_NewMapChipNum[y].List[x];
            }
        }

        m_NewMapChipNum = newMap;

        m_SubWindow.Repaint();
    }

    //=============================================================================
    //
    // Purpose : 変更を保存するときの処理．
    //
    //=============================================================================
    private void Apply()
    {
        GameObject mapObj = m_TargetMap.transform.FindChild("MapChip").gameObject;

        m_IsEdit = false;

        m_TargetMap.m_NumX = m_NewNumX;
        m_TargetMap.m_NumY = m_NewNumY;

        m_TargetMap.m_SizeX = m_NewSizeX;
        m_TargetMap.m_SizeY = m_NewSizeY;

        m_TargetMap.m_MapChipNum.Clear();

        for (int y = 0; y < m_NewMapChipNum.Count; y++)
        {
            m_TargetMap.m_MapChipNum.Add(new IntList(new List<int>()));

            for (int x = 0; x < m_NewMapChipNum[y].List.Count; x++)
            {
                m_TargetMap.m_MapChipNum[y].List.Add(m_NewMapChipNum[y].List[x]);
            }
        }

        for (int i = mapObj.transform.childCount - 1; i >= 0; --i)
        {
            GameObject.DestroyImmediate(mapObj.transform.GetChild(i).gameObject);
        }

        for (int y = 0; y < m_NewNumY; y++)
        {
            for (int x = 0; x < m_NewNumX; x++)
            {
                if (m_NewMapChipNum[y].List[x] != -1)
                {
                    GameObject obj = Instantiate(m_TargetMap.m_MapChipObject[m_NewMapChipNum[y].List[x]]);

                    obj.transform.parent = mapObj.gameObject.transform;

                    obj.transform.localPosition = new Vector2(m_NewSizeX / 2 + x * m_NewSizeX, - (m_NewSizeY / 2 + y * m_NewSizeY));
                }
            }
        }

        GameObject collisionObj = m_TargetMap.transform.FindChild("Collision").gameObject;

        for (int i = collisionObj.transform.childCount - 1; i >= 0; --i)
        {
            GameObject.DestroyImmediate(collisionObj.transform.GetChild(i).gameObject);
        }

        GameObject newObj = new GameObject();
    
        BoxCollider2D col = newObj.AddComponent<BoxCollider2D>();
        col.size = new Vector2(m_NewNumX * m_NewSizeX, 100);

        newObj.transform.SetParent(collisionObj.transform);
        newObj.name = "UpCollsion";
        newObj.tag = "UpCameraBlock";
        newObj.transform.localPosition = new Vector2(col.size.x / 2,col.size.y / 2);
        newObj.layer = LayerMask.NameToLayer("CameraBlock");

        newObj = new GameObject();

        col = newObj.AddComponent<BoxCollider2D>();
        col.size = new Vector2(m_NewNumX * m_NewSizeX, 100);

        newObj.transform.SetParent(collisionObj.transform);
        newObj.name = "DownCollsion";
        newObj.tag = "DownCameraBlock";
        newObj.transform.localPosition = new Vector2(col.size.x / 2, -(m_NewNumY * m_NewSizeY + col.size.y / 2));
        newObj.layer = LayerMask.NameToLayer("CameraBlock");

    }

    //=============================================================================
    //
    // Purpose : 関数の処理内容．
    //
    // Return : 関数の戻り値．
    //
    //=============================================================================
    public int GetSelectChipNum()
    {
        return m_SelectedGridIndex;
    }

    //=============================================================================
    //
    // Purpose : 変更を保存するときの処理．
    //
    //=============================================================================
    public void SetArryInit(List<List<int>> InitIns, int InitNum,int NumX,int NumY)
    {
        if(InitIns == null)
        {
            InitIns = new List<List<int>>();
        }

        InitIns.Clear();

        for (int y = 0; y < NumY; y++)
        {
            InitIns.Add(new List<int>());

            for (int x = 0; x < NumX; x++)
            {
                InitIns[y].Add(InitNum);
            }
        }
    }
}
