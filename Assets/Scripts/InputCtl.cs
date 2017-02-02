using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class InputCtl : MonoBehaviour {

    public GameObject JumpButton;
    public GameObject JoySteck;

	// Use this for initialization
	void Start () {

        JumpButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //飛行状態のボタン配置にする
    public void FlyButtonSet()
    {
        JumpButton.SetActive(false);
        JoySteck.SetActive(true);
    }

    //走行状態のボタン配置にする
    public void RunButtonSet()
    {
        JoySteck.SetActive(false);
        JumpButton.SetActive(true);
    }
}
