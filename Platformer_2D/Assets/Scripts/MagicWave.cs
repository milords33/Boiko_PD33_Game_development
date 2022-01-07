﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWave : MonoBehaviour
{
    [SerializeField] public SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _pushPower;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _delay;
    [SerializeField] private GameObject _shockEffect;

    private void Update()
    {
        Instantiate(_shockEffect, transform.position + new Vector3(0, 0.15f, 0), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
   {
        Bandits bandit = other.GetComponent<Bandits>();
        if (bandit != null)
            bandit.TakeDamage(_damage, _pushPower);

        EnemyArcher archer = other.GetComponent<EnemyArcher>();
        if (archer != null)
            archer.TakeDamage(_damage);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public void StartFly(bool faceRight)
    {
        if(!faceRight)
            transform.Rotate(0, 180, 0);

        _rigidbody.velocity = transform.right * _speed;
        Invoke(nameof(Destroy), _delay);
    }
}