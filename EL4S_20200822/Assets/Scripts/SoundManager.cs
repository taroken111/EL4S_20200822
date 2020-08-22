using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));

                if (instance == null)
                {
                    GameObject mg = (GameObject)Resources.Load("Prefabs/SoundManager");
                    Instantiate<GameObject>(mg);
                }
            }

            return instance;
        }
    }

    #endregion Singleton


    /*
        [SerializeField, Range(0, 1), Tooltip("マスタ音量")]
        float volume = 1;
        [SerializeField, Range(0, 1), Tooltip("BGMの音量")]
        float bgmVolume = 1;
        [SerializeField, Range(0, 1), Tooltip("SEの音量")]
        float seVolume = 1;
    */
    public const float DEFAULT_BGM_VOLUME = 0.15f;
    public const float DEFAULT_SE_VOLUME = 1.0f;

    BgmTable BgmDataTable; // 外部ファイル(Audio/BGM のscriptable)
    SeTable SeDataTable;  // 外部ファイル(Audio/SE  のscriptable)

    AudioSource bgmAudioSource; // 流れているbgm

    public AudioSource CurrentAudioSource = null; // 現在流れているbgm

    /// <summary>
    /// FadeOut中、もしくは再生待機中のAudioSource
    /// </summary>
    public AudioSource SubAudioSource
    {
        get
        {
            //bgmSourcesのうち、CurrentAudioSourceでない方を返す
            if (this.AudioSources == null)
                return null;
            foreach (AudioSource s in this.AudioSources)
            {
                if (s != this.CurrentAudioSource)
                {
                    return s;
                }
            }
            return null;
        }
    }
    private List<AudioSource> AudioSources = null; // クロスフェード用の二つのbgm

    AudioSource seAudioSource;

    /// <summary>
    /// 再生音量
    /// 次回フェードインから適用されます。
    /// 再生中の音量を変更するには、CurrentAudioSource.Volumeを変更してください。
    /// </summary>
    [Range(0f, 1f)]
    public float TargetVolume = 1.0f;
    /// <summary>
    /// フェードイン、フェードアウトにかかる時間です。
    /// </summary>
    public float TimeToFade = 2.0f;
    /// <summary>
    /// フェードインとフェードアウトの実行を重ねる割合です。
    /// 0を指定すると、完全にフェードアウトしてからフェードインを開始します。
    /// 1を指定すると、フェードアウトとフェードインを同時に開始します。
    /// </summary>
    [Range(0f, 1f)]
    public float CrossFadeRatio = 1.0f;

    /// <summary>
    /// コルーチン中断に使用
    /// </summary>
    private IEnumerator fadeOutCoroutine;
    /// <summary>
    /// コルーチン中断に使用
    /// </summary>
    private IEnumerator fadeInCoroutine;


    /// <summary>
    /// SE再生音量
    /// 次回フェードインから適用されます。
    /// 再生中の音量を変更するには、CurrentAudioSource.Volumeを変更してください。
    /// </summary>
    [Range(0f, 1f)]
    public float SEVolume = 1.0f;

    /// <summary>
    /// BGM再生音量
    /// 次回フェードインから適用されます。
    /// 再生中の音量を変更するには、CurrentAudioSource.Volumeを変更してください。
    /// </summary>
    [Range(0f, 1f)]
    public float BGMVolume = 1.0f;

    bool m_volumefading;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        seAudioSource = gameObject.AddComponent<AudioSource>();

        BgmDataTable = BgmTable.Entity;
        SeDataTable = SeTable.Entity;


        //AudioSourceを２つ用意。クロスフェード時に同時再生するために２つ用意する。
        AudioSources = new List<AudioSource>();
        AudioSources.Add(gameObject.AddComponent<AudioSource>());
        AudioSources.Add(gameObject.AddComponent<AudioSource>());
        foreach (AudioSource s in this.AudioSources)
        {
            s.playOnAwake = false;
            s.volume = 0f;
            s.loop = true;
        }

        BGMVolume = PlayerPrefs.GetFloat("BGMVolume", SoundManager.DEFAULT_BGM_VOLUME);
        SEVolume = PlayerPrefs.GetFloat("SEVolume", SoundManager.DEFAULT_BGM_VOLUME);
    }


    private bool PlaySound(AudioSource audioSource, AudioClip audioClip, float volume)
    {
        if (audioClip == null || volume <= 0.0f) return false; // オーディオの中身がない場合と音が0以下の場合false
        audioSource.clip = audioClip;
        audioSource.volume = volume * BGMVolume;
        audioSource.Play();

        return true;
    }
    public void PlayBgm(AudioClip audioClip)
    {

        // 現在流している曲と同じ場合早期リターンする
        if ((this.CurrentAudioSource != null)
            && (this.CurrentAudioSource.clip == audioClip))
        {
            //すでに指定されたBGMを再生中
            return;
        }
        //再生中のBGMをフェードアウト開始
        Stop();


        float fadeInStartDelay = TimeToFade * (1.0f - CrossFadeRatio);

        //BGM再生開始
        CurrentAudioSource = SubAudioSource;
        CurrentAudioSource.clip = audioClip;
        fadeInCoroutine = fadeIn(CurrentAudioSource, TimeToFade, CurrentAudioSource.volume, TargetVolume, fadeInStartDelay);
        StartCoroutine(fadeInCoroutine);
    }

    /// <summary>
    /// BGMを停止します。
    /// </summary>
    public void Stop()
    {
        if (this.CurrentAudioSource != null)
        {
            this.fadeOutCoroutine = fadeOut(this.CurrentAudioSource, this.TimeToFade, this.CurrentAudioSource.volume, 0f);
            StartCoroutine(this.fadeOutCoroutine);
        }
    }

    /// <summary>
    /// BGMをただちに停止します。
    /// </summary>
    public void StopImmediately()
    {
        this.fadeInCoroutine = null;
        this.fadeOutCoroutine = null;
        foreach (AudioSource s in this.AudioSources)
        {
            s.Stop();
        }
        this.CurrentAudioSource = null;
        m_volumefading = false;
    }

    /// <summary>
    /// BGMをフェードインさせながら再生を開始します。
    /// </summary>
    /// <param name="bgm">AudioSource</param>
    /// <param name="timeToFade">フェードインにかかる時間</param>
    /// <param name="fromVolume">初期音量</param>
    /// <param name="toVolume">フェードイン完了時の音量</param>
    /// <param name="delay">フェードイン開始までの待ち時間</param>
    private IEnumerator fadeIn(AudioSource bgm, float timeToFade, float fromVolume, float toVolume, float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }


        float startTime = Time.time;
        m_volumefading = true;
        bgm.Play();
        while (true)
        {
            float spentTime = Time.time - startTime;
            if (spentTime > timeToFade)
            {
                bgm.volume = toVolume * BGMVolume;
                this.fadeInCoroutine = null;
                break;
            }

            float rate = spentTime / timeToFade;
            float vol = Mathf.Lerp(fromVolume, toVolume, rate);
            bgm.volume = vol * BGMVolume;
            yield return null;
        }
        m_volumefading = false;
    }

    private IEnumerator fadeOut(AudioSource bgm, float timeToFade, float fromVolume, float toVolume)
    {
        float startTime = Time.time;
        while (true)
        {
            float spentTime = Time.time - startTime;
            if (spentTime > timeToFade)
            {
                bgm.volume = toVolume;
                bgm.Stop();
                this.fadeOutCoroutine = null;
                break;
            }

            float rate = spentTime / timeToFade;
            float vol = Mathf.Lerp(fromVolume, toVolume, rate);
            bgm.volume = vol;
            yield return null;
        }
    }


    /// <summary>
    /// フェードイン処理を中断します。
    /// </summary>
    private void stopFadeIn()
    {
        if (this.fadeInCoroutine != null)
            StopCoroutine(this.fadeInCoroutine);
        this.fadeInCoroutine = null;

    }

    /// <summary>
    /// フェードアウト処理を中断します。
    /// </summary>
    private void stopFadeOut()
    {
        if (this.fadeOutCoroutine != null)
            StopCoroutine(this.fadeOutCoroutine);
        this.fadeOutCoroutine = null;
    }



    //SE再生
    public void PlaySe(AudioClip audioClip, float volume)
    {
        seAudioSource.PlayOneShot(audioClip, volume * SEVolume);
    }


    //3D空間上でSEを再生する
    public void PlaySeAt(AudioClip audioClip, float volume, Vector3 position)
    {
        var obj = Instantiate(SeTable.Entity.OneShotSoundObj);
        obj.transform.position = position;
        var source = obj.GetComponent<AudioSource>();
        source.volume = volume * SEVolume;
        source.PlayOneShot(audioClip);
    }

    public void StopSe()
    {
        seAudioSource.Stop();
        seAudioSource.clip = null;
    }

    public void SetBGMVolume(float value)
    {
        BGMVolume = value;
        if (!m_volumefading && this.CurrentAudioSource != null)
        {
            this.CurrentAudioSource.volume = value;
        }
    }

}
