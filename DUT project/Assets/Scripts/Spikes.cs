using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _damageDelay;

    private float _lastDamageTime;

    private CharacterMovement _character;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMovement character = other.GetComponent<CharacterMovement>();
        if (character != null)
            _character = character;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CharacterMovement character = other.GetComponent<CharacterMovement>();
        if (character == _character)
            _character = null;
    }

    private void Update()
    {
        if (_character != null && Time.time - _lastDamageTime > _damageDelay)
        {
            _lastDamageTime = Time.time;
            _character.TakeDamage(_damage);
        }
    }
}