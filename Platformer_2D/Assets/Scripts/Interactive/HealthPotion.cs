using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int _hitPoints;
    [SerializeField] private GameObject _healEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();

        if (player != null)
        {
            Instantiate(_healEffect, transform.position, Quaternion.identity);
            player.AddHitPoints(_hitPoints);
            Destroy(gameObject);
        }
    }
}