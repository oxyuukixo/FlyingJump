using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditorSubWindow : EditorWindow
{
    //編集するマップ
    public Map m_TargetMap;

    public MapEditorMainWindow m_MainWindow;

    Vector2 m_ScrollPos = Vector2.zero;

    void OnDestroy()
    {
        if (m_MainWindow)
        {
            m_MainWindow.m_SubWindow = null;
            m_MainWindow.Close();
        }
    }

    void OnGUI()
    {
        if (!m_TargetMap)
        {
            EditorGUILayout.LabelField("編集するマップが選択されていません");
            return;
        }

        m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUI.skin.box);
        {
            //マップチップのエリア
            Rect area = GUILayoutUtility.GetRect(m_MainWindow.m_NewSizeX * m_MainWindow.m_NewNumX, m_MainWindow.m_NewSizeY * m_MainWindow.m_NewNumY);

            if (m_TargetMap.m_MapChipObject.Count > 0 && m_MainWindow.m_NewMapChipNum != null)
            {
                for (int y = 0; y < m_MainWindow.m_NewNumY; y++)
                {
                    for (int x = 0; x < m_MainWindow.m_NewNumX; x++)
                    {
                        if (m_MainWindow.m_NewMapChipNum[y].List[x] == -1)
                        {
                            continue;
                        }

                        SpriteRenderer sprite = m_TargetMap.m_MapChipObject[m_MainWindow.m_NewMapChipNum[y].List[x]].GetComponent<SpriteRenderer>();

                        GUI.DrawTexture(new Rect(area.x + x * m_MainWindow.m_NewSizeX, area.y + y * m_MainWindow.m_NewSizeY, sprite.sprite.texture.width, sprite.sprite.texture.width),
                            sprite.sprite.texture);
                    }
                }
            }

            Handles.color = new Color(0, 0, 0);

            for (int x = 0; x <= m_MainWindow.m_NewNumX; x++)
            {
                Handles.DrawLine(new Vector2(area.x + m_MainWindow.m_NewSizeX * x, area.y), new Vector2(area.x + m_MainWindow.m_NewSizeX * x, area.y + m_MainWindow.m_NewSizeY * m_MainWindow.m_NewNumY));
            }

            for (int y = 0; y <= m_MainWindow.m_NewNumY; y++)
            {
                Handles.DrawLine(new Vector2(area.x, area.y + m_MainWindow.m_NewSizeY * y), new Vector2(area.x + m_MainWindow.m_NewSizeX * m_MainWindow.m_NewNumX, area.y + m_MainWindow.m_NewSizeY * y));
            }


            Event e = Event.current;
            if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
            {
                if (e.button == 0)
                {
                    if (m_TargetMap.m_MapChipObject.Count > 0)
                    {
                        int posX = (int)((e.mousePosition.x - area.x) / m_MainWindow.m_NewSizeX);
                        int posY = (int)((e.mousePosition.y - area.y) / m_MainWindow.m_NewSizeY);

                        if (posX >= 0 && posX < m_MainWindow.m_NewNumX &&
                            posY >= 0 && posY < m_MainWindow.m_NewNumY)
                        {
                            m_MainWindow.m_NewMapChipNum[posY].List[posX] = m_MainWindow.GetSelectChipNum();
                        }

                        Repaint();
                    }
                }

                if (e.button == 1)
                {
                    int posX = (int)((e.mousePosition.x - area.x) / m_MainWindow.m_NewSizeX);
                    int posY = (int)((e.mousePosition.y - area.y) / m_MainWindow.m_NewSizeY);

                    if (posX < m_MainWindow.m_NewNumX && posY < m_MainWindow.m_NewNumY)
                    {
                        m_MainWindow.m_NewMapChipNum[posY].List[posX] = -1;
                    }

                    Repaint();
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }
}
