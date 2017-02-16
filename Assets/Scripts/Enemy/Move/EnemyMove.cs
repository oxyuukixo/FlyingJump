using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Vector2 m_Speed;

    public virtual void Move()
    {
        transform.position += new Vector3(m_Speed.x * Time.deltaTime, m_Speed.y * Time.deltaTime); 
    }
}
