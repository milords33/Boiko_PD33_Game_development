using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanditBoss : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _banditBoss;
    [SerializeField] private GameObject _leftDashAttackElement;
    [SerializeField] private GameObject _rightDashAttackElement;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _player;
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
   // [SerializeField] private string _deathAnimatorKey;

    private int _currentHitPoints;
    private float _startSpeed;
    private bool _attack = false;
    private bool _death = false;
    private bool _dashAttackBool = true;

    void Start()
    {
        _startSpeed = _speed;
        _hitPointsSlider.maxValue = _maxHitPoints;
        ChangeHitPoints(_maxHitPoints);
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_dashAttackBool)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

            if (distanceToPlayer < _viewingDistance)
            {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMover player = collision.GetComponent<PlayerMover>();
        if (player != null)
        {
            _attack = true;
            _speed = 0f;
            _animator.SetTrigger(_attackAnimatorKey);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
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
           // _animator.SetTrigger(_deathAnimatorKey);
        }
        _hitPointsSlider.value = hitPoints;
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
        _hitPointsSlider.transform.Rotate(0, 180, 0);
    }

    private void MadeDashAttack()
    {
        _animator.SetBool(_dashAttackAnimatorKey, true);

        if (_player.position.x < transform.position.x)
        {
            _rigidbody.velocity = new Vector2(-_speed, 0);
            if (_faceRight)
            {
                Flip();
                _leftDashAttackElement.SetActive(true);
            }
        }

        else if (_player.position.x > transform.position.x)
        {
            _rigidbody.velocity = new Vector2(_speed, 0);
            if (!_faceRight)
            {
                Flip();
                _rightDashAttackElement.SetActive(true);
            }
        }
    }
    private void StartChasing()
    {
        _animator.SetBool(_chasingAnimatorKey, true);
        if (_player.position.x < transform.position.x )
        {
            _rigidbody.velocity = new Vector2(-_speed/4, 0);
            if (_faceRight)
                Flip();
        }

        else if (_player.position.x > transform.position.x)
        {
            _rigidbody.velocity = new Vector2(_speed/4, 0);
            if (!_faceRight)
                Flip();
        }
    }


    private void AnimationEventEndDashAttack()
    {
        _dashAttackBool = false;
        _leftDashAttackElement.SetActive(false);
        _rightDashAttackElement.SetActive(false);
        _animator.SetBool(_dashAttackAnimatorKey, false);
        Invoke(nameof(PrepareForDashAttack), 5f);
    }

    private void PrepareForDashAttack()
    {
        _dashAttackBool = true;
    }

    private void SetStartSpeed()
    {
        _speed = _startSpeed;
    }

    public void TakeDamage(int damage)
    {
        ChangeHitPoints(_currentHitPoints - damage);
    }
}
