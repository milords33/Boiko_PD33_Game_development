﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] public SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _pushPower;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;


    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
            player.TakeDamage(_damage, _pushPower);
        DestroyObject();
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
    public void StartFly(Vector2 direction)
    {
        _rigidbody.velocity = direction * _speed;
        Invoke(nameof(DestroyObject), 5f);
    }
}
