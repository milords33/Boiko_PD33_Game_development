using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogNinja : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _frogNinja;
    [SerializeField] private int _maxHitPoints;

    private Vector2 _startPostion;
    private int _currentHitPoints;

    private int CurrentHitPoints
    {
        get => _currentHitPoints;
        set
        {
            _currentHitPoints = value;
        }
    }

    private void Start()
    {
        _currentHitPoints = _maxHitPoints;
        _startPostion = transform.position;
    }



}
