using System;
using UnityEngine;
using UnityEngine.UI;

public class RangeEnemy : MonoBehaviour
{
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private DragonFire _dragonFire;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _fireAnimatonKey;
    [SerializeField] private bool _faceRight;
    [SerializeField] private Slider _hpBar;
    [SerializeField] private int _maxHp;
    [SerializeField] private GameObject _enemySystem;

    private int _currentHP;

    private bool _canFire;

    private void Start()
    {
        _hpBar.maxValue = _maxHp;
        ChangeHp(_maxHp);
    }
    private int CurrentHP
    {
        get => _currentHP;
        set
        {
            _currentHP = value;
            _hpBar.value = _currentHP;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_attackRange * 2, 1, 0));
    }

    private void ChangeHp(int hp)
    {
        _currentHP = hp;
        _hpBar.value = hp;
    }

    private void FixedUpdate()
    {
        if (_canFire)
        {
            return;
        }
        CheckIfCanFire();
    }

    private void CheckIfCanFire()
    {
        Collider2D character = Physics2D.OverlapBox(transform.position, new Vector2(_attackRange, 1), 0, _whatIsPlayer);
        if (character != null)
        {
            StartFire(character.transform.position);
            _canFire = true;
        }
        else
        {
            _canFire = false;
        }
    }

    private void StartFire(Vector2 position)
    {
        float posX = transform.position.x;
        if (posX < position.x && !_faceRight || posX > position.x && _faceRight)
        {
            transform.Rotate(0, 180, 0);
            _faceRight = !_faceRight;
        }
        _animator.SetBool(_fireAnimatonKey, true);
    }

    public void Fire()
    {
        DragonFire dragonFire = Instantiate(_dragonFire, _muzzle.position, Quaternion.identity);
        dragonFire.StartFly(transform.right);
        _animator.SetBool(_fireAnimatonKey, false);
        Invoke(nameof(CheckIfCanFire), 1f);
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
            Destroy(_enemySystem);
    }
}