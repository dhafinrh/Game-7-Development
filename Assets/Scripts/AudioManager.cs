using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // static AudioSource bgmInstance;
    // static AudioSource sfxInstance;
    [SerializeField] private AudioSource bgm, sfx;

    public bool IsMute { get => bgm.mute; }
    public float BgmVolume { get => bgm.volume; }
    public float SfxVolume { get => sfx.volume; }

    private void Awake()
    {
        bgm.mute = PlayerPrefs.GetInt("Mute") == 1 ? true : false;
        sfx.mute = PlayerPrefs.GetInt("Mute") == 1 ? true : false;
        bgm.volume = PlayerPrefs.GetFloat("BGM Volume", 0.2f);
        sfx.volume = PlayerPrefs.GetFloat("SFX Volume", 0.5f);

        // if (bgmInstance != null)
        // {
        //     Destroy(bgm.gameObject);
        //     bgm = bgmInstance;
        // }
        // else
        // {
        //     bgmInstance = bgm;
        //     bgm.transform.SetParent(null);
        //     DontDestroyOnLoad(bgm.gameObject);
        // }

        // if (sfxInstance != null)
        // {
        //     Destroy(sfx.gameObject);
        //     sfx = sfxInstance;
        // }
        // else
        // {
        //     sfxInstance = sfx;
        //     sfx.transform.SetParent(null);
        //     DontDestroyOnLoad(sfx.gameObject);
        // }
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgm.isPlaying)
        {
            bgm.Stop();
        }
        bgm.clip = clip;
        bgm.loop = loop;
        bgm.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfx.isPlaying)
        {
            sfx.Stop();
        }
        sfx.clip = clip;
        sfx.Play();
    }

    public void Mute(bool value)
    {
        if (bgm != null)
        {
            bgm.mute = value;
        }
        if (sfx != null)
        {
            sfx.mute = value;
        }
        PlayerPrefs.SetInt("Mute", value ? 1 : 0);
    }

    public void SetBGMVolume(float value)
    {
        bgm.volume = value;
        PlayerPrefs.SetFloat("BGM Volume", value);
    }

    public void SetSFXVolume(float value)
    {
        sfx.volume = value;
        PlayerPrefs.SetFloat("SFX Volume", value);
    }
}
