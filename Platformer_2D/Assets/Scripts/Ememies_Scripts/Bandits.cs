 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bandits : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _bandit;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private PlayerMover _player;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private float _walkRange;
    [SerializeField] private float _speed;
    [SerializeField] private int _maxHitPoints;
    [SerializeField] private int _coinsAmount;
    [SerializeField] private bool _faceRight;

    [Header("Attack")]
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _pushPower;
    [SerializeField] private float _attackRadius;
    [SerializeField] private int _damage;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _attackAnimatorKey;
    [SerializeField] private string _hurtAnimatorKey;
    [SerializeField] private string _deathAnimatorKey;

    [Header("Audio")]
    [SerializeField] private AudioSource _hitSound;

    [Header("UI")]
    [SerializeField] private Slider _hitPointsBar;

    private Vector2 _startPostion;
    private bool _hurt = false;
    private int _currentHitPoints;

    private float _startSpeed;

    private void Start()
    {
        _startSpeed = _speed;
        _hitPointsBar.maxValue = _maxHitPoints;
        ChangeHitPoints(_maxHitPoints);
        _startPostion = transform.position;
    }


    private void FixedUpdate()
    {
        if(_currentHitPoints > 0 && !_hurt)
            _rigidbody.velocity = transform.right * _speed;
    }

    private void Update()
    {
        if (_currentHitPoints > 0 && !_hurt)
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            _speed = 0f;
            _animator.SetTrigger(_attackAnimatorKey);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_drawPostion, new Vector3(_walkRange * 2, 1, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackPoint.position, new Vector3(_attackRadius, _attackRadius, 0));
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
        _hitPointsBar.transform.Rotate(0, 180, 0);
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

   public void TakeDamage(int damage, float pushPower = 0, float heroPosX = 0)
    {
        _hurt = true;
        _hitSound.Play();
        _animator.SetTrigger(_hurtAnimatorKey);
        Instantiate(_hitEffect, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);

        ChangeHitPoints(_currentHitPoints - damage);
        if (_currentHitPoints <= 0)
        {
            _player.CoinsAmount += _coinsAmount;
            _animator.SetBool(_deathAnimatorKey, true);
            _rigidbody.simulated = false;
        }
        else
        {
            if (pushPower != 0)
            {
                int direction = transform.position.x > heroPosX ? 1 : -1;
                _rigidbody.AddForce(new Vector2(direction * pushPower, 0));
            }
        }
    }

    private void ChangeHitPoints(int hitPoints)
    {
        _currentHitPoints = hitPoints;
        _hitPointsBar.value = hitPoints;
    }

    private void AnimationEventDeath()
    {
        Destroy(_bandit);
    }

    private void AnimationEventHurt()
    {
        _hurt = false;
    }

    private void AnimationEventAttack()
    { 
        Collider2D[] targets = Physics2D.OverlapBoxAll(_attackPoint.position,
new Vector2(_attackRadius, _attackRadius), _whatIsPlayer);

        foreach (var target in targets)
        {
            PlayerMover player = target.GetComponent<PlayerMover>();
            if (player != null)
                player.TakeDamage(_damage, _pushPower, transform.position.x);
        }
        Invoke(nameof(InvokeMovement), 0.8f);
    }
    
    private void InvokeMovement()
    {
        _speed = _startSpeed;
    }
}
