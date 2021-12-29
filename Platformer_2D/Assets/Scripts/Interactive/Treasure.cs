using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] private int _coinsAmount;
    [SerializeField] Sprite _activeSprite;
    [SerializeField] AudioSource _treasureOpened;
    [SerializeField] AudioSource _moneyTaked;

    private SpriteRenderer _spriteRenderer;
    private bool Activated = false;

    PlayerMover _player;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();

        if (player != null && !Activated)
        {
            _spriteRenderer.sprite = _activeSprite;
            Activated = true;
            _player = player;
            _treasureOpened.Play();
            Invoke(nameof(MoneyTaked), 1f);
        }
    }

    private void MoneyTaked()
    {
        _moneyTaked.Play();
        _player.CoinsAmount += _coinsAmount;
    }
}
