using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromMainMenu : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _idleAnimatorKey;

    private float _speed;   

    private void Update()
    {
            _rigidbody.velocity = transform.right * _speed;  
    }

    public void AnimationEventRun()
    {
        _animator.SetBool(_runAnimatorKey, true);
        _speed = 4.4f;
    }    

    public void AnimationEventIdle()
    {
        _animator.SetBool(_runAnimatorKey, false);
        _animator.SetBool(_idleAnimatorKey, true);
        _speed = 0f;
    }
}
