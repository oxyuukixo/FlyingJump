using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //体力
    public int m_HP;

    public EnemyMove m_MoveScript;

    public EnemyFire m_FireScript;

    public GameObject m_DeathObject;

    private Rigidbody2D m_Rigidbody;

	// Use this for initialization
	void Start () {

        m_Rigidbody = GetComponent<Rigidbody2D>();

        if(!m_MoveScript)
        {
            m_MoveScript = gameObject.AddComponent<EnemyMove>();
        }

        if(!m_FireScript)
        {
            m_FireScript = gameObject.AddComponent<EnemyFire>();
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.x < MainCameraManager.Instance.GetCameraCornerPos(new Vector2(1, 0)).x)
        {
            m_MoveScript.Move();

            m_FireScript.Fire();
        }
	} 

    public void ApplyDamage(int damage)
    {
        if((m_HP -= damage) <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        GameObject obj = Instantiate(m_DeathObject);

        obj.transform.position = transform.position;

        obj.transform.SetParent(gameObject.transform.parent);

        Destroy(gameObject);
    }
}
