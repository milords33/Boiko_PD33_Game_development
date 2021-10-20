using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _enemySystem;
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
        ChangeHitPoints(_maxHitPoints);
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
        ChangeHitPoints(_currentHitPoints - damage);
    }

    private void ChangeHitPoints(int hitPoints)
    {
        _currentHitPoints = hitPoints;
        if (_currentHitPoints <= 0)
        {
            Destroy(_enemySystem);
        }
        _hitPointsBar.value = hitPoints;
    }
}