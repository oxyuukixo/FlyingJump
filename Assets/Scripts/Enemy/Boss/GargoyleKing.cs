using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargoyleKing : Boss {

    public AnimationCurve m_StayCurve;

    public float m_StayLoopTime = 3;

    public float m_StayMoveHeight;

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

}
