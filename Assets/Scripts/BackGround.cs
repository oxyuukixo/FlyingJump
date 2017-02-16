using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour,ScrollObjectInterface {

    private SpriteRenderer m_SpriteRenderer;

    void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        return m_SpriteRenderer.bounds.size.x;
    }

    public float GetHeight()
    {
        return m_SpriteRenderer.bounds.size.y;
    }
}
