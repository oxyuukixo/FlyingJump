using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {

    public GameObject m_Bullet;

    //射撃間の時間
    public float m_FireInterval = 0;

    //射撃のレート
    public float m_FireRare = 0;

    //一度のサイクルで出す弾の数
    public int m_BulletNum = 0;

    //射撃間隔の現在時間を保持しておく変数
    protected float m_CurrentFireInterval = 0;
    protected float m_CurrentFireRate = 0;
    protected float m_CUrrentNum = 0;

    public virtual void Fire()
    {
        if ((m_CurrentFireInterval += Time.deltaTime) > m_FireInterval)
        {
            if ((m_CurrentFireRate += Time.deltaTime) > m_FireRare)
            {
                if ((m_BulletNum + 1) > m_BulletNum)
                {
                    m_CurrentFireRate = 0;
                    m_CurrentFireInterval = 0;
                }
                else
                {
                    Fire();
                    m_CUrrentNum++;
                    m_CurrentFireRate = 0;
                }
            }
        }
    }

    public virtual void FireBullet()
    {
        GameObject obj = Instantiate(m_Bullet);
        obj.transform.position = gameObject.transform.position;
    }

}
