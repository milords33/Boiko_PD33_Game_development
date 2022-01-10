using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private int _pushPower;

    private bool _delay = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerMover player = other.collider.GetComponent<PlayerMover>();
        if (player != null)
        {
            if (!_delay)
            {
                 player.TakeDamage(_damage, _pushPower, transform.position.x);
                _delay = true;
                Invoke(nameof(MadeDelayFalse), 1f);
            }
        }
    }

    private void MadeDelayFalse()
    {
        _delay = false;
    }
}
