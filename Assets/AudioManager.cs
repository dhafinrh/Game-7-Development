using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public AudioSource sfxSource;
    public Slider SfxSlider;
    
    public AudioSource bgmSource;
    public Slider BgmSlider;
    
    public AudioSource FootStepSource;
    public Slider FootStepSlider;
    
    public AudioSource AmbientSource;
    public Slider AmbientSlider;
    
    public float sfxVolume;
    public float bgmVolume;
    public float FootStepVolume;
    public float AmbientVolume;

    public bool isMuted;

    private void Start() {
        // Load the saved values from PlayerPrefs
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        bgmVolume = PlayerPrefs.GetFloat("bgmVolume");
        // isMuted = PlayerPrefs.GetBool("isMuted");

        // Set the audio sources' volumes
        sfxSource.volume = sfxVolume;
        bgmSource.volume = bgmVolume;
        
        BgmSlider.value = bgmVolume;

        // Set the audio manager's mute state
        SetMuted(isMuted);
    }

    public void BGMVol(float value) {
        // Update the audio sources' volumes
        // sfxSource.volume = value;
        float BgmVol = value;

        // Save the new values to PlayerPrefs
        // PlayerPrefs.SetFloat("sfxVolume", value);
        PlayerPrefs.SetFloat("bgmVolume", value);
    }

    public void SetMuted(bool muted) {
        // Set the audio manager's mute state
        isMuted = muted;

        // Mute or unmute the audio sources
        sfxSource.mute = isMuted;
        bgmSource.mute = isMuted;

        // Save the new mute state to PlayerPrefs
        // PlayerPrefs.SetBool("isMuted", isMuted);
    }
}

// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Audio;
// using System.IO;
// using UnityEngine.UI;
// using static UnityEngine.JsonUtility;

// public class AudioManager : MonoBehaviour{

//     public AudioSource sfxSource;
//     public Slider sfxSlider;
//     public AudioSource bgmSource;
//     public Slider bgmSlider;
//     public AudioSource FootStepSrc;
//     public Slider FootStepSlider;
//     public AudioSource Ambient;
//     public Slider AmbientSlider;

//     public float sfxVolume;
//     public float bgmVolume;
//     public float FootStepVol;
//     public float AmbientVol;
//     public bool isMuted;

//     private void Start() {
//         // Load the saved values from JsonUtility
//         var settings = JsonUtility.FromJson<AudioSettings>(File.ReadAllText("AudioSettings.json"));
//         sfxVolume = settings.sfxVolume;
//         bgmVolume = settings.bgmVolume;
//         isMuted = settings.isMuted;
//         FootStepVol = settings.FootStepVol;
//         AmbientVol = settings.AmbientVolume;
//         bgmSlider = settings.bgmSlider;

//         // Set the audio sources' volumes
//         sfxSource.volume = sfxVolume;
//         bgmSource.volume = bgmVolume;
//         FootStepSrc.volume = FootStepVol;
//         Ambient.volume = AmbientVol;
        
//         // set Slider
//         sfxSlider.value = sfxVolume;
//         bgmSlider.value = bgmVolume;
//         FootStepSlider.value = FootStepVol;
//         AmbientSlider.value = AmbientVol;

//         // Set the audio manager's mute state
//         SetMuted(isMuted);
//     }

//     public void BGMVol(float value) {
//         // Update the audio sources' volumes
        
//         bgmSource.volume = value;
//         FootStepSrc.volume = value;

//         // Save the new values to JsonUtility
//         var settings = new AudioSettings()
//         {
//             sfxVolume = value,
//             bgmSlider = bgmSlider.value,
//             bgmVolume = value,
//             FootStepVol = value,
//             isMuted = isMuted
//         };
//         File.WriteAllText("AudioSettings.json", JsonUtility.ToJson(settings));
//     }
    
//     public void SFXVol(float value){
//         sfxSource.volume = value;
        
//         var settings = new AudioSettings()
//         {
//             sfxVolume = value,
//         };
//         File.WriteAllText("AudioSettings.json", JsonUtility.ToJson(settings));
//     }
    
//     public void AmbientVolume(float value){
//         Ambient.volume = value;
        
//         var settings = new AudioSettings()
//         {
//             AmbientVolume = value,
//             isMuted = isMuted
//         };
//         File.WriteAllText("AudioSettings.json", JsonUtility.ToJson(settings));
//     }

//     public void SetMuted(bool muted) {
//         // Set the audio manager's mute state
//         isMuted = muted;

//         // Mute or unmute the audio sources
//         sfxSource.mute = isMuted;
//         bgmSource.mute = isMuted;

//         // Save the new mute state to JsonUtility
//         var settings = new AudioSettings()
//         {
//             sfxVolume = sfxVolume,
//             bgmVolume = bgmVolume,
//             isMuted = muted
//         };
//         File.WriteAllText("AudioSettings.json", JsonUtility.ToJson(settings));
//     }
    
//         public void OnClick(){
//             sfxSource.Play();
//     }
// }



// public class AudioSettings{
//     public float sfxVolume;
//     public float bgmVolume;
//     public bool isMuted;
//     public float FootStepVol;
//     public float AmbientVolume;
    
//     public float sfxSlider;
//     public float bgmSlider;
//     public float AmbientSlider;
//     public float FootStepSlider;
// }
