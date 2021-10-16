using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskMan : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _maskMan;
    [SerializeField] private float _walkRange;
    [SerializeField] private float _speed;
    [SerializeField] private float _pushPower;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHitPoints;
    [SerializeField] private bool _faceRight;

    private Vector2 _startPostion;
    private int _currentHitPoints;
    private int CurrentHitPoints
    {
        get => _currentHitPoints;
        set
        {
            _currentHitPoints = value;
        }
    }

    private void Start()
    {
        _currentHitPoints = _maxHitPoints;
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
        PlayerMover player = other.collider.GetComponent<PlayerMover>();
        if (player != null)
        {
            if (player.CanAttackEnemy)
                TakeDamage(player.AttackDamage);
            else
            {
                //_animator.SetTrigger(_attackAnimatorKey);
                player.TakeDamage(_damage, _pushPower, transform.position.x);
            }
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

    public void TakeDamage(int damage)
    {
        CurrentHitPoints -= damage;
        //_hitSound.Play();
        if (CurrentHitPoints <= 0)
            Destroy(_maskMan);
    }
}