using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private float _speed;
    [SerializeField] private float _rollForce;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsCell;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private float _jumpForce;

    [SerializeField] private Collider2D _topBodyCollider;
    [SerializeField] private Transform _topBodyChecker;
    [SerializeField] private float _topBodyCheckerRadius;

    [SerializeField] private int _maxHitPoints;
    [SerializeField] private int _maxShieldPoints;

    [SerializeField] private Collider2D _AttackRange;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _attackPushPower;


    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpBoolAnimatorKey;
    [SerializeField] private string _jumpTriggerAnimatorKey;
    [SerializeField] private string _rollBoolAnimatorKey;
    [SerializeField] private string _rollTriggerAnimatorKey;
    [SerializeField] private string _BlockAnimatorKey;
    [SerializeField] private string _BlockIdleAnimatorKey;
    [SerializeField] private string _hurtAnimatorKey;
    [SerializeField] private string _deathAnimatorKey;

    [Header("UI")]
    [SerializeField] private TMP_Text _coinsAmountText;
    [SerializeField] private Slider _hitPointsBar;
    [SerializeField] private Slider _shieldPointsBar;
    [SerializeField] private GameObject _pausePanelGameObject;
    [SerializeField] private PausePanel _pausePanelClass;

    [Header("Audio")]
    [SerializeField] private AudioSource _runSound;
    [SerializeField] private AudioSource _jumpSound;
    [SerializeField] private AudioSource _rollSound;
    [SerializeField] private AudioSource _hitSound;
    [SerializeField] private AudioSource _hurtSound;
    [SerializeField] private AudioSource _deathSound;
    [SerializeField] private AudioSource _potionSound;
    [SerializeField] private AudioSource _swingSound;
    [SerializeField] private AudioSource _shieldActivated;
    [SerializeField] private AudioSource _shieldProtected;

    private float _horizontalDirection;
    private bool _roll = false;
    private bool _jump;

    private int _currentAttack = 0;
    private float _timeSinceAttack = 0.0f;
    private float _lastPushTime;

    private int _currentShieldPoints;
    private int _shieldProtectPoints = 25;
    private int _currentHitPoints;
    private bool _shieldActive;
    private bool _hurt;
    private bool _death;

    private int _coinsAmount;
    private bool _checkActiveMenuPanel = false;

    public bool CanAttackEnemy { get; set; }

    public int AttackDamage { get; set; }

    public float AttackPushPower { get; set; }

    public int CoinsAmount
    {
        get => _coinsAmount;
        set
        {
            _coinsAmount = value;
            _coinsAmountText.text = value.ToString();
        }
    }

    public int CurrentHitPoints
    {
        get => _currentHitPoints;
        set
        {
            _currentHitPoints = value;
            _hitPointsBar.value = _currentHitPoints;
        }
    }

    public int CurrentShieldPoints
    {
        get => _currentShieldPoints;
        set
        {
            _currentShieldPoints = value;
            _shieldPointsBar.value = _currentShieldPoints;
        }
    }


    private void Start()
    {
        _pausePanelClass.GetAudioVolume();
        _pausePanelGameObject.SetActive(_checkActiveMenuPanel);
        _hitPointsBar.maxValue = _maxHitPoints;
        CurrentHitPoints = _maxHitPoints;
        AttackDamage = _attackDamage;
        AttackPushPower = _attackPushPower;
        _AttackRange.enabled = false;
        CanAttackEnemy = false;
        _death = false;

        _shieldPointsBar.maxValue = _maxShieldPoints;
        CurrentShieldPoints = _maxShieldPoints;
        CoinsAmount = 0;

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
            ReloadScene();

        if (!_death)
        {
            #region Movement
            _horizontalDirection = Input.GetAxisRaw("Horizontal");
            _animator.SetFloat(_runAnimatorKey, Mathf.Abs(_horizontalDirection));

            if (_horizontalDirection > 0 && _spriteRenderer.flipX)
                _spriteRenderer.flipX = false;
            else if (_horizontalDirection < 0 && !_spriteRenderer.flipX)
                _spriteRenderer.flipX = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jump = true;
                _jumpSound.Play();
                ResetAttack();
            }

            if (Input.GetKeyDown(KeyCode.C) && !_roll)
            {
                _roll = true;
                _rollSound.Play();
                _animator.SetTrigger(_rollTriggerAnimatorKey);
                if (_spriteRenderer.flipX)
                    _rigidbody.velocity = new Vector2(-1 * _rollForce, _rigidbody.velocity.y);
                else
                    _rigidbody.velocity = new Vector2(1 * _rollForce, _rigidbody.velocity.y);
                ResetAttack();
            }
            #endregion

            #region Attacks
            _timeSinceAttack += Time.deltaTime;
            if (Input.GetKey(KeyCode.Mouse0) && _timeSinceAttack > 0.25f && !_roll)
            {
                _swingSound.Play();
                _AttackRange.enabled = true;
                CanAttackEnemy = true;
                _currentAttack++;

                if (_currentAttack > 3)
                    _currentAttack = 1;
                if (_timeSinceAttack > 1.0f)
                    _currentAttack = 1;

                _animator.SetTrigger("Attack " + _currentAttack);
                _timeSinceAttack = 0.0f;
            }
            #endregion

            #region Shield
            if (Input.GetKeyDown(KeyCode.Mouse1) && !_roll)
            {
                _animator.SetTrigger(_BlockAnimatorKey);
                _animator.SetBool(_BlockIdleAnimatorKey, true);
                _shieldActive = true;
                _shieldActivated.Play();
                ResetAttack();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                _animator.SetBool(_BlockIdleAnimatorKey, false);
                _shieldActive = false;
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _pausePanelGameObject.SetActive(!_checkActiveMenuPanel);
                _checkActiveMenuPanel = !_checkActiveMenuPanel;
            }
        }

    }

    private void FixedUpdate()
    {
        bool canJump = Physics2D.OverlapCircle
            (_groundChecker.position, _groundCheckerRadius, _whatIsGround);

        if (!_roll)
            _rigidbody.velocity = new 
                Vector2(_horizontalDirection * _speed, _rigidbody.velocity.y);


         bool canStand = !Physics2D.OverlapCircle(_topBodyChecker.position, _topBodyCheckerRadius, _whatIsCell);
         Collider2D coll = Physics2D.OverlapCircle(_topBodyChecker.position, _topBodyCheckerRadius, _whatIsCell);
        _topBodyCollider.enabled = !_roll && canStand;

        if (_jump && canJump && !_roll)
        {
            _animator.SetTrigger(_jumpTriggerAnimatorKey);
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }

        if (_animator.GetBool(_hurtAnimatorKey))
        {
            if (Time.time - _lastPushTime > 0.2f && canJump)
            {
                _hurt = false;
                _animator.SetBool(_hurtAnimatorKey, _hurt);
            }
            return;
        }

        _animator.SetBool(_jumpBoolAnimatorKey, !canJump);
        _animator.SetBool(_rollBoolAnimatorKey, _roll);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_topBodyChecker.position, _topBodyCheckerRadius);
    }

    private IEnumerator RestoreHitPoints(int pointsToAdd)
    {
        while (pointsToAdd != 0)
        {
            pointsToAdd--;
            CurrentHitPoints++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void RestoreShieldPoints()
    {
        CurrentShieldPoints += _shieldProtectPoints;
    }

    public void AddHitPoints(int hitPoints)
    {
        _potionSound.Play();
        int missingHP = _maxHitPoints - CurrentHitPoints;
        int pointToAdd = missingHP > hitPoints ? hitPoints : missingHP;
        StartCoroutine(RestoreHitPoints(pointToAdd));
    }

    // for moving enemies
    public void TakeDamage(int damage, float pushPower = 0, float enemyPosX = 0)
    {
        if (_animator.GetBool(_hurtAnimatorKey))
            return;

        if(_shieldActive && CurrentShieldPoints > 0)
        {
            CurrentShieldPoints -= _shieldProtectPoints;
            _shieldProtected.Play();
            Invoke(nameof(RestoreShieldPoints), 15f);
        }
        else
        {
            CurrentHitPoints -= damage;
            _hurtSound.Play();
            _hurt = true;
            _hitSound.Play();
        }

        if (CurrentHitPoints <= 0)
        {
            _speed = 0;
            _death = true;
            _deathSound.Play();
            _animator.SetBool(_deathAnimatorKey, _death);
            Invoke(nameof(ReloadScene), 2f);
        }

        if(pushPower != 0)
        {
            _lastPushTime = Time.time;
            int direction = transform.position.x > enemyPosX ? 1 : 1;
            _rigidbody.AddForce(new Vector2(direction * pushPower/2, pushPower));
            _animator.SetBool(_hurtAnimatorKey, _hurt);
        }
        ResetAttack();
    }
    // for spikes
    public void TakeDamage(int damage)
    {
        CurrentHitPoints -= damage;
        _hurtSound.Play();
        _hurt = true;
        ResetAttack();

        if (CurrentHitPoints <= 0)
        {
            _speed = 0;
            _death = true;
            _deathSound.Play();
            _animator.SetBool(_deathAnimatorKey, _death);
            Invoke(nameof(ReloadScene), 2f);
        }
    }

    private void ReloadScene()
    {
        //_pausePanelClass.SetAudioVolume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region Funcions for animations
    private void ResetAttack()
    {
        _AttackRange.enabled = false;
        CanAttackEnemy = false;
    }
    private void AnimationEvent_ResetRoll()
    {
        _roll = false;
    }

    private void AnimationEvent_RunSound()
    {
        _runSound.pitch = Random.Range(0.8f, 1.1f);
        _runSound.Play();
    }
    #endregion
}