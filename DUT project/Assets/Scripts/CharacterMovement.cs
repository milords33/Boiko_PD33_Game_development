using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region Лабораторная 1 (Виды передвижения)
// case 1 - 2 transform.Traslate(normalize/deltaTime)
/* 
[SerializeField] private float _speed = 3.5f;
void Update()
{
    if (Input.GetKey(KeyCode.D))
    {
        Motion(Vector2.right);
    }
    else if (Input.GetKey(KeyCode.A))
    {
        Motion(Vector2.left);
    }
}

private void Motion(Vector2 direction)
{
    // transform.Translate(direction * _speed * Time.deltaTime);
    // или
    // transform.Translate(direction.normalize * _speed);
}
*/

// case 3 AddForce
/*
{
public Rigidbody2D rb;
public Vector2 direction;
public float acceleration;
    void FixedUpdate()
    {
        rb.AddForce(direction.normalized * acceleration);
    }
}
*/
// case 4 transform.position Lerp
/*
public Vector2 startPosition;
public Vector2 endPosition;
public float step;
private float progress;

    void Start()
    {
        transform.position = startPosition;
    }

    void FixedUpdate()
    {
    transform.position = Vector2.Lerp(startPosition, endPosition, progress);
        progress += step;
    }
*/
#endregion

[RequireComponent(typeof(Rigidbody2D))]

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private float _speed;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    // Прыжок 
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private LayerMask _whatIsCell;

    // Присесть
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headCheckerRadius;
    [SerializeField] private Transform _headChecker;

    [SerializeField] private int _hitPoints;
    [SerializeField] private int _manaPoints;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _walkAnimatorKey;
    [SerializeField] private string _jumpAnimatorKey;
    [SerializeField] private string _crouchAnimatorKey;
    [SerializeField] private string _attackAnimatorKey;
    [SerializeField] private string _castAnimatorKey;
    [SerializeField] private string _hurtAnimatorKey;

    // Условия анимаций
    private float _horizontalDirection;
    private float _verticalDirection;
    private bool _jump;
    private bool _crawl;
    private bool _attack;
    private bool _cast;
    private bool _hurt;

    public bool CanClimb { get; set; }

    public int HitPoints { get; set; }
    private int tempHitPoints;
    public int ManaPoints { get; set; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        HitPoints = _hitPoints;
        tempHitPoints = HitPoints;
    }
    private void Update()
    {
        _verticalDirection = Input.GetAxisRaw("Vertical");

        
        _horizontalDirection = Input.GetAxisRaw("Horizontal");
        _animator.SetFloat(_walkAnimatorKey, Mathf.Abs(_horizontalDirection));
      
        if (_horizontalDirection > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if(_horizontalDirection < 0 && !_spriteRenderer.flipX)
        {  
            _spriteRenderer.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            _jump = true;

        _crawl = Input.GetKey(KeyCode.C);
        _attack = Input.GetKey(KeyCode.Q);
        _cast = Input.GetKey(KeyCode.E);

        if (HitPoints != tempHitPoints)
        {
            if (HitPoints > tempHitPoints)
            {
                tempHitPoints = HitPoints;
            }
            else
            {
                _hurt = true;
                tempHitPoints = HitPoints;
                Debug.Log("You received damage!");
            }
        }
        
    }

    private void FixedUpdate()
    {
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

        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);
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
        _animator.SetBool(_hurtAnimatorKey, _hurt);

        if (_attack)
        {
            _attack = false;
        }
        if(_cast)
        {
            _cast = false;
        }
        if(_hurt)
        {
            _hurt = false;
        }
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
        Debug.Log($"Hit points was raised {hitPoints}");
        HitPoints += hitPoints;
        Debug.Log($"Character HP = {HitPoints}");
    }

    public void AddManaPoints(int manaPoints)
    {
        Debug.Log($"Mana points was raised  {manaPoints}");
        ManaPoints += manaPoints;
        Debug.Log("Character MP = " + manaPoints);
    }

    public void ReduceHitPoints(int damage)
    {
        Debug.Log($"Hit points was reduced {damage}");
        HitPoints -= damage;
        Debug.Log("Character HP = " + HitPoints);
    }

}
