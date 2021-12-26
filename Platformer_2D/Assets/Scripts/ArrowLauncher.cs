using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private TrapArrow _arrowShoot;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _delay;

    private void Shoot()
    {
        TrapArrow trapArrow = Instantiate(_arrowShoot, _shootPoint.position, Quaternion.identity);
        trapArrow.Damage = _damage;
        trapArrow.Delay = _delay;
        trapArrow.Speed = _speed;
        trapArrow.StartFly(transform.up);
    }
}
