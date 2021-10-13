using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField] private int _poisonDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();

        if (player != null)
        {
            player.TakeDamage(_poisonDamage);
            Destroy(gameObject);
        }
    }
}
