using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelDoor : MonoBehaviour
{
    [SerializeField] private int _scoreToNextLevel;
    [SerializeField] private int _levelToLoad;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _doorOpenSprite;
    [SerializeField] private AudioSource _openDoorSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null && player.CoinsAmount >= _scoreToNextLevel)
        {
            _spriteRenderer.sprite = _doorOpenSprite;
            _openDoorSound.Play();
            PlayerPrefs.SetInt("loadingLevel", _levelToLoad);
            Invoke(nameof(LoadNextScene), 1f);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
