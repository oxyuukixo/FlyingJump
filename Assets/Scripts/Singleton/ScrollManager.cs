using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : SingletonMonoBehaviour<ScrollManager>
{
    //スクロールスピード
    public float m_ScrollSpeed;

    //スクロールする順番をランダムにするか
    public bool m_IsRandom;

    //スクロールするオブジェクトのリスト
    public GameObject[] m_BackList;
    public GameObject[] m_StageList;

    //現在スクロールしているオブジェクト
    private List<GameObject> m_BackObject = new List<GameObject>();
    private List<GameObject> m_StageObject = new List<GameObject>();

    //現在スクロールしているオブジェクトの番号
    private int m_BackNum = 0;
    private int m_StageNum = 0;

    //すべてのオブジェクトをスクロールし終わっていたら
    private bool m_IsScrollEnd = false;

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
    }

    // Use this for initialization
    void Start() {

        AddObject(m_BackObject, m_BackList,m_BackNum);
        BackUpCount();
        AddObject(m_BackObject, m_BackList, m_BackNum);

        AddObject(m_StageObject, m_StageList,m_StageNum);
        StageUpCount();
        AddObject(m_StageObject, m_StageList,m_StageNum);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Scroll(m_BackObject);
        
        if(ScrollCheck(m_BackObject[0]))
        {
            Destroy(m_BackObject[0]);
            m_BackObject.RemoveAt(0);
            BackUpCount();
            AddObject(m_BackObject, m_BackList, m_BackNum);
        }

        Scroll(m_StageObject);

        if (ScrollCheck(m_StageObject[0]))
        {
            if(m_IsScrollEnd)
            {
                GameManager.Instance.ScrollEnd();
            }

            Destroy(m_StageObject[0]);
            m_StageObject.RemoveAt(0);
            StageUpCount();
            AddObject(m_StageObject, m_StageList, m_StageNum);
        }
    }

    void Scroll(List<GameObject> obj)
    {
        for(int i = 0; i < obj.Count;i++)
        {
            obj[i].transform.position -= new Vector3(m_ScrollSpeed * Time.deltaTime, 0);
        }
    }

    bool BackUpCount()
    {
        if((m_BackNum += 1) >= m_BackList.Length)
        {
            m_BackNum = 0;
            return false;
        }

        return true;
    }

    bool StageUpCount()
    {
        if ((m_StageNum += 1) >= m_StageList.Length)
        {
            m_StageNum = 0;
            m_IsScrollEnd = true;

            return false;
        }

        return true;
    }

    GameObject GetRandomObject(GameObject[] objList)
    {
        return objList[Random.Range(0,objList.Length - 1)];
    }

    void AddObject(List<GameObject> obj,GameObject[] objList,int num)
    {
        GameObject addObj;

        if (m_IsRandom)
        {
            addObj = GetRandomObject(objList);
        }
        else
        {
            addObj = objList[num];
        }
       
        GameObject newObj = Instantiate(addObj);

        if(obj.Count > 0)
        {
            ScrollObjectInterface ScrollObj = obj[obj.Count - 1].GetComponent<ScrollObjectInterface>();
            newObj.transform.position = new Vector2(ScrollObj.GetX() + ScrollObj.GetWidth(), ScrollObj.GetY());
        }
        else
        {
            newObj.transform.position = transform.position;
        }

        if (m_IsScrollEnd)
        {
            foreach(Transform enemyObj in newObj.transform)
            {
                if(enemyObj.name == "Enemy")
                {
                    Destroy(enemyObj.gameObject);
                    break;
                }
            }
        }

        obj.Add(newObj);
    }

    bool ScrollCheck(GameObject obj)
    {
        ScrollObjectInterface scrollObj = obj.GetComponent<ScrollObjectInterface>();

        if(scrollObj.GetX() + scrollObj.GetWidth() < transform.position.x)
        {
            return true;
        }

        return false;
    }

    public void ScrollStop()
    {
        m_ScrollSpeed = 0;
    }
}
