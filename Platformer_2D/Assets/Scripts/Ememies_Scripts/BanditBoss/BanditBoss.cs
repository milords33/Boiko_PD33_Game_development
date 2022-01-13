using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanditBoss : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _banditBoss;
    [SerializeField] private GameObject _dashAttackElement;
    [SerializeField] private GameObject[] _armyOfEnemies;
    [SerializeField] private GameObject[] _newArmyOfEnemies;
    [SerializeField] private GameObject _nextLevel;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private float _viewingDistance;
    [SerializeField] private float _speed;
    [SerializeField] private float _pushPower;
    [SerializeField] private float _attackRadius;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHitPoints;
    [SerializeField] private Slider _hitPointsSlider;
    [SerializeField] private bool _faceRight;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _dashAttackAnimatorKey;
    [SerializeField] private string _chasingAnimatorKey;
    [SerializeField] private string _attackAnimatorKey;
    [SerializeField] private string _deathAnimatorKey;

    [Header("Effects")]
    [SerializeField] private GameObject _dashAttackEffect;
    [SerializeField] private GameObject _hitEffect;


    private int _currentHitPoints;
    private float _startSpeed;
    private float _dashSpeed;
    private bool _attack;
    private bool _death = false;
    private bool _dashAttackBool;
    private bool _canFlip;

    private bool _stage1;
    private bool _stage2;

    void Start()
    {
        _stage1 = true;
        _stage2 = true;
        _canFlip = true;
        _dashAttackBool = true;
        _attack = false;
        _startSpeed = _speed;
        _dashSpeed = _speed * 3f;
        _hitPointsSlider.maxValue = _maxHitPoints;
        ChangeHitPoints(_maxHitPoints);

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(_stage1 && _currentHitPoints <= _maxHitPoints/2)
        {
            _armyOfEnemies[0].SetActive(true);
            _stage1 = false;
        }

        if (_stage2 && _currentHitPoints < _maxHitPoints / 4)
        {
            _armyOfEnemies[1].SetActive(true);
            _stage2 = false;
        }

        if (_dashAttackBool && !_attack)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
            if (distanceToPlayer < _viewingDistance)
            {   
                _hitPointsSlider.gameObject.SetActive(true);
                MadeDashAttack();
            }
        }
        else if(!_attack)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

            if (distanceToPlayer < _viewingDistance)
            {
                StartChasing();
            }
        }
    }

    private void MadeDashAttack()
    {
        if (_faceRight)
            Instantiate(_dashAttackEffect, transform.position + new Vector3(1.3f, -0.05f, 0), Quaternion.identity);
        else
            Instantiate(_dashAttackEffect, transform.position + new Vector3(-1.3f, -0.05f, 0), Quaternion.identity);

        _animator.SetBool(_dashAttackAnimatorKey, true);
        _dashAttackElement.SetActive(true);
        if (_canFlip)
        {
            if (_player.position.x < transform.position.x)
            {
                _rigidbody.velocity = new Vector2(-_dashSpeed, 0);
                if (_faceRight)
                    Flip();
            }

            else if (_player.position.x > transform.position.x)
            {
                _rigidbody.velocity = new Vector2(_dashSpeed, 0);
                if (!_faceRight)
                    Flip();
            }
        }
    }
    private void AnimationEventEndDashAttack()
    {
        _dashAttackBool = false;
        _dashAttackElement.SetActive(false);
        _animator.SetBool(_dashAttackAnimatorKey, false);
        _animator.SetBool(_chasingAnimatorKey, true);
        Invoke(nameof(PrepareForDashAttack), 5f);
    }

    private void StartChasing()
    {
        if (_canFlip)
        {
            if (_player.position.x < transform.position.x)
            {
                _rigidbody.velocity = new Vector2(-_speed, 0);
                if (_faceRight)
                    Flip();
            }

            else if (_player.position.x > transform.position.x)
            {
                _rigidbody.velocity = new Vector2(_speed, 0);
                if (!_faceRight)
                    Flip();
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null && !_dashAttackBool && !_attack)
        {
            _animator.SetTrigger(_attackAnimatorKey);
            _speed = 0f;
            _attack = true;
        }
    }

    private void AnimationEventSimpleAttack()
    {
        Collider2D[] targets = Physics2D.OverlapBoxAll(_attackPoint.position,
            new Vector2(_attackRadius, _attackRadius), _whatIsPlayer);

        foreach(var target in targets)
        {
            PlayerMover player = target.GetComponent<PlayerMover>();
            if (player != null)
                player.TakeDamage(_damage, _pushPower, transform.position.x);
        }
        _attack = false;
        _speed = _startSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_attackPoint.position, new Vector3(_attackRadius, _attackRadius, 0));
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
        _canFlip = false;
        Invoke(nameof(CreateDelayForFlip), 0.2f);
    }

    private void CreateDelayForFlip()
    {
        _canFlip = true;
    }

    private void PrepareForDashAttack()
    {
        _dashAttackBool = true;
    }

    public void TakeDamage(int damage)
    {
        Instantiate(_hitEffect, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        ChangeHitPoints(_currentHitPoints - damage);
    }

    private void AnimationEventDeath()
    {
        _nextLevel.SetActive(true);
        Destroy(_banditBoss.gameObject);
    }
}
