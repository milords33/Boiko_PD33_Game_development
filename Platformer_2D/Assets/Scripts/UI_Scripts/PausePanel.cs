using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _mixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _effectsSlider;
    [SerializeField] private Button _saveAudioPreferences;


    private void Update()
    {
        _mixer.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-35, 0, _musicSlider.value));

        if (_musicSlider.value == 0)
            _mixer.audioMixer.SetFloat("MusicVolume", -80);

        _mixer.audioMixer.SetFloat("EffectsVolume", Mathf.Lerp(-35, 0, _effectsSlider.value));


        if (_effectsSlider.value == 0)
            _mixer.audioMixer.SetFloat("EffectsVolume", -80);


        _mixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-35, 0, _masterSlider.value));

        if (_masterSlider.value == 0)
            _mixer.audioMixer.SetFloat("MasterVolume", -80);

        _saveAudioPreferences.onClick.AddListener(SaveAudioPreferences);
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void OnButtonClickHandler()
    {
        
    }

    public void GetAudioVolume()
    {

        if (PlayerPrefs.HasKey("MasterSlider"))
            _masterSlider.value = PlayerPrefs.GetFloat("MasterSlider");

        if (PlayerPrefs.HasKey("EffectsSlider"))
            _effectsSlider.value = PlayerPrefs.GetFloat("EffectsSlider");

        if (PlayerPrefs.HasKey("MusicSlider"))
            _musicSlider.value = PlayerPrefs.GetFloat("MusicSlider");

    }

    private void SaveAudioPreferences()
    {
        PlayerPrefs.SetFloat("MasterSlider", System.Convert.ToSingle(_masterSlider.value));

        PlayerPrefs.SetFloat("EffectsSlider", System.Convert.ToSingle(_effectsSlider.value));

        PlayerPrefs.SetFloat("MusicSlider", System.Convert.ToSingle(_musicSlider.value));
/*        Debug.Log(PlayerPrefs.GetFloat("MusicSlider"));*/
    }

}
