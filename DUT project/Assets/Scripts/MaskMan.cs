using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskMan : MonoBehaviour
{
    [SerializeField] private float _walkRange;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private bool _faceRight;
    [SerializeField] private int _damage;
    [SerializeField] private float _pushPower;
    private Vector2 _startPostion;

    private void Start()
    {
        _startPostion = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_drawPostion, new Vector3(_walkRange * 2, 1, 0));
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.right * _speed;
    }

    private void Update()
    {
        float xPos = transform.position.x;
        if (xPos > _startPostion.x + _walkRange && _faceRight)
        {
            Flip();
        }
        else if (xPos < _startPostion.x - _walkRange && !_faceRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CharacterMovement character = other.collider.GetComponent<CharacterMovement>();
        if (character != null)
        {
            character.TakeDamage(_damage, _pushPower, transform.position.x);
        }
    }
    private Vector2 _drawPostion
    {
        get
        {
            if (_startPostion == Vector2.zero)
                return transform.position;
            else
                return _startPostion;
        }
    }
}