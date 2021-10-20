using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

#region lab5
    // Долго занимался рефакторингом, плохо написал код и многое не успел:)
    // 
    // Главная проблема - у нас всё в одном классе PlayerMover.
    // Было бы лучше разбить всё на отдельные классы "Animation, UI, Attack и тд..."
    // 
#endregion

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private float _speed;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    #region Элементы ответственные за прыжок
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsCell;
    [SerializeField] private LayerMask _whatIsPlatform;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private float _jumpForce;
    #endregion

    #region Элементы ответственные за приседание
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private Transform _headChecker;
    [SerializeField] private float _headCheckerRadius;
    #endregion

    [SerializeField] private int _maxHitPoints;
    [SerializeField] private int _maxManaPoints;
    [SerializeField] private EndLevelPortal _portalForEscape;

    
    [Header("Animation")]
    #region Анимации
    [SerializeField] private Animator _animator;
    [SerializeField] private string _walkAnimatorKey;
    [SerializeField] private string _jumpAnimatorKey;
    [SerializeField] private string _crouchAnimatorKey;
    [SerializeField] private string _attackAnimatorKey;
    [SerializeField] private string _castAnimatorKey;
    [SerializeField] private string _hurtAnimatorKey;
    [SerializeField] private string _deathAnimatorKey;
    #endregion


    [Header("UI")]
    [SerializeField] private TMP_Text _coinsAmountText;
    [SerializeField] private Slider _hitPointsBar;
    [SerializeField] private Slider _manaPointsBar;
    [SerializeField] private GameObject _menuPanel;

    [Header("Attack")]
    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private Transform _swordAttackPoint;
    [SerializeField] private float _swordAttackRadius;
    [SerializeField] private int _swordDamage;

    [SerializeField] private Transform _skillCastPoint;
    [SerializeField] private LineRenderer _castLine;
    [SerializeField] private float _skillLength;
    [SerializeField] private int _skillDamage;
    [SerializeField] private int _manaForCast;

    [SerializeField] private bool _faceRight;


    private float _horizontalDirection;
    private float _verticalDirection;

    private bool _jump;
    private bool _crawl;
    // private bool _cast;
    private bool _hurt;
    private bool _death;

    private int _currentHitPoints;
    private int _currentManaPoints;
    private int _coinsAmount;
    private float _lastPushTime;

    private bool _needToAttack;
    private bool _needToCast;

    private bool _checkActiveMenuPanel = false;
    private bool checkEnoughMoney = false;

    public bool CanClimb { private get; set; }

    public int CoinsAmount
    {
        get => _coinsAmount;
        set
        {
            _coinsAmount = value;
            _coinsAmountText.text = value.ToString();
        }
    }

    private int CurrentHitPoints
    {
        get => _currentHitPoints;
        set
        {
            _currentHitPoints = value;
            _hitPointsBar.value = _currentHitPoints;
        }
    }

    public int CurrentManaPoints
    {
        get => _currentManaPoints;
        set
        {
            _currentManaPoints = value;
            _manaPointsBar.value = _currentManaPoints;
        }
    }

    private void Start()
    {
        _menuPanel.SetActive(_checkActiveMenuPanel);

        Vector2 vector = new Vector2(10, 11);

        _hitPointsBar.maxValue = _maxHitPoints;
        CurrentHitPoints = _maxHitPoints;

        _manaPointsBar.maxValue = _maxManaPoints;
        CurrentManaPoints = _maxManaPoints;

        // AttackDamage = _attackDamage;
        // _AttackRange.enabled = false;
        // CanAttackEnemy = false;
        _death = false;

        CoinsAmount = 0;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_death)
        {
            if (_animator.GetBool(_hurtAnimatorKey))
            {
                return;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                _needToAttack = true;
            }

            if (Input.GetButtonDown("Fire2") && CurrentManaPoints >= _manaForCast)
            {
                _needToCast = true;
                CurrentManaPoints -= _manaForCast;
            }

            #region Движение
            _verticalDirection = Input.GetAxisRaw("Vertical");
            _horizontalDirection = Input.GetAxisRaw("Horizontal");
            _animator.SetFloat(_walkAnimatorKey, Mathf.Abs(_horizontalDirection));

            if (_horizontalDirection > 0 && !_faceRight)
                Flip();
            else if (_horizontalDirection < 0 && _faceRight)
                Flip();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jump = true;
            }

            _crawl = Input.GetKey(KeyCode.C);

            #endregion
            if (!checkEnoughMoney)
                checkEnoughMoney = _portalForEscape.CompareCoins(CoinsAmount);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _menuPanel.SetActive(!_checkActiveMenuPanel);
                _checkActiveMenuPanel = !_checkActiveMenuPanel;
            }
        }
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    private void FixedUpdate()
    {
        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);

        if (_animator.GetBool(_hurtAnimatorKey))
        {
            if (canJump && Time.time - _lastPushTime > 0.2f)
            {
                _hurt = false;
                _animator.SetBool(_hurtAnimatorKey, _hurt);
            }

            _needToAttack = false;
            _needToCast = false;
            return;
        }

        if (CanClimb)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _verticalDirection * _speed);
            _rigidbody.gravityScale = 0;
        }
        else
        {
            _rigidbody.gravityScale = 2;
        }

        bool canStand = !Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsCell);
        Collider2D coll = Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsCell);
        _headCollider.enabled = !_crawl && canStand;

        if (_jump && canJump)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }

        _animator.SetBool(_jumpAnimatorKey, !canJump);
        _animator.SetBool(_crouchAnimatorKey, !_headCollider.enabled);

        if (!_headCollider.enabled)
        {
            _needToAttack = false;
            _needToCast = false;
        }

        if (_needToAttack)
        {
            StartAttack();
            _horizontalDirection = 0;
        }

        if (_needToCast)
        {
            StartCast();
            _horizontalDirection = 0;
        }

        _rigidbody.velocity = new Vector2(_horizontalDirection * _speed, _rigidbody.velocity.y);
    }

    // Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_swordAttackPoint.position, new Vector3(_swordAttackRadius, _swordAttackRadius, 0));
    }

    private void StartAttack()
    {
        if (_animator.GetBool(_attackAnimatorKey))
        {
            return;
        }

        _animator.SetBool(_attackAnimatorKey, true);
    }

    private void Attack()
    {
        Collider2D[] targets = Physics2D.OverlapBoxAll(_swordAttackPoint.position,
            new Vector2(_swordAttackRadius, _swordAttackRadius), _whatIsEnemy);

        foreach (var target in targets)
        {
            DragonEnemy dragonEnemy = target.GetComponent<DragonEnemy>();
            if (dragonEnemy != null)
            {
                dragonEnemy.TakeDamage(_swordDamage);
            }

            Opossum opossum = target.GetComponent<Opossum>();
            if (opossum != null)
            {
                opossum.TakeDamage(_swordDamage);
            }

            MaskMan maskMan = target.GetComponent<MaskMan>();
            if(maskMan != null)
            {
                maskMan.TakeDamage(_swordDamage);
            }

            ArrowLauncher arrowLauncher = target.GetComponent<ArrowLauncher>();
            if (arrowLauncher != null)
            {
                arrowLauncher.TakeDamage(_swordDamage);
            }
        }
        _animator.SetBool(_attackAnimatorKey, false);
        _needToAttack = false;
    }

    private void StartCast()
    {
        if (_animator.GetBool(_castAnimatorKey))
        {
            return;
        }

        _animator.SetBool(_castAnimatorKey, true);
    }

    private void Cast()
    {
        RaycastHit2D[] hits =
            Physics2D.RaycastAll(_skillCastPoint.position, transform.right, _skillLength, _whatIsEnemy);
        foreach (var hit in hits)
        {
            DragonEnemy dragon = hit.collider.GetComponent<DragonEnemy>();
            if (dragon != null)
            {
                dragon.TakeDamage(_skillDamage);
            }

            Opossum opossum = hit.collider.GetComponent<Opossum>();
            if (opossum != null)
            {
                opossum.TakeDamage(_skillDamage);
            }

            MaskMan maskMan = hit.collider.GetComponent<MaskMan>();
            if (maskMan != null)
            {
                maskMan.TakeDamage(_skillDamage);
            }

            ArrowLauncher arrowLauncher = hit.collider.GetComponent<ArrowLauncher>();
            if(arrowLauncher != null)
            {
                arrowLauncher.TakeDamage(_skillDamage);
            }
        }
        _animator.SetBool(_castAnimatorKey, false);
        _castLine.SetPosition(0, _skillCastPoint.position);
        _castLine.SetPosition(1, _skillCastPoint.position + transform.right * _skillLength);
        _castLine.enabled = true;
        _needToCast = false;
        Invoke(nameof(DisableLine), 0.08f);
    }

    private void DisableLine()
    {
        _castLine.enabled = false;
    }

    // Взаимодействие с зельем здоровья
    public void AddHitPoints(int hitPoints)
    {
        int missingHitPoints = _maxHitPoints - CurrentHitPoints;
        int pointToAdd = missingHitPoints > hitPoints ? hitPoints : missingHitPoints;
        StartCoroutine(RestoreHitPoints(pointToAdd));
    }

    private IEnumerator RestoreHitPoints(int pointsToAdd)
    {
        while(pointsToAdd !=0)
        {
            pointsToAdd --;
            CurrentHitPoints++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RestoreManaPoints(int pointsToAdd)
    {
        while (pointsToAdd != 0)
        {
            pointsToAdd--;
            CurrentManaPoints++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddManaPoints(int manaPoints)
    {
        int missingManaPoints = _maxManaPoints - CurrentManaPoints;
        int pointsToAdd = missingManaPoints > manaPoints ? manaPoints : missingManaPoints;
        StartCoroutine(RestoreManaPoints(pointsToAdd));
    }

    public void TakeDamage(int damage, float pushPower = 0, float enemyPosX = 0)
    {
        if (_animator.GetBool(_hurtAnimatorKey))
            return;

        _hurt = true;
        CurrentHitPoints -= damage;
        if (CurrentHitPoints <= 0)
        {
            _speed = 0;
            _death = true;
            _animator.SetBool(_deathAnimatorKey, _death);
            Invoke(nameof(ReloadScene), 0.9f);
        }

        if (pushPower != 0)
        {
            _lastPushTime = Time.time;
            int direction = transform.position.x > enemyPosX ? 1 : 1;
            _rigidbody.AddForce(new Vector2(direction * pushPower / 2, pushPower));
            _animator.SetBool(_hurtAnimatorKey, _hurt);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
