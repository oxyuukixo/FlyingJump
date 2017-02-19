using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargoyleKing : Boss {

    public AnimationCurve m_StayCurve;

    public float m_StayLoopTime = 3;

    public float m_StayMoveHeight;

    public GameObject m_DeathExplosion;

    public int m_DeathExplotionNum;

    public float m_DeathExplosionInterval;

    public float m_DeathExplosionAddSpeed;

	// Use this for initialization
	void Start () {

       

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void Spawned()
    {
        StartCoroutine(StayCorutine());
    }

    IEnumerator StayCorutine()
    {
        float currentTime = 0;

        while(!isDied)
        {
            float rate = m_StayCurve.Evaluate(currentTime / m_StayLoopTime);

            float y = MainCameraManager.Instance.transform.position.y + rate * m_StayMoveHeight;

            transform.position = new Vector2(transform.position.x, y);

            if((currentTime += Time.deltaTime) > m_StayLoopTime)
            {
                currentTime -= m_StayLoopTime;
            }

            yield return null;
        }
    }

    IEnumerator DeathExplosion()
    {
        int currentExplosionNum = 0;

        float currentInterval = 0;
        float currentDeatExplosionInterval = m_DeathExplosionInterval;

        while(currentExplosionNum < m_DeathExplotionNum)
        {
            if((currentInterval += Time.deltaTime) > currentDeatExplosionInterval)
            {
                float x = Random.Range(transform.position.x - m_renderer.bounds.size.x / 2, transform.position.x + m_renderer.bounds.size.x / 2);
                float y = Random.Range(transform.position.y - m_renderer.bounds.size.y / 2, transform.position.y + m_renderer.bounds.size.y / 2);

                GameObject explosion = Instantiate(m_DeathExplosion);
                explosion.transform.position = new Vector2(x, y);

                currentDeatExplosionInterval -= m_DeathExplosionAddSpeed;
                currentInterval = 0;
                currentExplosionNum++;
            }

            yield return null;
        }

        FadeManager.Instance.m_FadeColor = new Color(255, 255, 255);
        FadeManager.Instance.FadeFanction(NotDisp, Died);
    }

    private void NotDisp()
    {
        m_renderer.enabled = false;
    }

    private void Died()
    {
        FadeManager.Instance.m_FadeColor = new Color(0, 0, 0);
        GameManager.Instance.GameClear();
    }

    public override void Death()
    {
        GetComponent<PolygonCollider2D>().enabled = false;

        StartCoroutine(DeathExplosion());

        ScrollManager.Instance.ScrollStop();
    }
    
}
