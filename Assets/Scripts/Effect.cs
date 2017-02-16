using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {

    public string m_AudioName;

	// Use this for initialization
	void Start () {

        SoundManager.Instance.LoadSE(m_AudioName, m_AudioName, true);
        SoundManager.Instance.PlaySE(m_AudioName);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MyDestroy()
    {
        Destroy(this.gameObject);
    }
}
