using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraManager : SingletonMonoBehaviour<MainCameraManager>
{ 
    //状態遷移しているか
    private bool m_IsChange = false;

    private Camera m_camera;
    public Camera cameraCompornemt
    {
        get { return m_camera; }
    }

    public bool IsChange
    {
        get { return m_IsChange; }
    }

    //移動できる範囲のコリジョンのタグの名前
    private string m_BlockTag;

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        m_camera = GetComponent<Camera>();
    }

    public void Change(float gravity,string blockTag)
    {
        m_BlockTag = blockTag;

        m_IsChange = true;
        GetComponent<Rigidbody2D>().gravityScale = gravity;
        GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == m_BlockTag)
        {
            m_IsChange = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        }
    }

    public Vector2 GetCameraCornerPos(Vector2 pos)
    {
        return m_camera.ScreenToWorldPoint(new Vector2(Screen.width * pos.x,-Screen.height * pos.y));
    }
}
