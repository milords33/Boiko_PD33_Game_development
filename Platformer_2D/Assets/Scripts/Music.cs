using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] public AudioSource _backgroundMusic;
    [SerializeField] public AudioSource _bossFightMusic;

    private void Awake()
    {
        _backgroundMusic.Play();
    }

    public void ChangeMusic()
    {
        if (_backgroundMusic.isPlaying && _bossFightMusic != null)
        {
            _backgroundMusic.Stop();
            _bossFightMusic.Play();
        }
        else if (_bossFightMusic.isPlaying && _backgroundMusic != null)
        {
            _bossFightMusic.Stop();
            _backgroundMusic.Play();
        }
        else
            throw new System.Exception("lost music data");
    }

}
