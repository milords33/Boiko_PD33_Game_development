using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Lever : MonoBehaviour
{
    [SerializeField] Sprite _activeSprite;
    [SerializeField] Treasure _treasure;

    private SpriteRenderer _spriteRenderer;

    private Sprite _inactiveSprite;

    private bool _activated;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inactiveSprite = _spriteRenderer.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMovement character = other.GetComponent<CharacterMovement>();
        if (character != null && !_activated)
        {
            _treasure.CanBeActived = true;
            _spriteRenderer.sprite = _activeSprite;
            _activated = true;
            Debug.Log("The treasure can be opened");
        }
    }
}
