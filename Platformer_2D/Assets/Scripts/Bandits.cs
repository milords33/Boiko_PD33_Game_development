using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandits : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _bandit;
    [SerializeField] private float _walkRange;
    [SerializeField] private float _speed;
    [SerializeField] private float _pushPower;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHp;
    [SerializeField] private bool _faceRight;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _attackAnimatorKey;
    [SerializeField] private string _hurtAnimatorKey;
    [SerializeField] private string _deathAnimatorKey;

    [Header("Audio")]
    [SerializeField] private AudioSource _hitSound;

    private Vector2 _startPostion;
    private bool _hurt = false;
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
        _currentHitPoints = _maxHp;
        _startPostion = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_drawPostion, new Vector3(_walkRange * 2, 1, 0));
    }

    private void FixedUpdate()
    {
        if(_currentHitPoints > 0 && !_hurt)
            _rigidbody.velocity = transform.right * _speed;
    }

    private void Update()
    {
        if (_currentHitPoints > 0)
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

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_currentHitPoints > 0)
        {
            PlayerMover player = other.collider.GetComponent<PlayerMover>();
            if (player != null)
            {
                if (player.CanAttackEnemy)
                    TakeDamage(player.AttackDamage, player.AttackPushPower, player.transform.position.x);
                else
                {
                    _animator.SetTrigger(_attackAnimatorKey);
                    player.TakeDamage(_damage, _pushPower, transform.position.x);
                }
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

   public void TakeDamage(int damage, float pushPower = 0, float heroPosX = 0)
    {
        _hurt = true;
        CurrentHitPoints -= damage;
        _hitSound.Play();
        _animator.SetTrigger(_hurtAnimatorKey);


        if (CurrentHitPoints <= 0)
        {
            _animator.SetTrigger(_deathAnimatorKey);
        }
        else
        {
            if (pushPower != 0)
            {
                int direction = transform.position.x > heroPosX ? 1 : 1;
                _rigidbody.AddForce(new Vector2(direction * pushPower, 0));
            }
        }
    }

    private void AnimationDeath()
    {
        Destroy(_bandit);
    }

    private void AnimationHurt()
    {
        _hurt = false;
    }
}
