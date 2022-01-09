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

    [Header("Player preferences")]
    [SerializeField] private float _speed;
    [SerializeField] private float _rollForce;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsCell;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _topBodyCheckerRadius;

    [Header("PlayerElements")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _topBodyCollider;
    [SerializeField] private Transform _topBodyChecker;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private MagicWave  _magicWave;

    [Header("Attack")]
    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private Transform _swordAttackPoint;
    [SerializeField] private float _swordAttackPushPower;
    [SerializeField] private float _swordAttackRadius;
    [SerializeField] private int _manaForCast;

    [SerializeField] private bool _faceRight;

    private bool _needToAttack;


    [Header("Effects")]
    [SerializeField] private GameObject _groundEffect;
    [SerializeField] private GameObject _hitEffect;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpBoolAnimatorKey;
    [SerializeField] private string _jumpTriggerAnimatorKey;
    [SerializeField] private string _rollBoolAnimatorKey;
    [SerializeField] private string _rollTriggerAnimatorKey;
    [SerializeField] private string _blockAnimatorKey;
    [SerializeField] private string _blockIdleAnimatorKey;
    [SerializeField] private string _magicWaveAnimatorKey;
    [SerializeField] private string _hurtAnimatorKey;
    [SerializeField] private string _deathAnimatorKey;
    [SerializeField] private string _attackAnimatorKey;

    [Header("UI")]
    [SerializeField] private TMP_Text _coinsAmountText;
    [SerializeField] private Slider _hitPointsSlider;
    [SerializeField] private Slider _shieldPointsSlider;
    [SerializeField] private Slider _manaPointsSlider;
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
    [SerializeField] private AudioSource _magicSound;


    private int _maxHitPoints;
    private int _maxManaPoints;
    private int _maxShieldPoints;
    private int _attackDamage;

    private float _horizontalDirection;
    private bool _roll = false;
    private bool _jump;

    private int _currentAttack = 0;
    private float _timeSinceAttack = 0.0f;
    private float _lastPushTime;

    private const int SHIELD_PROTECT_POINTS = 25;
    private const int STANDART_CHARACTERISTICS = 100;
    private int _currentShieldPoints;
    private int _currentHitPoints;
    private int _currentManaPoints;

    private bool _magicCast = false;
    private bool _shieldActive;
    private bool _hurt;
    private bool _death;

    private int _coinsAmount;
    private bool _checkActiveMenuPanel = false;
    private bool _trade = false;

    public float SwordAttackPushPower { get; set; }

    public int CoinsAmount
    {
        get => _coinsAmount;
        set
        {
            _coinsAmount = value;
            _coinsAmountText.text = value.ToString();
        }
    }

    public int MaxHitPoints
    {
        get => _maxHitPoints;
        set
        {
            _maxHitPoints = value;
        }
    }

    public int MaxManaPoints
    {
        get => _maxManaPoints;
        set
        {
            _maxManaPoints = value;
        }
    }

    public int MaxShieldPoints
    {
        get => _maxShieldPoints;
        set
        {
            _maxShieldPoints = value;
        }
    }

    public int CurrentHitPoints
    {
        get => _currentHitPoints;
        set
        {
            _currentHitPoints = value;
             _hitPointsSlider.value = _currentHitPoints;
        }
    }

    public int CurrentShieldPoints
    {
        get => _currentShieldPoints;
        set
        {
            _currentShieldPoints = value;
            _shieldPointsSlider.value = _currentShieldPoints;
        }
    }

    public int CurrentManaPoints
    {
        get => _currentManaPoints;
        set
        {
            _currentManaPoints = value;
            _manaPointsSlider.value = _currentManaPoints;
        }
    }

    public int AttackDamage
    {
        get => _attackDamage;
        set
        {
            _attackDamage = value;
        }
    }

    public bool Trade
    {
        get => _trade;
        set
        {
            _trade = value;
        }
    }

    private void Awake()
    {
        SetCharacteristics();
    }

    private void LoadData()
    {
        _hitPointsSlider.maxValue = _maxHitPoints;
        _shieldPointsSlider.maxValue = _maxShieldPoints;
        _manaPointsSlider.maxValue = _maxManaPoints;


        SwordAttackPushPower = _swordAttackPushPower;
        CurrentShieldPoints = _maxShieldPoints;

        if (PlayerPrefs.HasKey("HitPoints"))
        {
            CurrentHitPoints = PlayerPrefs.GetInt("HitPoints");
            int count = System.Convert.ToInt32(_hitPointsSlider.maxValue / 25 - 4);
            RectTransform rectTransfrom = _hitPointsSlider.GetComponent<RectTransform>();
            for (int i = 0; i < count; i++)
            {
                rectTransfrom.sizeDelta += new Vector2(15f, 0);
                rectTransfrom.localPosition += new Vector3(22.5f, 0, 0);
            }
        }
        else
            CurrentHitPoints = _maxHitPoints;

        if (PlayerPrefs.HasKey("ManaPoints"))
        {
            CurrentManaPoints = PlayerPrefs.GetInt("ManaPoints");
            int count = System.Convert.ToInt32(_manaPointsSlider.maxValue / 25 - 4);
            RectTransform rectTransfrom = _manaPointsSlider.GetComponent<RectTransform>();
            for (int i = 0; i < count; i++)
            {
                rectTransfrom.sizeDelta += new Vector2(15f, 0);
                rectTransfrom.localPosition += new Vector3(22.5f, 0, 0);
            }
        }
        else
            CurrentManaPoints = _maxManaPoints;

        if (PlayerPrefs.HasKey("CoinsAmount"))
            CoinsAmount = PlayerPrefs.GetInt("CoinsAmount");
        else
            CoinsAmount = 0;
    }

    private void Start()
    {   
        //_pausePanelClass.GetAudioVolume();
        _death = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        LoadData();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
            ReloadScene();

        if (!_death && !_magicCast)
        {
            #region Movement
            _horizontalDirection = Input.GetAxisRaw("Horizontal");
            _animator.SetFloat(_runAnimatorKey, Mathf.Abs(_horizontalDirection));

            if (_horizontalDirection > 0 && !_faceRight)
                Flip();
            else if (_horizontalDirection < 0 && _faceRight )
                Flip();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jump = true;
                _jumpSound.Play();
            }

            if (Input.GetKeyDown(KeyCode.C) && !_roll)
            {
                _roll = true;
                _rollSound.Play();
                _animator.SetTrigger(_rollTriggerAnimatorKey);
                if (!_faceRight)
                    _rigidbody.velocity = new Vector2(-1 * _rollForce, _rigidbody.velocity.y);
                else
                    _rigidbody.velocity = new Vector2(1 * _rollForce, _rigidbody.velocity.y);
            }
            #endregion

            #region Attacks
            _timeSinceAttack += Time.deltaTime;
            if (Input.GetKey(KeyCode.Mouse0) && _timeSinceAttack > 0.25f && !_roll && !_trade)
            {
                _swingSound.Play();
                _needToAttack = true;
                _currentAttack++;

                if (_currentAttack > 3)
                    _currentAttack = 1;
                if (_timeSinceAttack > 1.0f)
                    _currentAttack = 1;

                _animator.SetTrigger("Attack " + _currentAttack);
                _timeSinceAttack = 0.0f;
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.F) && CurrentManaPoints >= _manaForCast)
                _animator.SetBool(_magicWaveAnimatorKey, true);

            #region Shield
            if (Input.GetKeyDown(KeyCode.Mouse1) && !_roll)
            {
                _animator.SetTrigger(_blockAnimatorKey);
                _animator.SetBool(_blockIdleAnimatorKey, true);
                _shieldActive = true;
                _shieldActivated.Play();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                _animator.SetBool(_blockIdleAnimatorKey, false);
                _shieldActive = false;
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.Escape) && !_trade)
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
            _needToAttack = false;
            return;
        }


        if (!_topBodyCollider.enabled)
        {
            _needToAttack = false;
        }

        if (_needToAttack)
        {
            StartAttack();
            _horizontalDirection = 0;
        }

        _animator.SetBool(_jumpBoolAnimatorKey, !canJump);
        _animator.SetBool(_rollBoolAnimatorKey, _roll);
    }

    private void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_topBodyChecker.position, _topBodyCheckerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_swordAttackPoint.position, new Vector3(_swordAttackRadius, _swordAttackRadius, 0));
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
        CurrentShieldPoints += SHIELD_PROTECT_POINTS;
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            EnemyArcher archer = target.GetComponent<EnemyArcher>();
            if (archer != null)
                archer.TakeDamage(_attackDamage);

            Bandits bandit = target.GetComponent<Bandits>();
            if (bandit != null) 
                bandit.TakeDamage(_attackDamage, _swordAttackPushPower, transform.position.x);
            
            ArrowLauncher arrowLauncer = target.GetComponent<ArrowLauncher>();
            if (arrowLauncer != null)
                arrowLauncer.TakeDamage(_attackDamage);

            FlyingEye eye = target.GetComponent<FlyingEye>();
            if (eye != null)
                eye.TakeDamage(_attackDamage);

            BloodySkeleton bloodySkeleton = target.GetComponent<BloodySkeleton>();
            if (bloodySkeleton != null)
                bloodySkeleton.TakeDamage(_attackDamage);
        }
        _animator.SetBool(_attackAnimatorKey, false);
        _needToAttack = false;
    }

    private void SetCharacteristics()
    {
        if (PlayerPrefs.HasKey("MaxHitPoints"))
            _maxHitPoints = PlayerPrefs.GetInt("MaxHitPoints");
        else
            _maxHitPoints = STANDART_CHARACTERISTICS;

        if (PlayerPrefs.HasKey("MaxManaPoints"))
            _maxManaPoints = PlayerPrefs.GetInt("MaxManaPoints");
        else
            _maxManaPoints = STANDART_CHARACTERISTICS;

        if (PlayerPrefs.HasKey("MaxShieldPoints"))
            _maxShieldPoints = PlayerPrefs.GetInt("MaxShieldPoints");
        else
            _maxShieldPoints = STANDART_CHARACTERISTICS;

        if (PlayerPrefs.HasKey("AttackDamage"))
            _attackDamage = PlayerPrefs.GetInt("AttackDamage");
        else
            _attackDamage = STANDART_CHARACTERISTICS/2;
    }


    #region Funcions for animations
    private void AnimationEventResetRoll()
    {
        _roll = false;
    }

    private void AnimationEventRunSound()
    {
        Instantiate(_groundEffect, transform.position + new Vector3(0,0.15f,0), Quaternion.identity);
        _runSound.pitch = Random.Range(0.8f, 1.1f);
        _runSound.Play();
    }

    private void AnimationEventStartCastMagic()
    {
        _magicCast = true;
        _magicSound.Play();
        CurrentManaPoints -= _manaForCast;
    }

    private void AnimationEventEndCastMagic()
    {
        MagicWave magicWave = Instantiate(_magicWave, _shootPoint.position, Quaternion.identity);
        magicWave.StartFly(_faceRight);
        _magicCast = false;
        _animator.SetBool(_magicWaveAnimatorKey, false);
    }
    #endregion

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

        if (_shieldActive && CurrentShieldPoints > 0)
        {
            CurrentShieldPoints -= SHIELD_PROTECT_POINTS;
            _shieldProtected.Play();
            Invoke(nameof(RestoreShieldPoints), 15f);
        }
        else
        {
            CurrentHitPoints -= damage;
            _hurtSound.Play();
            _hurt = true;
            _hitSound.Play();
            Instantiate(_hitEffect, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }

        if (CurrentHitPoints <= 0)
        {
            _speed = 0;
            _death = true;
            _deathSound.Play();
            _animator.SetBool(_deathAnimatorKey, _death);
            Invoke(nameof(ReloadScene), 2f);
        }

        if (pushPower != 0)
        {
            _lastPushTime = Time.time;
            int direction = transform.position.x > enemyPosX ? 1 : -1;
            _rigidbody.AddForce(new Vector2(direction * pushPower, pushPower / 6));
            _animator.SetBool(_hurtAnimatorKey, _hurt);
        }
    }
    // for spikes
    public void TakeDamage(int damage)
    {
        Instantiate(_hitEffect, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        CurrentHitPoints -= damage;
        _hurtSound.Play();
        _hurt = true;

        if (CurrentHitPoints <= 0)
        {
            _speed = 0;
            _death = true;
            _deathSound.Play();
            _animator.SetBool(_deathAnimatorKey, _death);
            Invoke(nameof(ReloadScene), 2f);
        }
    }
    public void ChangeSliderValue(string sliderName, int value)
    {
        if (sliderName == _hitPointsSlider.gameObject.name)
        {
            _maxHitPoints += value;
            _hitPointsSlider.maxValue = _maxHitPoints;
            RectTransform rectTransfrom = _hitPointsSlider.GetComponent<RectTransform>();
            rectTransfrom.sizeDelta += new Vector2(15f, 0);
            rectTransfrom.localPosition += new Vector3(22.5f, 0, 0);
        }
        else if (sliderName == _manaPointsSlider.gameObject.name)
        {
            _maxManaPoints += value;
            _manaPointsSlider.maxValue = _maxManaPoints;
            RectTransform rectTransfrom = _manaPointsSlider.GetComponent<RectTransform>();
            rectTransfrom.sizeDelta += new Vector2(15f, 0);
            rectTransfrom.localPosition += new Vector3(22.5f, 0, 0);
        }
        else if (sliderName == _shieldPointsSlider.gameObject.name)
        {
            _maxShieldPoints += value;
            _shieldPointsSlider.maxValue = _maxShieldPoints;
            RectTransform rectTransfrom = _shieldPointsSlider.GetComponent<RectTransform>();
            rectTransfrom.sizeDelta += new Vector2(15f, 0);
            rectTransfrom.localPosition += new Vector3(22.5f, 0, 0);
        }
        else
            throw new System.Exception("Invalid Slider Name");
    }
}