using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour,ScrollObjectInterface {

    //マップの幅
    public int m_NumX = 1;
    public int m_NumY = 1;

    //マップチップの大きさ
    public int m_SizeX = 1;
    public int m_SizeY = 1;

    //マップチップに使用するゲームオブジェクト
    [HideInInspector]
    public List<GameObject> m_MapChipObject;

    //マップチップの番号
    [SerializeField]
    public List<IntList> m_MapChipNum;

	// Use this for initialization
	void Start () {
 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetX()
    {
        return transform.position.x;
    }

    public float GetY()
    {
        return transform.position.y;
    }

    public float GetWidth()
    {
        return m_NumX * m_SizeX;
    }

    public float GetHeight()
    {
        return m_NumY * m_SizeY;
    }
}

[System.Serializable]
public class IntList
{
    public List<int> List = new List<int>();

    public IntList(List<int> list)
    {
        List = list;
    }
}
