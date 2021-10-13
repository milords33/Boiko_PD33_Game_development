using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Treasure : MonoBehaviour
{
    [SerializeField] private int _coinsAmount;

    [SerializeField] Sprite _activeSprite;

    private SpriteRenderer _spriteRenderer;

    private Sprite _inactiveSprite;

    public bool Activated { get; set; } = false;

    public bool CanBeActived { get; set; }




    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inactiveSprite = _spriteRenderer.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();

        if(player != null && !Activated && CanBeActived)
        {
            _spriteRenderer.sprite = _activeSprite;
            Activated = true;
            player.CoinsAmount += _coinsAmount;
            Debug.Log("Treasure was activated!");
            Debug.Log($"You found {_coinsAmount}");
        }
    }
}
