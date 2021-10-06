using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private Rigidbody2D _rigidbody;

    public void StartFly(Vector2 direction)
    {
        _rigidbody.velocity = direction * _speed;
        Invoke(nameof(Destroy), 5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CharacterMovement character = other.GetComponent<CharacterMovement>();
        if (character != null)
        {
            character.TakeDamage(_damage);
        }

        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
