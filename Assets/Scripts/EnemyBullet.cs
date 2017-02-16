using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public Vector2 m_Speed;

    private Rigidbody2D m_rigibody;

    // Use this for initialization
    void Start()
    {
        m_rigibody = GetComponent<Rigidbody2D>();

        m_rigibody.velocity = new Vector2(m_Speed.x, m_Speed.y);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(this);
    }
}
