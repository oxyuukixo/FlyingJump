using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour {

    public float SpeedX;        //速さ

    public GameObject[] ObjectList;         //使う背景の種類

    public GameObject[] ScrollOblect;       //画面に出てるオブジェクト

    public bool IsRandom;               //ランダムで出現させるか

    private Camera MainCamera;

    private int num = 0;                 //順番に出現させるときの出現させる番号

    void Awake()
    {
        for (int i = 0; i < ScrollOblect.Length; i++)
        {
            if (ScrollOblect[i] == null || !ScrollOblect[i].activeInHierarchy)
            {
                if (IsRandom)
                {
                    ScrollOblect[i] = Instantiate(ObjectList[Random.Range(0, ObjectList.Length)]);
                }
                else
                {
                    ScrollOblect[i] = Instantiate(ObjectList[num]);

                    if((num += 1) >= ObjectList.Length)
                    {
                        num = 0;
                    }
                }
            }

            float Pos = 0;

            if (i == 0)
            {
                Pos = transform.position.x;
            }
            else
            {
                Pos = ScrollOblect[i - 1].transform.position.x + ScrollOblect[i - 1].GetComponent<SpriteRenderer>().bounds.size.x;
            }

            ScrollOblect[i].transform.position = new Vector3(Pos, transform.position.y, transform.position.z);
        }
    }

	// Use this for initialization
	void Start () {
        GameObject obj = GameObject.Find("Main Camera");
        MainCamera = obj.GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {

        for(int i = 0; i < ScrollOblect.Length; i++)
        {
            if (ScrollOblect[i].transform.position.x + ScrollOblect[i].GetComponent<SpriteRenderer>().bounds.size.x < MainCamera.ScreenToWorldPoint(new Vector2(0,0)).x)
            {
                Destroy(ScrollOblect[i]);

                if (IsRandom)
                {
                    ScrollOblect[i] = Instantiate(ObjectList[Random.Range(0, ObjectList.Length)]);
                }
                else
                {
                    ScrollOblect[i] = Instantiate(ObjectList[num]);

                    if ((num += 1) >= ObjectList.Length)
                    {
                        num = 0;
                    }
                }

                float Pos = 0;

                if (i == 0)
                {
                    Pos = ScrollOblect[ScrollOblect.Length - 1].transform.position.x + ScrollOblect[ScrollOblect.Length - 1].GetComponent<SpriteRenderer>().bounds.size.x;
                }
                else
                {
                    Pos = ScrollOblect[i - 1].transform.position.x + ScrollOblect[i - 1].GetComponent<SpriteRenderer>().bounds.size.x;
                }

                ScrollOblect[i].transform.position = new Vector3(Pos, transform.position.y, transform.position.z);
            }
        }
    }
}
