using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelDoor : MonoBehaviour
{
    [SerializeField] private int _levelToLoad;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _doorOpenSprite;
    [SerializeField] private AudioSource _openDoorSound;
    [SerializeField] private GameObject _interactionButton;

    private PlayerMover _player;

    private void OnTriggerEnter2D(Collider2D other)
    {
       _player = other.GetComponent<PlayerMover>();
        if(_player != null )
            _interactionButton.gameObject.SetActive(true);

        LoadFromMainMenu loadFromMainMenu = other.GetComponent<LoadFromMainMenu>();
        if (loadFromMainMenu != null)
        {
            _spriteRenderer.sprite = _doorOpenSprite;
            _openDoorSound.Play();
        }
    }

    private void OnTriggerExit2D()
    {
        _interactionButton.gameObject.SetActive(false);
        _player = null;
    }

    private void OnTriggerStay2D()
    {
        if (Input.GetKey(KeyCode.E) && _player!=null)
        {
            _spriteRenderer.sprite = _doorOpenSprite;
            _openDoorSound.Play();
            SaveProgress(_player);
            Invoke(nameof(LoadNextScene), 1f);
        }
    }
    
    private void SaveProgress(PlayerMover player)
    {
        PlayerPrefs.SetInt("loadingLevel", _levelToLoad);
        PlayerPrefs.SetInt("CoinsAmount", player.CoinsAmount);
        PlayerPrefs.SetInt("HitPoints", player.CurrentHitPoints);
        PlayerPrefs.SetInt("ManaPoints", player.CurrentManaPoints);

        PlayerPrefs.SetInt("MaxHitPoints", player.MaxHitPoints);
        PlayerPrefs.SetInt("MaxManaPoints", player.MaxManaPoints);
        PlayerPrefs.SetInt("MaxShieldPoints", player.MaxShieldPoints);
        PlayerPrefs.SetInt("AttackDamage", player.AttackDamage);

    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
