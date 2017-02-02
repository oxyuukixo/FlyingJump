using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSystem : MonoBehaviour {

    public float FadeTime;

    public Color FadeColor;

    private Image FadeImage;

    private float NowTime;

    private float FadeSpeedRed;
    private float FadeSpeedGreen;
    private float FadeSpeedBlue;
    private float FadeSpeedAlpha;

    private bool IsFade = false;
    private bool IsFadeIn;

    private bool IsLoadScene = false;
    private string LoadName;

    // Use this for initialization
    void Start () {

        FadeImage = GetComponent<Image>();
        FadeImage.color = FadeColor;

        FadeSpeedRed = FadeColor.r / FadeTime;
        FadeSpeedGreen = FadeColor.g / FadeTime;
        FadeSpeedBlue = FadeColor.b / FadeTime;
        FadeSpeedAlpha = FadeColor.a / FadeTime;

        Fade(true);
    }
	
	// Update is called once per frame
	void Update () {
	
        if (IsFade)
        {
            if(IsFadeIn)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }

            if((NowTime += Time.deltaTime) > FadeTime)
            {
                IsFade = false;

                if(IsLoadScene)
                {
                    SceneManager.LoadScene(LoadName);
                }
            }
        }

	}

    private void FadeIn()
    {
        FadeImage.color -= new Color(FadeSpeedRed, FadeSpeedGreen, FadeSpeedBlue, FadeSpeedAlpha) * Time.deltaTime;
    }

    private void FadeOut()
    {
        FadeImage.color += new Color(FadeSpeedRed, FadeSpeedGreen, FadeSpeedBlue, FadeSpeedAlpha) * Time.deltaTime;
    }

    public void Fade(bool FadeIn)
    {
        IsFade = true;
        IsFadeIn = FadeIn;
        NowTime = 0;
    }

    public void Fade(bool FadeIn,string SceneName)
    {
        IsFade = true;
        IsFadeIn = FadeIn;
        NowTime = 0;

        IsLoadScene = true;
        LoadName = SceneName;
    }
}
