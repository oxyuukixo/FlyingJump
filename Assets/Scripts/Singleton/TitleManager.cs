using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SingletonMonoBehaviour<TitleManager> {

	// Use this for initialization
	void Awake () {

        if (this != Instance)
        {
            Destroy(this);
            return;
        }
	}

    void Start()
    {
        SoundManager.Instance.LoadBGM("Title", "title", true);
        SoundManager.Instance.LoadSE("enter", "enter", true);

        SoundManager.Instance.PlayBGM("Title");
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.Instance.PlaySE("enter");

            SoundManager.Instance.FadeOutBGM(3f);
            FadeManager.Instance.LoadScene("Stage01",3f);
           
        }
    }
}
