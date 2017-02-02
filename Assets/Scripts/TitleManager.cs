using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    public AudioClip BGM;

    private AudioSource Audio;

	// Use this for initialization
	void Start () {
        Audio = GetComponent<AudioSource>();

        Audio.PlayOneShot(BGM);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("FadeObject").GetComponent<FadeSystem>().Fade(false, "Main");
        }

    }
}
