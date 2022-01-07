using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] private int _coinsAmount;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private AudioSource _treasureOpened;
    [SerializeField] private AudioSource _moneyTaked;
    [SerializeField] private GameObject _coinsEffect;

    private SpriteRenderer _spriteRenderer;
    private bool Activated = false;

    private PlayerMover _player;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();

        if (player != null && !Activated)
        {
            Instantiate(_coinsEffect, transform.position + new Vector3(0, 0.15f, 0), Quaternion.identity);
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
