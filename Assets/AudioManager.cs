using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    public AudioSource BGM;
    public AudioSource SFX;
    public AudioSource Ambient;
    public AudioSource FootStep;
    // public AudioSource onClick;
    public AudioSource CoinCollect;
    
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider FootStepSlider;
    [SerializeField] private Slider AmbientSlider;
    
    [SerializeField] private float BGMVolume;
    [SerializeField] private float SFXVolume;
    [SerializeField] private float FootStepVolume;
    [SerializeField] private float AmbientVolume;
    [SerializeField] private float CoinCollectVol;
    
    private int isMuted;


    void Start() {
        BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        FootStepVolume = PlayerPrefs.GetFloat("FootVolume", 1f);
        AmbientVolume = PlayerPrefs.GetFloat("AmbientVolume", 1f);
        
        BGMSlider.value = BGMVolume;
        SFXSlider.value = SFXVolume;
        FootStepSlider.value = FootStepVolume;
        AmbientSlider.value = AmbientVolume;
        // CoinCollectVol = SFXVolume;
    }
    
        void Awake(){
        if(SceneManager.GetActiveScene().name == "MainMenu"){
            FootStep.GetComponent<AudioSource>().Stop();
            CoinCollect.GetComponent<AudioSource>().Stop();
        }
    }
    
    void Update() {
        BGM.volume = BGMSlider.value;
        SFX.volume = SFXSlider.value;
        FootStep.volume = FootStepSlider.value;
        Ambient.volume = AmbientSlider.value;
    }
    
    public void onClick(){
        SFX.Play();
    }

    public void SaveBGMVolume() {
        // Save the music volume to PlayerPrefs.
        PlayerPrefs.SetFloat("BGMVolume", BGMSlider.value);
    }
    public void SaveSFXVolume() {
        // Save the sfx volume to PlayerPrefs.
        PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
    }
    public void SaveFootVolume() {
        // Save the footstep volume to PlayerPrefs.
        PlayerPrefs.SetFloat("FootVolume", FootStepSlider.value);
    }
    public void SaveAmbientVolume() {
        // Save the ambient volume to PlayerPrefs.
        PlayerPrefs.SetFloat("AmbientVolume", AmbientSlider.value);
    }
}
