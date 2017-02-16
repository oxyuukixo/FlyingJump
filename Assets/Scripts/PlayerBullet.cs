using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    public Vector2 m_Speed;

    public int m_Damage;

    private Rigidbody2D m_rigibody;

	// Use this for initialization
	void Start () {

        m_rigibody = GetComponent<Rigidbody2D>();

        m_rigibody.velocity = new Vector2(m_Speed.x, m_Speed.y);
    }
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "FlyEnemy" || col.tag == "GroundEnemy")
        {
            col.GetComponent<Enemy>().ApplyDamage(m_Damage);

            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
