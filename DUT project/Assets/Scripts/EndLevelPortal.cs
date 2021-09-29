using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelPortal : MonoBehaviour
{
    [SerializeField] private int _coinsToNextLevel;
    [SerializeField] private int _levelToLoad;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _portalEmergence;
    [SerializeField] private Sprite _openPortalsSprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMovement character = other.GetComponent<CharacterMovement>();
        if (character != null && character.CoinsAmount >= _coinsToNextLevel)
        {
            _spriteRenderer.sprite = _openPortalsSprite;
            Invoke(nameof(LoadNextScene),0.3f);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_levelToLoad);
    }

    public bool CompareCoins(int coins)
    {
        if (coins >= _coinsToNextLevel)
        {
            _spriteRenderer.sprite = _portalEmergence;
            return true;
        }
        else
            return false;
    }
    
}
