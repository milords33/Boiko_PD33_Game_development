using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#region Лабораторная 1 (Виды передвижения)
// case 1 Translate
/* 
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void FixedUpdate()
    {

        if (_direction.x > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_direction.x < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }
        float direction = Input.GetAxisRaw("Horizontal");
        _direction.x = direction;
        transform.Translate(_direction.normalized * _speed);
    }
*/

// case 2 AddForce
/*
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _acceleration;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void FixedUpdate()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _spriteRenderer.flipX = true;
            _direction.x -= _acceleration;
            
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _spriteRenderer.flipX = false;
            _direction.x += _acceleration;
            
        }
        _rb.AddForce(_direction * _acceleration);
    }
*/
// case 3 Impulse
/*
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _acceleration;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _spriteRenderer.flipX = true;
            _direction.x -= _acceleration;

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _spriteRenderer.flipX = false;
            _direction.x += _acceleration;

        }
        _rb.AddForce(_direction * _acceleration, ForceMode2D.Impulse);
*/
// case 4 Lerp
/*
    [SerializeField] Vector2 startPosition;
    [SerializeField] Vector2 endPosition;
    [SerializeField] private float step;
    [SerializeField] private float progress;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Start()
    {
        transform.position = startPosition;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            startPosition = transform.position;
            endPosition.x += direction;
            //endPosition.y = 0;
            transform.position = Vector2.Lerp(startPosition, endPosition, step);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            startPosition = transform.position;
            _spriteRenderer.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            startPosition = transform.position;
            _spriteRenderer.flipX = false;
        }
    }
*/
// case 5 In our project
/*
     private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;  
    [SerializeField] private SpriteRenderer _spriteRenderer;
     private float _direction;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");

        if (_direction > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_direction < 0 && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction * _speed, _rigidbody.velocity.y);//x=1, y=0
    }
*/
#endregion

[RequireComponent(typeof(Rigidbody2D))]

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private float _speed;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    #region Элементы ответственные за прыжок
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsCell;
    #endregion

    #region Элементы ответственные за приседание
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headCheckerRadius;
    [SerializeField] private Transform _headChecker;
    #endregion

    [SerializeField] private int _maxHP;
    [SerializeField] private int _maxMP;

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
    [SerializeField] private Slider _hpBar;
    [SerializeField] private Slider _mpBar;


    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _jump;
    private bool _crawl;
    private bool _attack;
    private bool _cast;
    private bool _hurt;
    private bool _death;
    private int _currentHP;
    private int _currentMP;
    private int _coinsAmount;

    private float _lastPushTime;

    public int CoinsAmount
    {
        get => _coinsAmount;
        set
        {
            _coinsAmount = value;
            _coinsAmountText.text = value.ToString();
        }
    }
    public bool CanClimb  { private get; set; }

    private bool checkEnoughMoney = false;

    private int CurrentHP
    {
        get => _currentHP;
        set
        {
            _currentHP = value;
            _hpBar.value = _currentHP;
        }
    }
    public int CurrentMP
    {
        get => _currentMP;
        set
        {
            _currentMP = value;
            _mpBar.value = _currentMP;
        }
    }

    private void Start()
    {
        _hpBar.maxValue = _maxHP;
        CurrentHP = _maxHP;

        _mpBar.maxValue = _maxMP;
        CurrentMP = _maxMP;

        CoinsAmount = 0;
        _death = false;
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!_death)
        {
            _verticalDirection = Input.GetAxisRaw("Vertical");


            _horizontalDirection = Input.GetAxisRaw("Horizontal");
            _animator.SetFloat(_walkAnimatorKey, Mathf.Abs(_horizontalDirection));

            if (_horizontalDirection > 0 && _spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = false;
            }
            else if (_horizontalDirection < 0 && !_spriteRenderer.flipX)
            {
                _spriteRenderer.flipX = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
                _jump = true;

            _crawl = Input.GetKey(KeyCode.C);
            _attack = Input.GetKey(KeyCode.Q);
            _cast = Input.GetKey(KeyCode.E);

            if (!checkEnoughMoney)
                checkEnoughMoney = _portalForEscape.CompareCoins(CoinsAmount);
        }

    }

    private void FixedUpdate()
    {
        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);

        if (_animator.GetBool(_hurtAnimatorKey))
        {
            if (Time.time - _lastPushTime > 0.2f && canJump)
            {
                _hurt = false;
                _animator.SetBool(_hurtAnimatorKey, _hurt);
            }
            return;
        }

        // передвижение
        _rigidbody.velocity = new Vector2(_horizontalDirection * _speed, _rigidbody.velocity.y);

        // Возможность подниматься по лестнице
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

        _headCollider.enabled = !_crawl && canStand;

        // Условие прыжка
        if (_jump && canJump)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _jump = false;
        }

        // Атака 
        

        _animator.SetBool(_jumpAnimatorKey, !canJump);
        _animator.SetBool(_crouchAnimatorKey, !_headCollider.enabled);
        _animator.SetBool(_attackAnimatorKey, _attack);
        _animator.SetBool(_castAnimatorKey, _cast);

        if (_attack)
            _attack = false;
        if(_cast)
            _cast = false;
        //if(_hurt)
        //    _hurt = false;
    }

    // Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
    }

    // Взаимодействие с зельем здоровья
    public void AddHitPoints(int hitPoints)
    {
        Debug.Log($"Hit points started to grow");
        int missingHP = _maxHP - CurrentHP;
        int pointToAdd = missingHP > hitPoints ? hitPoints : missingHP;
        StartCoroutine(RestoreHP(pointToAdd));
    }

    private IEnumerator RestoreHP(int pointsToAdd)
    {
        while(pointsToAdd !=0)
        {
            pointsToAdd --;
            CurrentHP++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RestoreMP(int pointsToAdd)
    {
        while (pointsToAdd != 0)
        {
            pointsToAdd--;
            CurrentMP++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddManaPoints(int manaPoints)
    {
        Debug.Log($"Mana started to grow");
        int missingMP = _maxMP - CurrentMP;
        int pointToAdd = missingMP > manaPoints ? manaPoints : missingMP;
        StartCoroutine(RestoreMP(manaPoints));
    }

    public void TakeDamage(int damage, float pushPower = 0, float enemyPosX = 0)
    {
        if (_animator.GetBool(_hurtAnimatorKey))
            return;

        _hurt = true;

        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            _speed = 0;
            _death = true;
            _animator.SetBool(_deathAnimatorKey, _death);
            Invoke(nameof(ReloadScene), 0.9f);
        }

        if(pushPower != 0)
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
