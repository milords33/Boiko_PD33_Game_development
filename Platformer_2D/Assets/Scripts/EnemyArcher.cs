using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    [SerializeField] private string _shootAnimatonKey;
    [SerializeField] private Transform _shootPoint;

    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private GameObject _archer;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackRange;
    [SerializeField] private Arrow _arrowShoot;
    [SerializeField] private bool _faceRight;
    [SerializeField] private int _maxHp;

    [Header("Sounds")]
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _hitSound;

    //[SerializeField] private Slider _hpBar;

    private int _currentHP;
    private bool _canShoot;
    private bool _death;

    private void Start()
    {
        //_hpBar.maxValue = _maxHp;
        ChangeHp(_maxHp);
    }

    private int CurrentHP
    {
        get => _currentHP;
        set
        {
            _currentHP = value;
           // _hpBar.value = _currentHP;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerMover player = other.collider.GetComponent<PlayerMover>();
        if (player != null)
        {
            if (player.CanAttackEnemy)
                TakeDamage(player.AttackDamage);
        }
    }
        private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_attackRange * 2, 1, 0));
    }

    private void ChangeHp(int hp)
    {
        _currentHP = hp;
       // _hpBar.value = hp;
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
        if (player != null)
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
            _faceRight = !_faceRight;
        }
        _animator.SetBool(_shootAnimatonKey, true);
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        _hitSound.Play();
        if (CurrentHP <= 0)
            Destroy(_archer);
    }

    #region Funcions for animations
    private void AnimationEvent_ShootSound()
    {
        _shootSound.pitch = Random.Range(0.8f, 1.1f);
        _shootSound.Play();
    }

    private void Shoot()
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
    #endregion

}
