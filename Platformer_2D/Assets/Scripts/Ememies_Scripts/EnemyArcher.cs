using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyArcher : MonoBehaviour
{
    [SerializeField] private GameObject _archer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private Arrow _arrowShoot;
    [SerializeField] private float _attackRange;
    [SerializeField] private int _maxHitPoints;
    [SerializeField] private bool _faceRight;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _shootAnimatonKey;
    [SerializeField] private string _hurtAnimatorKey;
    [SerializeField] private string _deathAnimatorKey;

    [Header("Audio")]
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _hitSound;

    [Header("UI")]
    [SerializeField] private Slider _hitPointsBar;

    private bool _hurt = false;
    private bool _canShoot;
    private bool _death;

    private int _currentHitPoints;
    private int CurrentHitPoints
    {
        get => _currentHitPoints;
        set
        {
            _currentHitPoints = value;
             _hitPointsBar.value = _currentHitPoints;
        }
    }
    private void Start()
    {
        _hitPointsBar.maxValue = _maxHitPoints;
        ChangeHitPoints(_maxHitPoints);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_currentHitPoints > 0)
        {
            PlayerMover player = other.collider.GetComponent<PlayerMover>();
            if (player != null)
            {
                if (player.CanAttackEnemy)
                    TakeDamage(player.AttackDamage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_attackRange * 2, 1, 0));
    }

    private void ChangeHitPoints(int hitPoints)
    {
        _currentHitPoints = hitPoints;
        _hitPointsBar.value = hitPoints;
    }

    private void FixedUpdate()
    {
        if (_canShoot)
        {
            return;
        }
        CheckIfCanShoot();
    }

    private void CheckIfCanShoot()
    {
        Collider2D player = Physics2D.OverlapBox(transform.position, new Vector2(_attackRange, 1), 0, _whatIsPlayer);
        if (player != null && !_hurt)
        {
            StartShoot(player.transform.position);
            _canShoot = true;
        }
        else
        {
            _canShoot = false;
        }
    }

    private void StartShoot(Vector2 position)
    {
        float posX = transform.position.x;
        if (posX < position.x && !_faceRight || posX > position.x && _faceRight)
        {
            transform.Rotate(0, 180, 0);
            _hitPointsBar.transform.Rotate(0, 180, 0);
            _faceRight = !_faceRight;
        }
        _animator.SetBool(_shootAnimatonKey, true);
    }

    public void TakeDamage(int damage)
    {
        _hurt = true;
        CurrentHitPoints -= damage;
        _hitSound.Play();
        _animator.SetTrigger(_hurtAnimatorKey);

        if (CurrentHitPoints <= 0)
        {
            _animator.SetTrigger(_deathAnimatorKey);
            _rigidbody.simulated = false;
            _collider.enabled = false;
        }
    }

    #region Funcions for animations
    private void AnimationEvent_ShootSound()
    {
        _shootSound.pitch = Random.Range(0.8f, 1.1f);
        _shootSound.Play();
    }


    private void AnimationEventShoot()
    {
        Arrow arrow = Instantiate(_arrowShoot, _shootPoint.position, Quaternion.identity);
        if (_faceRight)
            arrow.SpriteRenderer.flipX = true;
        else
            arrow.SpriteRenderer.flipX = false;
        arrow.StartFly(transform.right);

        _animator.SetBool(_shootAnimatonKey, false);
        Invoke(nameof(CheckIfCanShoot), 1f);
    }

    private void AnimationEventHurt()
    {
        _hurt = false;
    }

    private void AnimationEventDeath()
    {
        Destroy(_archer);
    }

    #endregion

}
