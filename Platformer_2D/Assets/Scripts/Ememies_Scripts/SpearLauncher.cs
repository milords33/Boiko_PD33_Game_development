using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearLauncher : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Spear _spearActive;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _delay;

    private void Shoot()
    {
        Spear Spear = Instantiate(_spearActive, _shootPoint.position, Quaternion.identity);
        Spear.Damage = _damage;
        Spear.Delay = _delay;
        Spear.Speed = _speed;
        Spear.StartFly(transform.up);
    }
}
