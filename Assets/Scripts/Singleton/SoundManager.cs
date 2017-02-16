using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    //ボリューム
    public float m_BGMVolume = 1f;
    public float m_SEVolume = 1f;

    //チャンネル数
    const int BGM_CHANNEL = 2;
    const int SE_CHANNEL = 4;

    // サウンドリソース
    private AudioSource m_SourceBGM = null; // BGM(デフォルト)
    private AudioSource[] m_SourceArrayBGM; // BGM(チャンネル)
    private AudioSource m_SourceSE = null; // SE (デフォルト)
    private AudioSource[] m_SourceArraySE; // SE (チャンネル)

    // BGMにアクセスするためのテーブル
    private Dictionary<string, Data> m_PoolBGM = new Dictionary<string, Data>();

    // SEにアクセスするためのテーブル 
    private Dictionary<string, Data> m_PoolSE = new Dictionary<string, Data>();

    //シーン移動時に開放するサウンドのリスト
    private List<string> m_SceneOnlyBGM = new List<string>();
    private List<string> m_SceneOnlySE = new List<string>();

    //フェード時のコルーチン
    private List<IEnumerator> m_FadeIEnumerator = new List<IEnumerator>();

    //サウンドの種類
    private enum SoundType
    {
        BGM,
        SE
    }

    /// 保持するデータ
    private class Data
    {
        /// アクセス用のキー
        public string m_Key;
        /// リソース名
        public string m_FileName;
        /// AudioClip
        public AudioClip m_Clip;

        /// コンストラクタ
        public Data(SoundType type, string key, string fileName)
        {
            m_Key = key;

            switch (type)
            {
                case SoundType.BGM:

                    m_FileName = "Sounds/BGM/" + fileName;

                    break;

                case SoundType.SE:

                    m_FileName = "Sounds/SE/" + fileName;

                    break;
            }

            // AudioClipの取得
            m_Clip = Resources.Load(m_FileName) as AudioClip;
        }
    }

    //=============================================================================
    //
    // Purpose : Awake処理．
    //
    //=============================================================================
    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this);

        //オーディオソースの作成

        //BGM
        m_SourceBGM = gameObject.AddComponent<AudioSource>();

        m_SourceArrayBGM = new AudioSource[BGM_CHANNEL];
        for (int i = 0; i < BGM_CHANNEL; i++)
        {
            m_SourceArrayBGM[i] = gameObject.AddComponent<AudioSource>();
        }

        //SE
        m_SourceSE = gameObject.AddComponent<AudioSource>();

        m_SourceArraySE = new AudioSource[SE_CHANNEL];
        for (int i = 0; i < SE_CHANNEL; i++)
        {
            m_SourceArraySE[i] = gameObject.AddComponent<AudioSource>();
        }

        SceneManager.sceneLoaded += DeleteSceneSound;
    }

    //=============================================================================
    //
    // Purpose : オーディオソースの取得．
    //
    // Return : 取得したオーディオソース
    //
    //=============================================================================
    AudioSource GetAudioSource(SoundType type, int channel = -1)
    {
        switch (type)
        {
            case SoundType.BGM:

                if (0 <= channel && channel < BGM_CHANNEL)
                {
                    // チャンネル指定
                    return m_SourceArrayBGM[channel];
                }
                else
                {
                    // デフォルト
                    return m_SourceBGM;
                }

            case SoundType.SE:

                if (0 <= channel && channel < SE_CHANNEL)
                {
                    // チャンネル指定
                    return m_SourceArraySE[channel];
                }
                else
                {
                    // デフォルト
                    return m_SourceSE;
                }

            default:

                Debug.Log("Is nothing SoundType");

                return null;
        }
    }

    // サウンドのロード
    // ※Resources/Soundsフォルダに配置すること
    //=============================================================================
    //
    // Purpose : BGMのロード関数．
    //
    //=============================================================================
    public void LoadBGM(string key, string fileName, bool onlyScene = false)
    {
        if (m_PoolBGM.ContainsKey(key))
        {
            //// すでに登録済みなのでいったん消す
            //if (m_SourceBGM.clip == m_PoolBGM[key].m_Clip)
            //{
            //    m_SourceBGM.clip = null;
            //}

            //foreach (AudioSource audio in m_SourceArrayBGM)
            //{
            //    if (audio.clip == m_PoolBGM[key].m_Clip)
            //    {
            //        audio.clip = null;
            //    }
            //}

            //m_PoolBGM.Remove(key);

            return;
        }

        m_PoolBGM.Add(key, new Data(SoundType.BGM, key, fileName));

        //このシーンだけで取得したい場合
        if (onlyScene)
        {
            m_SceneOnlyBGM.Add(key);
        }
    }

    //=============================================================================
    //
    // Purpose : SEのロード関数．
    //
    //=============================================================================
    public void LoadSE(string key, string fileName, bool onlyScene = false)
    {
        if (m_PoolSE.ContainsKey(key))
        {
            //// すでに登録済みなのでいったん消す
            //if (m_SourceSE.clip == m_PoolSE[key].m_Clip)
            //{
            //    m_SourceSE.clip = null;
            //}

            //foreach (AudioSource audio in m_SourceArraySE)
            //{
            //    if (audio.clip == m_PoolSE[key].m_Clip)
            //    {
            //        audio.clip = null;
            //    }
            //}

            //m_PoolSE.Remove(key);

            return;
        }

        m_PoolSE.Add(key, new Data(SoundType.SE, key, fileName));

        //このシーンだけで取得したい場合
        if (onlyScene)
        {
            m_SceneOnlySE.Add(key);
        }
    }

    //=============================================================================
    //
    // Purpose : BGMの再生関数．
    //
    // Return : 再生開始できたか
    //
    //=============================================================================
    public bool PlayBGM(string key,int channel = -1)
    {
        AudioSource playAudio;

        if (m_PoolBGM.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }
        if (0 <= channel && channel < BGM_CHANNEL)
        {
            playAudio = GetAudioSource(SoundType.BGM, channel);
        }
        else
        {
            playAudio = GetAudioSource(SoundType.BGM);
        }

        // いったん止める
        playAudio.Stop();

        // リソースの取得
        var data = m_PoolBGM[key];

        playAudio.loop = true;
        playAudio.clip = data.m_Clip;
        playAudio.volume = m_BGMVolume;
        playAudio.Play();

        return true;
    }

    public void UnPauseBGM(int channel = -1)
    {
        GetAudioSource(SoundType.BGM).UnPause();

        for(int i = 0; i < BGM_CHANNEL;i++)
        {
            GetAudioSource(SoundType.BGM, i).UnPause();
        }

        for(int i = 0; i < m_FadeIEnumerator.Count;i++)
        {
            StopCoroutine(m_FadeIEnumerator[i]);
        }

    }

    public bool FadeInBGM(string key,float fadeTime,int channel = -1)
    {
        AudioSource playAudio;

        if (m_PoolBGM.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        if (0 <= channel && channel < BGM_CHANNEL)
        {
            playAudio = GetAudioSource(SoundType.BGM, channel);
        }
        else
        {
            playAudio = GetAudioSource(SoundType.BGM);
        }

        // いったん止める
        playAudio.Stop();

        // リソースの取得
        var data = m_PoolBGM[key];

        playAudio.loop = true;
        playAudio.clip = data.m_Clip;
        playAudio.volume = 0;
        playAudio.Play();

        m_FadeIEnumerator.Add(FadingInBGM(playAudio, fadeTime,m_FadeIEnumerator.Count));

        StartCoroutine(m_FadeIEnumerator[m_FadeIEnumerator.Count - 1]);

        return true;
    }

    private IEnumerator FadingInBGM(AudioSource audio, float fadeTime,int mynum)
    {
        IEnumerator my = m_FadeIEnumerator[mynum];

        float currentTime = 0;
        while (currentTime <= fadeTime)
        {
            audio.volume = Mathf.Lerp(0f, 1f, currentTime / fadeTime * m_BGMVolume);
            currentTime += Time.deltaTime;
            yield return null;
        }

        m_FadeIEnumerator.Remove(my);
    }

    /// BGMの停止
    //=============================================================================
    //
    // Purpose : BGMの停止関数．
    //
    // Return : BGMを停止できたか
    //
    //=============================================================================
    public void StopBGM(int channel = -1)
    {
        AudioSource stopAudio;

        if (0 <= channel && channel < BGM_CHANNEL)
        {
            stopAudio = GetAudioSource(SoundType.BGM, channel);
        }
        else
        {
            stopAudio = GetAudioSource(SoundType.BGM);
        }

        stopAudio.Stop();
    }

    public void FadeOutBGM(float fadeTime,int channel = -1)
    {
        AudioSource stopAudio;

        if (0 <= channel && channel < BGM_CHANNEL)
        {
            stopAudio = GetAudioSource(SoundType.BGM, channel);
        }
        else
        {
            stopAudio = GetAudioSource(SoundType.BGM);
        }

        m_FadeIEnumerator.Add(FadingOutBGM(stopAudio, fadeTime, m_FadeIEnumerator.Count));

        StartCoroutine(m_FadeIEnumerator[m_FadeIEnumerator.Count - 1]);
    }

    private IEnumerator FadingOutBGM(AudioSource audio,float fadeTime,int mynum)
    {
        IEnumerator my = m_FadeIEnumerator[mynum];

        float currentTime = 0;

        while (currentTime <= fadeTime)
        {
            audio.volume = Mathf.Lerp(m_BGMVolume, 0f, currentTime / fadeTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        m_FadeIEnumerator.Remove(my);

        audio.Stop();
    }

    public void PauseBGM(int channel = -1)
    {
        GetAudioSource(SoundType.BGM).Pause();

        for (int i = 0; i < BGM_CHANNEL; i++)
        {
            GetAudioSource(SoundType.BGM, i).Pause();
        }

        for (int i = 0; i < m_FadeIEnumerator.Count; i++)
        {
            StartCoroutine(m_FadeIEnumerator[i]);
        }
    }

    /// SEの再生
    /// ※事前にLoadSeでロードしておくこと
    //=============================================================================
    //
    // Purpose : SEの再生関数．
    //
    // Return : 再生開始できたか
    //
    //=============================================================================
    public bool PlaySE(string key, int channel = -1)
    {
        if (m_PoolSE.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var data = m_PoolSE[key];

        if (0 <= channel && channel < SE_CHANNEL)
        {
            // チャンネル指定
            AudioSource source = GetAudioSource(SoundType.SE, channel);
            source.clip = data.m_Clip;
            source.volume = m_SEVolume;
            source.Play();
        }
        else
        {
            // デフォルトで再生
            AudioSource source = GetAudioSource(SoundType.SE);
            source.volume = m_SEVolume;
            source.PlayOneShot(data.m_Clip);
        }

        return true;
    }

    //=============================================================================
    //
    // Purpose : シーン移動時のサウンド削除関数．
    //
    //=============================================================================
    void DeleteSceneSound(Scene scenename,LoadSceneMode SceneMode)
    {
        foreach(string key in m_SceneOnlyBGM)
        {
            if(m_SourceBGM.clip == m_PoolBGM[key].m_Clip)
            {
                m_SourceBGM.clip = null;
            }

            foreach(AudioSource audio in m_SourceArrayBGM)
            {
                if(audio.clip == m_PoolBGM[key].m_Clip)
                {
                    audio.clip = null;
                }
            }

            m_PoolBGM.Remove(key);
        }

        foreach (string key in m_SceneOnlySE)
        {
            if (m_SourceSE.clip == m_PoolSE[key].m_Clip)
            {
                m_SourceSE.clip = null;
            }

            foreach (AudioSource audio in m_SourceArraySE)
            {
                if (audio.clip == m_PoolSE[key].m_Clip)
                {
                    audio.clip = null;
                }
            }

            m_PoolSE.Remove(key);
        }

        m_SceneOnlyBGM.Clear();
        m_SceneOnlySE.Clear();
    }
}