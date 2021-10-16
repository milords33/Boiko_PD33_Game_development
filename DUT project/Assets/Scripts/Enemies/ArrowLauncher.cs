using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _arrowLauncher;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Arrow _arrowShoot;
    [SerializeField] private float _speed;
    [SerializeField] private float _delay;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHitPoints;
    [SerializeField] private Slider _hitPointsBar;

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
        ChangeHp(_maxHitPoints);
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

    private void Shoot()
    {
        Arrow arrow = Instantiate(_arrowShoot, _shootPoint.position, Quaternion.identity);
        arrow.Damage = _damage;
        arrow.Delay = _delay;
        arrow.Speed = _speed;
        arrow.StartFly(transform.right);
    }

    public void TakeDamage(int damage)
    {
        CurrentHitPoints -= damage;
        if (CurrentHitPoints <= 0)
            Destroy(_arrowLauncher);
    }

    private void ChangeHp(int hitPoints)
    {
        _currentHitPoints = hitPoints;
        _hitPointsBar.value = hitPoints;
    }
}