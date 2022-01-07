using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _arrowLauncher;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private TrapArrow _arrowShoot;
    [SerializeField] private float _speed;
    [SerializeField] private float _delay;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHitPoints;
    [SerializeField] private Slider _hitPointsBar;

    [SerializeField] private GameObject _destroyEffect;

    private bool _destroyed;

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

    private void Shoot()
    {
        if (!_destroyed)
        {
            TrapArrow trapArrow = Instantiate(_arrowShoot, _shootPoint.position, Quaternion.identity);
            trapArrow.Damage = _damage;
            trapArrow.Delay = _delay;
            trapArrow.Speed = _speed;
            trapArrow.StartFly(transform.up);
        }
    }

    public void TakeDamage(int damage)
    {
        ChangeHitPoints(_currentHitPoints - damage);
    }

    private void ChangeHitPoints(int hitPoints)
    {
        _currentHitPoints = hitPoints;
        if (_currentHitPoints <= 0)
        {
            Instantiate(_destroyEffect, transform.position, Quaternion.identity);
            Destroy(_arrowLauncher);
        }
        _hitPointsBar.value = hitPoints;
    }

}
