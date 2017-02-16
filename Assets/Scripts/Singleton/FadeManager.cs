using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{

    //フェード色
    public Color m_FadeColor = Color.black;

    //フェードしているかどうか
    [HideInInspector]
    public bool m_IsFadeing;

    //フェードテクスチャを表示するか
    private bool m_IsDisp = true;

    //シーン移行するか
    private bool m_IsLoadScene = false;

    //=============================================================================
    //
    // Purpose : Awake処理．
    //
    //=============================================================================
    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);

        FadeIn();
    }

    //=============================================================================
    //
    // Purpose : GUI描画．
    //
    //=============================================================================
    public void OnGUI()
    {
        if (m_IsDisp)
        {
            GUI.color = m_FadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        }
    }

    //=============================================================================
    //
    // Purpose : シーン移動関数．
    //
    //=============================================================================

    public void LoadScene(string name, float fadeOutTime = 2f ,float fadeInTime = 2f)
    {
        if (!m_IsFadeing && !m_IsLoadScene)
        {
            m_IsLoadScene = true;

            StartCoroutine(LoadingScene(name, fadeOutTime, fadeInTime));
        }
    }

    //=============================================================================
    //
    // Purpose : シーン移動のコルーチン．
    //
    //=============================================================================

    private IEnumerator LoadingScene(string name, float fadeOutTime = 2f, float fadeInTime = 2f)
    {
        FadeOut(fadeOutTime);

        while (m_IsFadeing)
        {
            yield return null;
        }

        SceneManager.LoadScene(name);

        FadeIn(fadeInTime);

        while (m_IsFadeing)
        {
            yield return null;
        }

        m_IsLoadScene = false;
    }

    //=============================================================================
    //
    // Purpose : フェードインの開始関数．
    //
    //=============================================================================
    public void FadeIn(float fadeTime = 2f)
    {
        if (!m_IsFadeing)
        {
            m_IsFadeing = true;

            StartCoroutine(FadingIn(fadeTime));
        }
    }

    //=============================================================================
    //
    // Purpose : フェードインのコルーチン．
    //
    // Return : ．
    //
    //=============================================================================
    private IEnumerator FadingIn(float fadeTime)
    {
        float currentTime = 0;
        while (currentTime <= fadeTime)
        {
            m_FadeColor.a = Mathf.Lerp(1f, 0f, currentTime / fadeTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        m_IsDisp = false;
        m_IsFadeing = false;
    }

    //=============================================================================
    //
    // Purpose : フェードアウトの開始関数．
    //
    //=============================================================================
    public void FadeOut(float fadeTime = 2f)
    {
        if (!m_IsFadeing)
        {
            m_IsFadeing = true;
            m_IsDisp = true;

            StartCoroutine(FadingOut(fadeTime));
        }
    }

    //=============================================================================
    //
    // Purpose : フェードアウトのコルーチン．
    //
    // Return : ．
    //
    //=============================================================================
    private IEnumerator FadingOut(float fadeTime)
    {
        float currentTime = 0;
        while (currentTime <= fadeTime)
        {
            m_FadeColor.a = Mathf.Lerp(0f, 1f, currentTime / fadeTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        m_IsFadeing = false;
    }
}
