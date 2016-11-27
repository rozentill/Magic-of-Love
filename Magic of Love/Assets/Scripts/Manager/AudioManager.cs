using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 2016.05.31 SoundManager
/// Manage the play and stop of bgm and se.
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class AudioManager : BehaviorSingleton<AudioManager> {

    public float fadeTime = 1f;
    public float normalVolume = 1f;
    
    AudioSource bgm, voice;
    BGM nextBGM = BGM.BGM_NONE;
    bool _isPlayBGM = false;
    private float _currentFadeTime = 0f;

    private bool _playLock = false;

    public static BGM currentBGM = BGM.BGM_NONE;

    List<AudioSource> se = new List<AudioSource>();

    public SoundName soundName = new SoundName();

    void Awake()
    {
        bgm = GetComponent<AudioSource>();
        bgm.loop = true;

        voice = gameObject.AddComponent<AudioSource>();
        voice.loop = false;
        
    }

    void Update()
    {
        for (int i = 0; i < se.Count; i++)
        {
            if (!se[i].isPlaying)
            {
                Destroy(se[i]);
                se.RemoveAt(i);
            }
        }
    }

    public void Play(BGM bgmName)
    {
        if (!_playLock && nextBGM != bgmName)
        {
            _playLock = true;

            nextBGM = bgmName;

            print("SoundManager: Start Play BGM " + nextBGM.ToString());

            if (_isPlayBGM)
            {
                _currentFadeTime = 0f;
                StartCoroutine(FadeCoroutine());
            }
            else
                PlayIn();
        }
        else
        {
            print("SoundManager: Start Play BGM " + bgmName + " " + _playLock);
        }
    }

    private void PlayIn()
    {
        print("SoundManager: Finish Play.");
        bgm.clip = (AudioClip)Resources.Load(soundName.GetSoundPath(nextBGM));
        bgm.volume = normalVolume; 
        bgm.Play();
        _isPlayBGM = true;
        _playLock = false;
        currentBGM = nextBGM;
    }

    public void Play(SE seName)
    {
        se.Add(this.gameObject.AddComponent<AudioSource>());
        se[se.Count - 1].clip = (AudioClip)Resources.Load(soundName.GetSoundPath(seName));
        se[se.Count - 1].Play();
    }

    public AudioSource Play(SE seName, bool loop)
    {
        se.Add(this.gameObject.AddComponent<AudioSource>());
        se[se.Count - 1].clip = (AudioClip)Resources.Load(soundName.GetSoundPath(seName));
        se[se.Count - 1].loop = loop;
        se[se.Count - 1].Play();
        return se[se.Count - 1];
    }


    public void StopBGM()
    {
        if (!_playLock && _isPlayBGM)
        {
            print("SoundManager: Start Stop BGM " + nextBGM.ToString());
 
            nextBGM = BGM.BGM_NONE;
            _currentFadeTime = 0f;
            _isPlayBGM = false;
            StartCoroutine(StopCoroutine());
        }
    }

    public void StopSE()
    {
        se.Clear();
    }

    public void StopSE(AudioSource sound)
    {
        if (sound != null)
        {
            Destroy(sound);
            se.Remove(sound);
        }
    }

    private IEnumerator StopCoroutine()
    {
        if (!_isPlayBGM) 
        {
            if (_currentFadeTime < fadeTime)
            {
                bgm.volume = normalVolume * (1 - _currentFadeTime / fadeTime);
                _currentFadeTime += 0.05f;
                yield return new WaitForSeconds(0.05f);
                StartCoroutine(StopCoroutine());
            }
            else
            {
                print("SoundManager: Finish Stop.");
                bgm.Stop();
            }
        }
    }

    private IEnumerator FadeCoroutine()
    {
        if (_currentFadeTime < fadeTime)
        {
            bgm.volume = normalVolume * (1 - _currentFadeTime / fadeTime);
            _currentFadeTime += 0.05f;
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(FadeCoroutine());
        }
        else
        {
            bgm.Stop();
            PlayIn();
        }
    }

    public float PlayVoice(int index)
    {
        voice.Stop();
        voice.clip = (AudioClip)Resources.Load(soundName.GetVoicePath(index));
        voice.Play();
        return voice.clip.length;
    }

    public void StopVoice()
    {
        voice.Stop();
    }

    public void Pause()
    {
        AudioSource[] seAll = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < seAll.Length; i++)
            seAll[i].Pause();
    }

    public void UnPause()
    {
        AudioSource[] seAll = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < seAll.Length; i++)
            seAll[i].UnPause();
    }
}
