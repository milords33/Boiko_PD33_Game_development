using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _mixer;
    [SerializeField] private Toggle _backgroundMusicToggle;
    [SerializeField] private Slider _musicSlider;

    private void Update()
    {
        if (!_backgroundMusicToggle.isOn)
            _mixer.audioMixer.SetFloat("MusicVolume", -80);

        if (_backgroundMusicToggle.isOn)
            _mixer.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 0, _musicSlider.value));
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ChangeMasterVolume(float volume)
    {
        _mixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80, 0, volume));
    }
}