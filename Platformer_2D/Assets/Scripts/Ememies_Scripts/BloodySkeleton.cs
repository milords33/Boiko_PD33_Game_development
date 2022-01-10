using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodySkeleton : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private GameObject _bloodySkeleton;
    [SerializeField] private Transform _player;
    [SerializeField] private float _viewingDistance;
    [SerializeField] private float _speed;
    [SerializeField] private float _pushPower;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHitPoints;
    [SerializeField] private Slider _hitPointsSlider;
    [SerializeField] private bool _faceRight;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _chasingAnimatorKey;
    [SerializeField] private string _emergenceAnimatorKey;
    [SerializeField] private string _disappearingAnimatorKey;
    [SerializeField] private string _deathAnimatorKey;

    private int _currentHitPoints;
    private float _startSpeed;
    private bool _emergence = false;
    private bool _death = false;

    void Start()
    {
        _startSpeed = _speed;
        _hitPointsSlider.maxValue = _maxHitPoints;
        ChangeHitPoints(_maxHitPoints);
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!_death)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

            if (distanceToPlayer < _viewingDistance)
            {
                _animator.SetBool(_emergenceAnimatorKey, true);
                if (_emergence)
                    StartChasing();
            }
            else
            {
                _animator.SetBool(_disappearingAnimatorKey, true);
                StopChasing();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_death)
        {
            PlayerMover player = other.collider.GetComponent<PlayerMover>();
            if (player != null)
            {
                player.TakeDamage(_damage, _pushPower, transform.position.x);
                TakeDamage(_maxHitPoints);
            }
        }
    }


    private void ChangeHitPoints(int hitPoints)
    {
        _currentHitPoints = hitPoints;
        if (_currentHitPoints <= 0)
        {
            _death = true;
            _animator.SetTrigger(_deathAnimatorKey);
        }
        _hitPointsSlider.value = hitPoints;
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
        _hitPointsSlider.transform.Rotate(0, 180, 0);
    }

    private void StartChasing()
    {
        _animator.SetBool(_chasingAnimatorKey, true);
        _animator.SetBool(_disappearingAnimatorKey, false);
        if (_player.position.x < transform.position.x && _emergence )
        {
            _rigidbody.velocity = new Vector2(-_speed, 0);
            if(_faceRight)
                Flip();
        }

        else if (_player.position.x > transform.position.x && _emergence)
        {
            _rigidbody.velocity = new Vector2(_speed, 0);
            if (!_faceRight)
            Flip();
        }
    }

    private void StopChasing()
    {
        _animator.SetBool(_emergenceAnimatorKey, false);
        _animator.SetBool(_chasingAnimatorKey, false);
        _rigidbody.velocity = new Vector2(0, 0);
        _emergence = false;
        _animator.SetTrigger(_disappearingAnimatorKey);
    }

    private void SetStartSpeed()
    {
        _speed = _startSpeed;
    }

    private void AnimationEventEmergence()
    {
        _emergence = true;
    }

    private void AnimationEventDeathInFight()
    {
        Destroy(_bloodySkeleton);
    }

    private void AnimationEventDeathWithoutFight()
    {
        if(!_animator.GetBool(_emergenceAnimatorKey) && !_animator.GetBool(_chasingAnimatorKey))
            Destroy(_bloodySkeleton);
    }


    public void TakeDamage(int damage)
    {
        ChangeHitPoints(_currentHitPoints - damage);
    }
}
