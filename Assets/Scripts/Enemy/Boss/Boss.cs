using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    //体力
    public int m_HP;

    public float m_SpawnTime = 2;

    protected SpriteRenderer m_renderer;

    //出現完了したか
    private bool m_IsSpawn = false;

    //死んだか
    protected bool m_IsDied = false;

    public bool isDied
    {
        get { return m_IsDied; }
    }


    void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Spawn()
    {
        float x = MainCameraManager.Instance.GetCameraCornerPos(1,0).x + m_renderer.bounds.size.x / 2;
        float y = MainCameraManager.Instance.transform.position.y;

        transform.position = new Vector2(x, y);

        StartCoroutine(SpawnCoroutine());
    }

    protected virtual IEnumerator SpawnCoroutine()
    {
        float StartX = transform.position.x;

        float targetX = MainCameraManager.Instance.GetCameraCornerPos(1, 0).x - m_renderer.bounds.size.x / 2;

        float currentTime = 0;

        while (currentTime <= m_SpawnTime)
        {
            float x = Mathf.Lerp(StartX,targetX, currentTime / m_SpawnTime);

            float y = MainCameraManager.Instance.transform.position.y;

            transform.position = new Vector2(x, y);

            currentTime += Time.deltaTime;

            yield return null;
        }

        m_IsSpawn = true;

        Spawned();
    }

    protected virtual void Spawned()
    {

    }

    public virtual void ApplyDamage(int damage)
    {
        if((m_HP -= damage) <= 0)
        {
            m_IsDied = true;

            Death();
        }
    }

    public virtual void Death()
    {
        GameManager.Instance.GameClear();
    }
}
