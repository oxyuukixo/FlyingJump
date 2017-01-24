using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    //マップの幅
    public int m_NumX = 0;
    public int m_NumY = 0;

    //マップチップの大きさ
    public int m_SizeX = 0;
    public int m_SizeY = 0;

    //マップチップに使用するゲームオブジェクト
    [HideInInspector]
    public List<GameObject> m_MapChipObject;

    //マップチップの番号
    [HideInInspector]
    public int[,] m_MapChipNum;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
