using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    public GameMode m_GameMode = GameMode.Boss;

    public GameObject m_BossObject;

    private bool m_IsBossEnable = false;

    public string m_BGM;
    public string m_BossBGM;

    public bool BossEnable
    {
        get { return m_IsBossEnable; }
    }

    public GameMode gameMode
    {
        get { return m_GameMode; }
        set { m_GameMode = value; }
    }


    public enum GameMode
    {
        Boss
    }

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);

        SceneManager.activeSceneChanged += SceneChange;
    }
    void SceneChange(Scene oldScene, Scene NewScene)
    {
        if(NewScene.name == "Stage01")
        {
            m_IsBossEnable = false;

            SoundManager.Instance.LoadBGM("BGM", m_BGM, true);

            if(gameMode == GameMode.Boss)
            {
                SoundManager.Instance.LoadBGM("BossBGM", m_BossBGM, true);
            }

            SoundManager.Instance.PlayBGM("BGM",0);
        }
    }

    public void SetGameState(GameMode gameMode,GameObject boss)
    {
        m_GameMode = gameMode;

        m_BossObject = boss;
    }

    public void ScrollEnd()
    {
        switch(m_GameMode)
        {
            case GameMode.Boss:

                if(!m_IsBossEnable)
                {
                    m_IsBossEnable = true;

                    GameObject obj = Instantiate(m_BossObject);
                    obj.GetComponent<Boss>().Spawn();

                    SoundManager.Instance.FadeOutBGM(2f, 0);
                    SoundManager.Instance.FadeInBGM("BossBGM", 2f, 1);
                }

                break;
        }
    }

    public void GameClear()
    {
        StartCoroutine(GameClearCoroutine());

        ScrollManager.Instance.ScrollStop();
    }

    private IEnumerator GameClearCoroutine()
    {
        yield return new WaitForSeconds(2f);

        SoundManager.Instance.FadeOutBGM(2f, 0);
        SoundManager.Instance.FadeOutBGM(2f, 1);

        FadeManager.Instance.LoadScene("Title");
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());

        SoundManager.Instance.FadeOutBGM(2f,0);
        SoundManager.Instance.FadeOutBGM(2f,1);

        ScrollManager.Instance.ScrollStop();
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(2f);

        FadeManager.Instance.LoadScene("Title");
    }
}
